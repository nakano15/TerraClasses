using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace TerraClasses
{
    public class NpcMod : GlobalNPC
    {
        public int CPReward = 0;

        public override bool CloneNewInstances
        {
            get
            {
                return false;
            }
        }

        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        public override void SetDefaults(NPC npc)
        {
            CPReward = npc.lifeMax / 8 + (npc.damage * 2 + npc.defense) / 2;
        }

        public override bool CheckDead(NPC npc)
        {
            CombatText.NewText(npc.getRect(), Microsoft.Xna.Framework.Color.Cyan, CPReward + " CP", true);
            for (int p = 0; p < 255; p++)
            {
                if (Main.player[p].active && npc.playerInteraction[p])
                {
                    Main.player[p].GetModPlayer<PlayerMod>().AddClassExp(CPReward);
                    Main.player[p].GetModPlayer<PlayerMod>().OnMobKill(npc);
                }
            }
            return base.CheckDead(npc);
        }
        
        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            switch (type)
            {
                case Terraria.ID.NPCID.Merchant:
                    shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.ClassBooks.MerchantBook>());
                    shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.ClassBooks.FighterBook>());
                    shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.ClassBooks.ArcherBook>());
                    break;
                case Terraria.ID.NPCID.Dryad:
                    shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.ClassBooks.MageBook>());
                    shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.ClassBooks.ClericBook>());
                    shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.ClassBooks.SummonerBook>());
                    break;
                case Terraria.ID.NPCID.ArmsDealer:
                    shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.ClassBooks.SharpshooterBook>());
                    shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.ClassBooks.ArcherBook>());
                    break;
                case Terraria.ID.NPCID.GoblinTinkerer:
                    shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.ClassBooks.ThiefBook>());
                    break;
            }

            float Discount = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().BuyValue;
            foreach (Item i in shop.item)
            {
                if (i.type > 0 && i.value > 1)
                {
                    i.value = (int)(i.value * Discount);
                }
            }
        }

        public override void NPCLoot(NPC npc)
        {
            if (npc.type == Terraria.ID.NPCID.Demon && Main.rand.Next(500) == 0)
                Item.NewItem(npc.getRect(), ModContent.ItemType<Items.ClassBooks.CerberusBook>());
            if ((npc.type == Terraria.ID.NPCID.AngryBones || npc.type == Terraria.ID.NPCID.AngryBonesBig || npc.type == Terraria.ID.NPCID.AngryBonesBigHelmet ||
                npc.type == Terraria.ID.NPCID.AngryBonesBigMuscle) && Main.rand.Next(1000) == 0)
                Item.NewItem(npc.getRect(), ModContent.ItemType<Items.ClassBooks.BersekerBook>());
        }
    }
}
