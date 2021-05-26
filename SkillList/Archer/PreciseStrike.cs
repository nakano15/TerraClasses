using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace TerraClasses.SkillList.Archer
{
    public class PreciseStrike : SkillBase
    {
        public PreciseStrike()
        {
            Name = "Precise Strike";
            Description = "Attacks using a Shortsword type weapons are faster.\n" +
                " Speed increases by 10% + 3% per level.\n" +
                " After level 5, also increases Melee Critical Rate by 20%.";
            MaxLevel = 10;
        }

        public override void UpdateStatus(Player player, SkillData data)
        {
            if (player.inventory[player.selectedItem].useStyle == Terraria.ID.ItemUseStyleID.Stabbing)
            {
                player.meleeSpeed += 0.1f + 0.03f * data.Level;
                if (data.Level > 5)
                    player.meleeCrit += 20;
            }
        }
    }
}
