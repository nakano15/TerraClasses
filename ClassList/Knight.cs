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
            AddSkill(56);
            AddSkill(67);
            MaxLevel = 50;
        }

        public override ClassUnlockLoreBase CreateLoreBase()
        {
            ClassUnlockLoreBase Lore = new ClassUnlockLoreBase();
            Lore.AddHuntObjective(Terraria.ID.NPCID.Zombie, 30, "Who do you pledge loyalty to?");
            Lore.AddHuntObjective(Terraria.ID.NPCID.GoblinWarrior, 20, " Vow to protect someone and");
            Lore.AddTalkToNpcObjective(Terraria.ID.NPCID.DyeTrader, "You want to learn what is It to be a Knight? Well, maybe I have something related to that... Check this.", "\nkeep that someone safe");
            Lore.AddGatherItemObjective(Terraria.ID.ItemID.CopperShortsword, 1, " to be a true knight.");
            return Lore;
        }
    }
}
