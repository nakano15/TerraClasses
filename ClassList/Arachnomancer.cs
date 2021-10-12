using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.ClassList
{
    public class Arachnomancer : ClassBase
    {
        public Arachnomancer()
        {
            Name = "Arachnomancer";
            Description = "Summon spiders to aid you in combat.";
            MaxLevel = 50;
            AddSkill(57);
            AddSkill(59);
            AddSkill(83);
            AddSkill(84);
            SkillColor = new Microsoft.Xna.Framework.Color(90, 13, 110);

            MaxHealthBonus = 2;
            MaxManaBonus = 4;
            PhysicalDamageBonus = 3;
            MagicalDamageBonus = 1;
            DefenseBonus = 0;
        }
    }
}
