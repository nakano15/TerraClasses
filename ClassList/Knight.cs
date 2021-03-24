using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.ClassList
{
    public class Knight : ClassBase
    {
        public Knight()
        {
            Name = "Knight";
            Description = "Who did you vow to protect?";
            AddSkill(49);
            AddSkill(50);
            AddSkill(51);
            AddSkill(52);
            AddSkill(53);
            AddSkill(54);
            AddSkill(55);
            MaxLevel = 50;
        }

        public override ClassUnlockLoreBase CreateLoreBase()
        {
            ClassUnlockLoreBase Lore = new ClassUnlockLoreBase();
            Lore.AddHuntObjective(Terraria.ID.NPCID.Zombie, 30, "The lost knowledge on how to be ");
            Lore.AddHuntObjective(Terraria.ID.NPCID.GoblinWarrior, 20, "a Knight, is to");
            Lore.AddTalkToNpcObjective(Terraria.ID.NPCID.DyeTrader, "You want to learn what is It to be a Knight? Well, maybe I have something related to that... Check this.", "\nprotect those in need");
            Lore.AddGatherItemObjective(Terraria.ID.ItemID.CopperShortsword, 1, ", and master mounted combat.");
            return Lore;
        }
    }
}
