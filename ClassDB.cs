using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TerraClasses.ClassList;

namespace TerraClasses
{
    public class ClassDB
    {
        public static ClassBase GetClass(int ID)
        {
            switch (ID)
            {
                case 0:
                    return new Terrarian();
                case 1:
                    return new Fighter();
                case 2:
                    return new Archer();
                case 3:
                    return new Merchant();
                case 4:
                    return new Mage();
                case 5:
                    return new Thief();
                case 6:
                    return new Cleric();
                case 7:
                    return new Cerberus();
                case 8:
                    return new Sharpshooter();
                case 9:
                    return new Berseker();
                case 10:
                    return new Summoner();
                case 11:
                    return new Knight();
                case 12:
                    return new Hoplite();
                case 13:
                    return new Vilemancer();
                case 14:
                    return new Arachnomancer();
            }
            return new ClassBase();
        }
    }
}
