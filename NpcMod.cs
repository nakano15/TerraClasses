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
        private static Player SpawnPlayer = null;

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

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            SpawnPlayer = spawnInfo.player;
        }

        public override void SetDefaults(NPC npc)
        {
            CPReward = npc.lifeMax / 8 + (npc.damage * 2 + npc.defense) / 2;
            if(MainMod.EnableMonsterStatusBuffingBasedOnClasses && !Main.gameMenu && SpawnPlayer != null)
            {
                ReadjustStatusBasedOnDifficulty(npc, SpawnPlayer.GetModPlayer<PlayerMod>().DifficultyLevel);
            }
        }

        public void ReadjustStatusBasedOnDifficulty(NPC npc, float DifficultyLevel)
        {
            if (npc.lifeMax > 5 || npc.damage > 0)
            {
                int StatusMod = (int)DifficultyLevel;
                npc.lifeMax += (int)(npc.lifeMax * 0.2f * StatusMod);
                npc.damage += (int)(npc.damage * 0.2f * StatusMod);
                npc.defense += (int)(npc.defense * 0.1f * StatusMod);
            }
        }

        public override bool CheckDead(NPC npc)
        {
            if (CPReward > 0)
            {
                bool PlayerGotClassExp = false;
                for (int p = 0; p < 255; p++)
                {
                    if (Main.player[p].active && npc.playerInteraction[p])
                    {
                        if (p == Main.myPlayer)
                            PlayerGotClassExp = true;
                        Main.player[p].GetModPlayer<PlayerMod>().AddClassExp(CPReward);
                    }
                }
                if (PlayerGotClassExp)
                    CombatText.NewText(npc.getRect(), Microsoft.Xna.Framework.Color.Cyan, CPReward + " CP", true);
            }
            if (Main.netMode < 2 && npc.playerInteraction[Main.myPlayer])
            {
                Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().OnMobKill(npc);
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

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            if (npc.HasBuff(Terraria.ID.BuffID.BrokenArmor))
            {
                damage += npc.defense / 2;
            }
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (npc.HasBuff(Terraria.ID.BuffID.BrokenArmor))
            {
                damage += npc.defense / 2;
            }
            foreach(SkillProjectile skproj in SkillData.SkillProjs)
            {
                if(skproj.ProjectilePos == projectile.whoAmI)
                {
                    skproj.skillData.OnSkillProjectileHitNpc(projectile, skproj.Owner, npc, damage, knockback, crit, hitDirection);
                    return;
                }
            }
        }

        public override void NPCLoot(NPC npc)
        {
            if ((npc.type == Terraria.ID.NPCID.Demon || npc.type == Terraria.ID.NPCID.VoodooDemon) && Main.rand.Next(500) == 0)
                Item.NewItem(npc.getRect(), ModContent.ItemType<Items.ClassBooks.CerberusBook>());
            if ((npc.type == Terraria.ID.NPCID.HeadlessHorseman || npc.type == Terraria.ID.NPCID.ArmoredSkeleton || npc.type == Terraria.ID.NPCID.ArmoredViking ||
                npc.type == Terraria.ID.NPCID.RustyArmoredBonesSword || npc.type == Terraria.ID.NPCID.BlueArmoredBonesSword || npc.type == Terraria.ID.NPCID.HellArmoredBonesSword) 
                && Main.rand.Next(500) == 0)
                Item.NewItem(npc.getRect(), ModContent.ItemType<Items.ClassBooks.KnightBook>());
            if ((npc.type == Terraria.ID.NPCID.AngryBones || npc.type == Terraria.ID.NPCID.AngryBonesBig || npc.type == Terraria.ID.NPCID.AngryBonesBigHelmet ||
                npc.type == Terraria.ID.NPCID.AngryBonesBigMuscle) && Main.rand.Next(1000) == 0)
                Item.NewItem(npc.getRect(), ModContent.ItemType<Items.ClassBooks.BersekerBook>());
        }
    }
}
