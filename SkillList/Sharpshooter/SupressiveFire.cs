using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace TerraClasses.SkillList.Sharpshooter
{
    public class SupressiveFire : SkillBase
    {
        public SupressiveFire()
        {
            Name = "Supressive Fire";
            Description = "Fires several shots at arrow direction.\nEach shot inflicts 70% + 12% per level of damage.\nAt level 6 and above, armor penetration increases by 10%.\nLevel increases precision.";
            MaxLevel = 10;
            Cooldown = GetCooldown(20);
            CastTime = 30;
            skillType = Enum.SkillTypes.Active;
            UnallowOtherSkillUsage = true;
        }

        public override void Update(Terraria.Player player, SkillData data)
        {
            if (data.Time == 0 && !player.inventory.Take(10).Any(x => x.useAmmo == Terraria.ID.AmmoID.Bullet))
            {
                CombatText.NewText(player.getRect(), Color.Red, "You don't have a gun type weapon.");
                data.EndUse(true);
                return;
            }
            const int ShotType = Terraria.ID.ProjectileID.BulletHighVelocity;
            const int FireDelay = 6;
            const float ShotSpeed = 17f;
            if (data.Time >= FireDelay)
            {
                int MouseSwayMaxDistance = 12 - data.Level / 2;
                Vector2 ShotDestination = new Vector2(Main.mouseX + Main.screenPosition.X, Main.mouseY + Main.screenPosition.Y);
                Vector2 ShotSpawnCenter = player.Center;
                if (MouseSwayMaxDistance > 0)
                {
                    ShotDestination.X += Main.rand.Next(-MouseSwayMaxDistance, MouseSwayMaxDistance + 1);
                    ShotDestination.Y += Main.rand.Next(-MouseSwayMaxDistance, MouseSwayMaxDistance + 1);
                }
                Vector2 ShotDirection = GetDirection(ShotSpawnCenter, ShotDestination) * ShotSpeed;
                int Damage = data.GetRangedDamage(0, 0.7f + 0.12f * data.Level, player);
                Projectile.NewProjectile(ShotSpawnCenter, ShotDirection, ShotType, Damage, 0.4f, player.whoAmI);
                for (int i = 0; i < 10; i++)
                {
                    if (player.inventory[i].useAmmo == Terraria.ID.AmmoID.Bullet)
                    {
                        FakeWeaponUsage(player, i, GetMousePositionInTheWorld, FireDelay);
                        break;
                    }
                }
                data.ChangeStep();
            }
            if (data.Step >= 16)
            {
                data.EndUse();
            }
        }
    }
}
