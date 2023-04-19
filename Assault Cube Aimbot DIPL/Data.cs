using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Assault_Cube_Aimbot_DIPL
{
    public class Entity
    {
        public IntPtr baseAddress;
        public Vector3 feet, head;
        public Vector2 viewAngles;
        public float magnitude, viewOffset;
        public int health, team, currentAmmo, dead;
        public string name;
    }

    //4x4 matrix of 16 floats
    public class ViewMatrix
    {
        public float m11, m12, m13, m14;
        public float m21, m22, m23, m24;
        public float m31, m32, m33, m34;
        public float m41, m42, m43, m44;
    }
}
