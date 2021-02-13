using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria.ID;

namespace TerraClasses.ClassList
{
    public class Summoner : ClassBase
    {
        public Summoner()
        {
            Name = "Summoner";
            Description = "Be able to call for aid.";
            MaxLevel = 50;
            AddSkill(35);
            AddSkill(36);
            AddSkill(37);
            AddSkill(38);
            AddSkill(39);
            AddSkill(44);
        }

        public override ClassUnlockLoreBase CreateLoreBase()
        {
            ClassUnlockLoreBase LoreBase = new ClassUnlockLoreBase();
            LoreBase.AddHuntObjective(NPCID.Snatcher, 20, "I entrust you the knowledge");
            LoreBase.AddGatherItemObjective(ItemID.VariegatedLardfish, 10, " of invoking creatures");
            LoreBase.AddHuntObjective(NPCID.Hornet, 30, "\nand make them do as you command");
            LoreBase.AddGatherItemObjective(ItemID.BeeWax, 20, ",\nto save the lands from the evil.");
            return LoreBase;
        }
    }
}
