using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Assault_Cube_Aimbot_DIPL
{
    public class Offsets
    {
        public static int
            //Starting address of matrix, move by 16 floats to get it
            //(had to guess by playing with the numbers)
            viewMatrix = 0x17DFFC-0x6C + 0x4 * 16,
            localPlayer = 0x0018AC00,
            entityList = 0x00191FCC;

        //Offsets for entity class (local player)
        public static int
            head = 0x4,
            feet = 0x28,
            angles = 0x34,
            health = 0xEC,
            dead = 0xB4,
            name = 0x205,
            team = 0x30C,
            currentAmmo = 0x140;
    }
}
