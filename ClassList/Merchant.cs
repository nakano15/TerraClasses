using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.ClassList
{
    public class Merchant : ClassBase
    {
        public Merchant()
        {
            Name = "Merchant";
            Description = "You seems to really like golds.";
            MaxLevel = 50;
            AddSkill(20); //Barter
            AddSkill(21); //Avarice
            AddSkill(24); //Discount
            AddSkill(45); //Throw Coins
        }

        public override ClassUnlockLoreBase CreateLoreBase()
        {
            ClassUnlockLoreBase LoreBase = new ClassUnlockLoreBase();
            LoreBase.AddTalkToNpcObjective(Terraria.ID.NPCID.Merchant, "You want to learn what is It to be a merchant? Alright, pay me 1 Gold Coin.", "The hidden knowledge to being ");
            LoreBase.AddGatherItemObjective(Terraria.ID.ItemID.GoldCoin, 1, "a merchant is contained here.");
            LoreBase.AddTalkToNpcObjective(Terraria.ID.NPCID.Merchant, "You've got the coin. Now, you need to get me a rare product. An Angel Statue", "\nBuy low, sell high.");
            LoreBase.AddGatherItemObjective(Terraria.ID.ItemID.AngelStatue, 1, " Watch the profit, and the sales.");
            LoreBase.AddTalkToNpcObjective(Terraria.ID.NPCID.Merchant, "Good, now get out. I've got nothing else to say.", "\nThat is the basic of mercantile.");
            return LoreBase;
        }
    }
}
