using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.ClassList
{
    public class Vilemancer : ClassBase
    {
        public Vilemancer()
        {
            Name = "Vilemancer";
            Description = "Control the corrupt forces at your will.";
            SkillColor = new Microsoft.Xna.Framework.Color(40, 168, 12);
            MaxLevel = 50;
            AddSkill(86);
            //AddSkill(87); //Disabled due to instability
            AddSkill(88);
            AddSkill(89);
            AddSkill(91);

            MaxHealthBonus = 1;
            MaxManaBonus = 3;
            PhysicalDamageBonus = 2;
            MagicalDamageBonus = 3;
            DefenseBonus = 1;
        }

        public override ClassUnlockLoreBase CreateLoreBase()
        {
            ClassUnlockLoreBase lore = base.CreateLoreBase();
            lore.AddGatherItemObjective(Terraria.ID.ItemID.RottenChunk, 99, "Curious about the disease");
            lore.AddGatherItemObjective(Terraria.ID.ItemID.VileMushroom, 10, " that plagues your land?");
            lore.AddGatherItemObjective(Terraria.ID.ItemID.ShadowScale, 80, " I will teach you");
            lore.AddHuntObjective(Terraria.ID.NPCID.EaterofWorldsBody, 10, " how to control it.");
            return lore;
        }
    }
}
