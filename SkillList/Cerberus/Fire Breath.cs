using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace TerraClasses.SkillList.Cerberus
{
    public class Fire_Breath : SkillBase
    {
        public Fire_Breath()
        {
            Name = "Fire Breath";
            Description = "Breathes fire at the mouse direction.\nYou will be unable to attack while breathing fire.\nCauses 120% + 15% per level of magic damage.\nFire breath lasts for 4 seconds.";
            skillType = Enum.SkillTypes.Active;
            MaxLevel = 10;
            Cooldown = GetCooldown(40);
            UnallowOtherSkillUsage = true;
        }

        public override void UpdateStatus(Player player, SkillData data)
        {
            player.delayUseItem = true;
        }

        public override void Update(Player player, SkillData data)
        {
            if (data.Time >= 20)
            {
                SkillData sd = PlayerMod.GetPlayerSkillData(player, 22);
                if (sd != null)
                {
                    for(int i = 0; i < 3; i++)
                        sd.SetInteger((byte)i, 30);
                }
                Vector2 ShotSpawnPosition = player.Center;
                ShotSpawnPosition.Y -= 8 * player.gravDir;
                //ShotSpawnPosition.X += player.direction * 12;
                int Damage = data.GetMagicDamage(0, 1.2f + 0.15f * data.Level, player);
                Vector2 ShotDirection = (new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition) - ShotSpawnPosition;
                ShotDirection.Normalize();
                ShotSpawnPosition -= ShotDirection * 40f; //Magic
                if (Main.mouseX + Main.screenPosition.X < player.Center.X)
                    player.direction = -1;
                else
                    player.direction = 1;
                if (data.Time % 5 == 0)
                    Projectile.NewProjectile(ShotSpawnPosition, ShotDirection * 8f, Terraria.ID.ProjectileID.Flames, Damage, 0.7f, player.whoAmI);
            }
            if (data.Time >= 4.2f * 60)
                data.EndUse(false);
        }
    }
}
