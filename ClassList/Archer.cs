using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.ClassList
{
    public class Archer : ClassBase
    {
        public Archer()
        {
            Name = "Archer";
            Description = "Good for Bow and Arrow users.";
            AddSkill(1);
            AddSkill(2);
            AddSkill(3);
            AddSkill(9);
            AddSkill(19);
            MaxLevel = 50;
        }

        public override ClassUnlockLoreBase CreateLoreBase()
        {
            ClassUnlockLoreBase LoreBase = new ClassUnlockLoreBase();
            LoreBase.AddGatherItemObjective(Terraria.ID.ItemID.WoodenBow, 1, "An Archer will always need a bow ");
            LoreBase.AddHuntObjective(Terraria.ID.NPCID.DemonEye, 20, "and arrows. \nPull the string");
            LoreBase.AddHuntObjective(Terraria.ID.NPCID.EyeofCthulhu, 1, " to launch the arrow farther, in order to hit your target.");
            return LoreBase;
        }
    }
}
