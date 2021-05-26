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
            MaxLevel = 50;
            AddSkill(58);
            AddSkill(60);
            AddSkill(61);
            AddSkill(62);
            AddSkill(63);
            AddSkill(7);
            AddSkill(69);
        }

        public override ClassUnlockLoreBase CreateLoreBase()
        {
            ClassUnlockLoreBase LoreBase = new ClassUnlockLoreBase();
            LoreBase.AddHuntObjective(Terraria.ID.NPCID.BlueSlime, 10, "You must be sneaky, and");
            LoreBase.AddGatherItemObjective(Terraria.ID.ItemID.Shackle, 5, " have light hands.");
            LoreBase.AddHuntObjective(Terraria.ID.NPCID.Zombie, 15, "\nYou need also to know when");
            LoreBase.AddHuntObjective(Terraria.ID.NPCID.EyeofCthulhu, 1, " you should get away from contact.");
            return LoreBase;
        }
    }
}
