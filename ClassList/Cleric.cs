using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.ClassList
{
    public class Cleric : ClassBase
    {
        public Cleric()
        {
            Name = "Cleric";
            Description = "You got the buffs! You got the healz!";
            SkillColor = new Microsoft.Xna.Framework.Color(0, 255, 243);
            MaxLevel = 50;
            AddSkill(12);
            AddSkill(13);
            AddSkill(34);
            AddSkill(64);
            AddSkill(65);
            AddSkill(66);
            AddSkill(71);

            MaxHealthBonus = 1;
            MaxManaBonus = 4;
            PhysicalDamageBonus = 2;
            MagicalDamageBonus = 3;
            DefenseBonus = 0;
        }

        public override ClassUnlockLoreBase CreateLoreBase()
        {
            ClassUnlockLoreBase LoreBase = new ClassUnlockLoreBase();
            LoreBase.AddHuntObjective(Terraria.ID.NPCID.DemonEye, 7, "Through faith comes the power");
            LoreBase.AddHuntObjective(Terraria.ID.NPCID.Zombie, 12, "\nto aid your allies, and expel");
            LoreBase.AddHuntObjective(Terraria.ID.NPCID.EyeofCthulhu, 1, "\nevil from everyone you want to protect.");
            return LoreBase;
        }
    }
}
