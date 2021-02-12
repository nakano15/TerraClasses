using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace TerraClasses
{
    class ItemMod : GlobalItem
    {
        public override bool UseItem(Terraria.Item item, Terraria.Player player)
        {
            return base.UseItem(item, player);
        }
        
        public override bool ReforgePrice(Terraria.Item item, ref int reforgePrice, ref bool canApplyDiscount)
        {
            reforgePrice = (int)(Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().ReforgeValue * reforgePrice);
            return base.ReforgePrice(item, ref reforgePrice, ref canApplyDiscount);
        }
    }
}
