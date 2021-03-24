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
                    return new ClassList.Terrarian();
                case 1:
                    return new ClassList.Fighter();
                case 2:
                    return new ClassList.Archer();
                case 3:
                    return new ClassList.Merchant();
                case 4:
                    return new ClassList.Mage();
                case 5:
                    return new ClassList.Thief();
                case 6:
                    return new ClassList.Cleric();
                case 7:
                    return new ClassList.Cerberus();
                case 8:
                    return new ClassList.Sharpshooter();
                case 9:
                    return new ClassList.Berseker();
                case 10:
                    return new ClassList.Summoner();
                case 11:
                    return new ClassList.Knight();
            }
            return new ClassBase();
        }
    }
}
