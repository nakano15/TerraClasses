using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace TerraClasses.SkillList.Merchant
{
    public class Barter : SkillBase
    {
        public Barter()
        {
            Name = "Barter";
            Description = "Reduces the price of Reforging and Healing by 2% per level.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Passive;
        }

        public override void Update(Player player, SkillData data)
        {
            float Change = 0.02f * data.Level;
            player.GetModPlayer<PlayerMod>().ReforgeValue -= Change;
            player.GetModPlayer<PlayerMod>().HealValue -= Change;
        }
    }
}
