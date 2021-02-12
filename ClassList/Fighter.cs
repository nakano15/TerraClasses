using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.ClassList
{
    public class Fighter : ClassBase
    {
        public Fighter()
        {
            Name = "Fighter";
            Description = "If you like fighting in the front row.";
            MaxLevel = 50;
            AddSkill(10);
            AddSkill(4);
            AddSkill(5);
            AddSkill(6);
            AddSkill(7);
            AddSkill(11);
        }

        public override ClassUnlockLoreBase CreateLoreBase()
        {
            ClassUnlockLoreBase LoreBase = new ClassUnlockLoreBase();
            LoreBase.AddHuntObjective(Terraria.ID.NPCID.Zombie, 20, "Doesn't matter how many comes, ");
            LoreBase.AddHuntObjective(Terraria.ID.NPCID.Skeleton, 10, "or how big the fight is, ");
            LoreBase.AddHuntObjective(Terraria.ID.NPCID.EyeofCthulhu, 1, "\nyou should fight them with all your strength.");
            return LoreBase;
        }
    }
}
