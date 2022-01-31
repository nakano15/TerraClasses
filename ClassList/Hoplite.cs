using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.ClassList
{
    public class Hoplite : ClassBase
    {
        public Hoplite()
        {
            Name = "Hoplite";
            Description = "You are not entertained.";
            SkillColor = new Microsoft.Xna.Framework.Color(213, 176, 14);
            MaxLevel = 50;
            AddSkill(80);
            AddSkill(81);
            AddSkill(82);

            MaxHealthBonus = 2;
            MaxManaBonus = 0;
            PhysicalDamageBonus = 5;
            MagicalDamageBonus = 0;
            DefenseBonus = 3;
        }

        public override ClassUnlockLoreBase CreateLoreBase()
        {
            ClassUnlockLoreBase lore = base.CreateLoreBase();
            lore.AddHuntObjective(Terraria.ID.NPCID.GoblinWarrior, 20, "As you have conquered challenges");
            lore.AddHuntObjective(Terraria.ID.NPCID.GoblinArcher, 10, ", you have raised your survival instinct.");
            lore.AddGatherItemObjective(Terraria.ID.ItemID.Javelin, 50, " Now go enjoy your freedom.");
            return lore;
        }
    }
}
