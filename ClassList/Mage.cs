using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.ClassList
{
    public class Mage : ClassBase
    {
        public Mage()
        {
            Name = "Mage";
            Description = "Patience rewards with strong spells.";
            MaxLevel = 50;
            AddSkill(8);
            AddSkill(14);
            AddSkill(15);
            AddSkill(16);
            AddSkill(17);
            AddSkill(18);
            AddSkill(46);
            AddSkill(48);
        }

        public override ClassUnlockLoreBase CreateLoreBase()
        {
            ClassUnlockLoreBase LoreBase = new ClassUnlockLoreBase();
            LoreBase.AddGatherItemObjective(Terraria.ID.ItemID.FallenStar, 10, "Channel your magic while");
            LoreBase.AddHuntObjective(Terraria.ID.NPCID.Vulture, 6, " reciting the magic words,");
            LoreBase.AddGatherItemObjective(Terraria.ID.ItemID.LesserManaPotion, 10, "\nyou will be able to control ");
            LoreBase.AddHuntObjective(Terraria.ID.NPCID.EyeofCthulhu, 1, " elements for your multi purpose benefit.");
            return LoreBase;
        }
    }
}
