using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.ClassList
{
    public class Thief : ClassBase
    {
        public Thief()
        {
            Name = "Thief";
            Description = "Hit the tank, not me!";
            SkillColor = new Microsoft.Xna.Framework.Color(83, 38, 162);
            MaxLevel = 50;
            AddSkill(58);
            AddSkill(60);
            AddSkill(61);
            AddSkill(62);
            AddSkill(63);
            AddSkill(7);
            AddSkill(69);

            MaxHealthBonus = 3;
            MaxManaBonus = 1;
            PhysicalDamageBonus = 5;
            MagicalDamageBonus = 0;
            DefenseBonus = 1;
        }

        public override ClassUnlockLoreBase CreateLoreBase()
        {
            ClassUnlockLoreBase LoreBase = new ClassUnlockLoreBase();
            LoreBase.AddHuntObjective(Terraria.ID.NPCID.BlueSlime, 10, "You must be sneaky, and");
            LoreBase.AddGatherItemObjective(Terraria.ID.ItemID.Shackle, 3, " have light hands.");
            LoreBase.AddHuntObjective(Terraria.ID.NPCID.Zombie, 12, "\nYou need also to know when");
            LoreBase.AddHuntObjective(Terraria.ID.NPCID.EyeofCthulhu, 1, " you should get away from contact.");
            return LoreBase;
        }
    }
}
