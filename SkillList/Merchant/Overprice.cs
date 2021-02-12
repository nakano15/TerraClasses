using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace TerraClasses.SkillList.Merchant
{
    public class Overprice : SkillBase
    {
        public Overprice()
        {
            Name = "Overprice";
            Description = "Increases the price of items you sell by 3,3% per level.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Passive;
        }

        public override void Update(Player player, SkillData data)
        {
            player.GetModPlayer<PlayerMod>().SellValue += player.GetModPlayer<PlayerMod>().SellValue * (0.0033f * data.Level);
        }
    }
}
