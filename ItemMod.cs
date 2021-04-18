using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace TerraClasses
{
    public class ItemMod : GlobalItem
    {
        public override bool ReforgePrice(Terraria.Item item, ref int reforgePrice, ref bool canApplyDiscount)
        {
            reforgePrice = (int)(Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().ReforgeValue * reforgePrice);
            return base.ReforgePrice(item, ref reforgePrice, ref canApplyDiscount);
        }
    }
}
