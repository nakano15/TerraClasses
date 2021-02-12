using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace TerraClasses.SkillList.Merchant
{
    public class Discount : SkillBase
    {
        public Discount()
        {
            Name = "Discount";
            Description = "Reduces the item prices in the stores by 2% per level.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Passive;
        }

        public override void Update(Player player, SkillData data)
        {
            player.GetModPlayer<PlayerMod>().BuyValue -= 0.02f * data.Level;
        }
    }
}
