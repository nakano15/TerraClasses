using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace TerraClasses.SkillList.Archer
{
    public class ArrowBarrage : SkillBase
    {
        public ArrowBarrage()
        {
            Name = "Arrow Barrage";
            Description = "Shoots 10 arrows at the mouse direciton.\n" +
                " Each arrow causes 60% + 10% per level of damage.";
            MaxLevel = 10;
            Cooldown = GetTime(18);
            skillType = Enum.SkillTypes.Active;
            UnallowOtherSkillUsage = true;
        }

        public override void Update(Player player, SkillData data)
        {
            if (data.Time == 0 && !player.inventory.Take(10).Any(x => x.useAmmo == Terraria.ID.AmmoID.Arrow))
            {
                CombatText.NewText(player.getRect(), Color.Red, "You don't have a bow type weapon.");
                data.EndUse(true);
                return;
            }
            const int ShotDelay = 8;
            const float ShotSpeed = 12;
            if (data.Time >= ShotDelay)
            {
                Vector2 SpawnPosition = player.Center;
                Vector2 MousePosition = Main.screenPosition;
                MousePosition.X += Main.mouseX;
                MousePosition.Y += Main.mouseY;
                Vector2 ShotDirection = MousePosition - SpawnPosition;
                ShotDirection.Normalize();
                int Damage = data.GetMagicDamage(0, 0.6f + 0.1f * data.Level, player);
                int pos = Projectile.NewProjectile(SpawnPosition, ShotDirection * ShotSpeed, Terraria.ID.ProjectileID.WoodenArrowFriendly, Damage, 3, player.whoAmI);
                Projectile proj = Main.projectile[pos];
                proj.noDropItem = true;
                for (int i = 0; i < 10; i++)
                {
                    if (player.inventory[i].useAmmo == Terraria.ID.AmmoID.Arrow)
                    {
                        FakeWeaponUsage(player, i, GetMousePositionInTheWorld, ShotDelay);
                        break;
                    }
                }
                data.ChangeStep();
            }
            if (data.Step >= 10)
                data.EndUse();
        }
    }
}
