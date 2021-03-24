using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace TerraClasses.SkillList.Merchant
{
    public class ThrowCoins : SkillBase
    {
        public ThrowCoins()
        {
            Name = "Throw Coins";
            Description = "Throw coins in front of you.\n" +
                " Coins launched will cause 33% + 4% per level of melee damage.\n" +
                " Uses 1 * Level Silver coins per use.\n" +
                " Coins launched varies between 3~5. Every 3 levels increases coin count.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Active;
            Cooldown = GetTime(45);
        }

        public override void Update(Player player, SkillData data)
        {
            if(!player.SellItem(100 * data.Level, 1))
            {
                CombatText.NewText(player.getRect(), Microsoft.Xna.Framework.Color.Red, "You are broken.");
                data.EndUse(true);
                return;
            }
            int CoinCount = Main.rand.Next(3, 6);
            for(int i = 3; i <= data.Level; i += 3)
            {
                CoinCount++;
            }
            const int ShotRange = 15;
            int ShotDamage = data.GetMeleeDamage(0, 0.33f + 0.04f * data.Level, player);
            for(int i = 0; i < CoinCount; i++)
            {
                Vector2 ShotDirection = new Vector2(Main.mouseX + Main.rand.Next(-ShotRange, ShotRange + 1), Main.mouseY + Main.rand.Next(-ShotRange, ShotRange + 1)) + Main.screenPosition - player.Center;
                ShotDirection.Normalize();
                Projectile.NewProjectile(player.Center, ShotDirection * 8f, Terraria.ID.ProjectileID.SilverCoin, ShotDamage, 0.33f, player.whoAmI);
            }
            data.EndUse(false);
        }
    }
}
