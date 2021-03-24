using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace TerraClasses.SkillList.Sharpshooter
{
    public class Snipe : SkillBase
    {
        public Snipe()
        {
            Name = "Snipe";
            Description = "Deliver a precise shot the next time you fire a gun.\n" +
                " Inflicts 320% + 16% per level of ranged damage.";
            MaxLevel = 10;
            Cooldown = GetTime(45, 1);
            skillType = Enum.SkillTypes.Active;
            CastTime = 20;
        }

        public override void Update(Player player, SkillData data)
        {
            if (data.Time == 0 && !player.inventory.Take(10).Any(x => x.useAmmo == Terraria.ID.AmmoID.Bullet))
            {
                CombatText.NewText(player.getRect(), Color.Red, "You don't have a gun type weapon.");
                data.EndUse(true);
                return;
            }
            const int ShotType = Terraria.ID.ProjectileID.BulletHighVelocity;
            const float ShotSpeed = 17f;
            Vector2 ShotDestination = new Vector2(Main.mouseX + Main.screenPosition.X, Main.mouseY + Main.screenPosition.Y);
            Vector2 ShotSpawnCenter = player.Center;
            Vector2 ShotDirection = GetDirection(ShotSpawnCenter, ShotDestination) * ShotSpeed;
            int Damage = data.GetRangedDamage(0, 3.2f + 0.16f * data.Level, player);
            int proj = Projectile.NewProjectile(ShotSpawnCenter, ShotDirection, ShotType, Damage, 0.4f, player.whoAmI);
            Projectile proj2 = Main.projectile[proj];
            proj2.maxPenetrate = 3 + data.Level / 5;
            for (int i = 0; i < 10; i++)
            {
                if (player.inventory[i].useAmmo == Terraria.ID.AmmoID.Bullet)
                {
                    FakeWeaponUsage(player, i, GetMousePositionInTheWorld, 20);
                    break;
                }
            }
            data.EndUse(false);
        }

        /*public override bool BeforeShooting(Terraria.Player player, SkillData data, Terraria.Item weapon, ref int type, ref int damage, ref float knockback, ref Microsoft.Xna.Framework.Vector2 Position, ref float SpeedX, ref float SpeedY)
        {
            if (weapon.ammo == Terraria.ID.AmmoID.Bullet)
            {
                damage = data.GetRangedDamage(0, 3.2f + 0.16f * data.Level, player);
                type = Terraria.ID.ProjectileID.BulletHighVelocity;
                knockback = 4f;
                SpeedX *= 1.2f;
                SpeedY *= 1.2f;
                data.EndUse(false);
            }
            return true;
        }*/
    }
}
