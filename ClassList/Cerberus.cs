using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.ClassList
{
    public class Cerberus : ClassBase
    {
        public Cerberus()
        {
            Name = "Cerberus";
            Description = "No longer guarding the gates of hell.";
            ClassType = ClassTypes.Aspect;
            MaxLevel = 50;
            AddSkill(22, "", 1);
            AddSkill(25, "");
        }

        public override ClassUnlockLoreBase CreateLoreBase()
        {
            ClassUnlockLoreBase LoreBase = new ClassUnlockLoreBase();
            LoreBase.AddHuntObjective(Terraria.ID.NPCID.Hellbat, 100, "");
            LoreBase.AddHuntObjective(Terraria.ID.NPCID.AngryBones, 200, " ");
            LoreBase.AddGatherItemObjective(Terraria.ID.ItemID.Hellstone, 200, "");
            LoreBase.AddGatherItemObjective(Terraria.ID.ItemID.Obsidian, 150, "");
            LoreBase.AddHuntObjective(Terraria.ID.NPCID.FireImp, 25, "");
            LoreBase.AddHuntObjective(Terraria.ID.NPCID.Guide, 1, "");
            LoreBase.AddHuntObjective(Terraria.ID.NPCID.WallofFlesh, 1, "");
            return LoreBase;
        }
    }
}
