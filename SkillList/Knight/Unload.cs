using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace TerraClasses.SkillList.Knight
{
    public class Unload : SkillBase
    {
        public Unload()
        {
            Name = "Unload";
            Description = "Unloads your firearm at your foes.\n" +
                " Can only be used with guns." +
                " Fires up to 5 + Level / 2 Shots in a row.\n" +
                " Each shot inflicts 80% + 6% per level of ranged damage.\n" +
                " Low knockback.\n" +
                " After level 5, increases movement speed by 5%.";
            MaxLevel = 10;
            Cooldown = GetTime(25);
            CastTime = 20;
            skillType = Enum.SkillTypes.Active;
            UnallowOtherSkillUsage = true;
        }

        public override void UpdateStatus(Player player, SkillData data)
        {
            if (data.Level > 5)
                player.moveSpeed += 0.05f;
        }

        public override void Update(Player player, SkillData data)
        {
            if (data.Time == 0 && !player.inventory.Take(10).Any(x => x.useAmmo == Terraria.ID.AmmoID.Bullet))
            {
                CombatText.NewText(player.getRect(), Color.Red, "You don't have a gun type weapon.");
                data.EndUse(true);
                return;
            }
            const int ShotType = Terraria.ID.ProjectileID.Bullet;
            const int FireDelay = 9;
            const float ShotSpeed = 10f;
            if (data.Time >= FireDelay)
            {
                int MouseSwayMaxDistance = 15 - data.Level / 2;
                Vector2 ShotDestination = new Vector2(Main.mouseX + Main.screenPosition.X, Main.mouseY + Main.screenPosition.Y);
                Vector2 ShotSpawnCenter = player.Center;
                if (MouseSwayMaxDistance > 0)
                {
                    ShotDestination.X += Main.rand.Next(-MouseSwayMaxDistance, MouseSwayMaxDistance + 1);
                    ShotDestination.Y += Main.rand.Next(-MouseSwayMaxDistance, MouseSwayMaxDistance + 1);
                }
                Vector2 ShotDirection = GetDirection(ShotSpawnCenter, ShotDestination) * ShotSpeed;
                int Damage = data.GetRangedDamage(0, 0.8f + 0.06f * data.Level, player);
                int proj = Projectile.NewProjectile(ShotSpawnCenter, ShotDirection, ShotType, Damage, 0.05f, player.whoAmI);
                Projectile proj2 = Main.projectile[proj];
                proj2.maxPenetrate = 3 + data.Level / 5;
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
            if (data.Step >= 5 + data.Level / 2)
            {
                data.EndUse();
            }
        }
    }
}
