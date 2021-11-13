using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.ClassList
{
    public class Vilemancer : ClassBase
    {
        public Vilemancer()
        {
            Name = "Vilemancer";
            Description = "Control the corrupt forces at your will.";
            SkillColor = new Microsoft.Xna.Framework.Color(40, 168, 12);
            MaxLevel = 50;

            MaxHealthBonus = 1;
            MaxManaBonus = 3;
            PhysicalDamageBonus = 2;
            MagicalDamageBonus = 3;
            DefenseBonus = 1;
        }
    }
}
