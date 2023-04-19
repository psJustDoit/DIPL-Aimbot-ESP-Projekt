using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Swed32;

namespace Assault_Cube_Aimbot_DIPL
{
    //Some global variables here
    public class Functions
    {
        public Swed memory;
        public IntPtr moduleBase;

        public Functions()
        {
            memory = new Swed("ac_client");
            moduleBase = memory.GetModuleBase(".exe");
        }

        //Get local player entity
        public Entity ReadLocalPlayer()
        {
            var localPlayer = ReadEntity(memory.ReadPointer(moduleBase, Offsets.localPlayer));
            localPlayer.viewAngles.X = memory.ReadFloat(localPlayer.baseAddress, Offsets.angles);
            localPlayer.viewAngles.Y = memory.ReadFloat(localPlayer.baseAddress, Offsets.angles + 0x4);
            return localPlayer;
        }

        //Read entity at address
        public Entity ReadEntity(IntPtr entityBase)
        {
            var entity = new Entity();
            entity.baseAddress = entityBase;

            entity.currentAmmo = memory.ReadInt(entity.baseAddress, Offsets.currentAmmo);
            entity.health = memory.ReadInt(entity.baseAddress, Offsets.health);
            entity.team = memory.ReadInt(entity.baseAddress, Offsets.team);

            entity.feet = memory.ReadVec(entity.baseAddress, Offsets.feet);
            entity.head = memory.ReadVec(entity.baseAddress, Offsets.head);

            entity.name = Encoding.UTF8.GetString(memory.ReadBytes(entity.baseAddress, Offsets.name, 11));

            return entity;
        }

        //Calculate magnitude of other entities, see which arent on our team and aim at closest one
        public List<Entity> ReadMultipleEntities(Entity localPlayer)
        {
            var entities = new List<Entity>();
            var entityList = memory.ReadPointer(moduleBase, Offsets.entityList);

            //20 because its the maximum number of players in an online server
            for(int i = 0; i < 20; ++i)
            {
                var currEntityBase = memory.ReadPointer(entityList, i * 0x4); //Each entity 4 bytes apart
                var entity = ReadEntity(currEntityBase);
                entity.magnitude = CalculateMagnitude(localPlayer, entity);

                if (entity.health > 0 && entity.health < 101)
                    entities.Add(entity);
            }

            return entities;
        }

        //Calculates length of vector to destination entity
        public static float CalculateMagnitude(Entity localPlayer, Entity destEntity)
        {
            return (float)Math.Sqrt(Math.Pow(destEntity.feet.X - localPlayer.feet.X, 2)
                + Math.Pow(destEntity.feet.Y - localPlayer.feet.Y, 2)
                + Math.Pow(destEntity.feet.Z - localPlayer.feet.Z, 2));
        }

        //Calculate angles needed to turn
        public Vector2 CalculateAngles(Entity localPlayer, Entity destEntity)
        {
            float x, y;

            //Head position
            var deltaX = destEntity.head.X - localPlayer.head.X;
            var deltaY = destEntity.head.Y - localPlayer.head.Y;
            float deltaZ = destEntity.head.Z - localPlayer.head.Z;
            float distance = CalculateDistance(localPlayer, destEntity);

            //Convert radians to degrees, x is offset by 90 degrees from target so +90 compensation
            x = (float)(Math.Atan2(deltaY, deltaX) * 180 / Math.PI) + 90;

            y = (float)(Math.Atan2(deltaZ, distance) * 180 / Math.PI);
            return new Vector2(x, y);
        }

        //Aims local player in direction of enemy entites
        public void DoAiming(Entity entity, float x, float y)
        {
            memory.WriteFloat(entity.baseAddress, Offsets.angles, x);
            //y is 4 bytes away from x
            memory.WriteFloat(entity.baseAddress, Offsets.angles + 0x4, y);
        }

        //Calculates distance to entities
        public static float CalculateDistance(Entity localPlayer, Entity destEntity)
        {
            return (float)Math.Sqrt(Math.Pow(destEntity.feet.X - localPlayer.feet.X, 2)
                + Math.Pow(destEntity.feet.Y - localPlayer.feet.Y, 2));
        }

        //Convert view matrix to 2D coordinates
        //width and height are dimensions of the game window
        public Point WorldToScreen(ViewMatrix ntx, Vector3 position, int width, int height)
        {
            var twoD = new Point();

            float screenW = (ntx.m14 * position.X) + (ntx.m24 * position.Y) 
                + (ntx.m34 * position.Z) + ntx.m44;

            //Used to determine whether or not to draw lines to enemies
            //If its higher than this number then entity is in front of us and lines will be drawn
            if (screenW > 0.001f)
            {
                float screenX = (ntx.m11 * position.X) + (ntx.m21 * position.Y)
                    + (ntx.m31 * position.Z) + ntx.m41;

                float screenY = (ntx.m12 * position.X) + (ntx.m22 * position.Y) + 
                    + (ntx.m32 * position.Z) + ntx.m42;

                float camX = width / 2f;
                float camY = height / 2f;

                float X = camX + (camX * screenX / screenW);
                float Y = camY - (camY * screenY / screenW);

                twoD.X = (int)X;
                twoD.Y = (int)Y;

                return twoD;
            }

            else
            {
                //Offscreen coordinates
                return new Point(-99, -99);
            }
        }

        //Read the local player view matrix
        public ViewMatrix ReadMatrix()
        {
            var viewMatrix = new ViewMatrix();

            //Returns array of 16 floats of the 4x4 view matrix
            var ntx = memory.ReadMatrix(moduleBase + Offsets.viewMatrix);

            viewMatrix.m11 = ntx[0];
            viewMatrix.m12 = ntx[1];
            viewMatrix.m13 = ntx[2];
            viewMatrix.m14 = ntx[3];

            viewMatrix.m21 = ntx[4];
            viewMatrix.m22 = ntx[5];
            viewMatrix.m23 = ntx[6];
            viewMatrix.m24 = ntx[7];

            viewMatrix.m31 = ntx[8];
            viewMatrix.m32 = ntx[9];  
            viewMatrix.m33 = ntx[10];
            viewMatrix.m34 = ntx[11];

            viewMatrix.m41 = ntx[12];
            viewMatrix.m42 = ntx[13];
            viewMatrix.m43 = ntx[14];
            viewMatrix.m44 = ntx[15];

            return viewMatrix;
        }

        //Draws rectangles around entities
        public Rectangle CalculateRect(Point feet, Point head)
        {
            var rect = new Rectangle();

            //Because these are screen coordinates,
            //coordinates at feet are higher than head coordinates
            //Offset by 4 to make the rectangle at middle of entity
            rect.X = head.X - (feet.Y - head.Y) / 4;
            rect.Y = head.Y;

            rect.Width = (feet.Y - head.Y) / 2;
            rect.Height = feet.Y - head.Y;

            return rect;

        }
        
    }
}
