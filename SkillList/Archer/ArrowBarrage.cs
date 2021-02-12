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
            Name = "Bone Arrow Barrage";
            Description = "Shoots 10 arrows at the mouse direciton.\nEach arrow causes 60% + 10% per level of damage.";
            MaxLevel = 10;
            Cooldown = GetCooldown(20);
            skillType = Enum.SkillTypes.Active;
        }

        public override void Update(Player player, SkillData data)
        {
            if (data.Time == 0 && !player.inventory.Take(10).Any(x => x.useAmmo != Terraria.ID.AmmoID.Arrow))
            {
                CombatText.NewText(player.getRect(), Color.Red, "You don't have a bow type weapon.");
                data.EndUse(true);
                return;
            }
            const float FireballDelay = 8;
            const float ShotSpeed = 12;
            if (data.Time >= FireballDelay)
            {
                Vector2 SpawnPosition = player.Center;
                Vector2 MousePosition = Main.screenPosition;
                MousePosition.X += Main.mouseX;
                MousePosition.Y += Main.mouseY;
                Vector2 ShotDirection = MousePosition - SpawnPosition;
                ShotDirection.Normalize();
                int Damage = data.GetMagicDamage(0, 0.6f + 0.1f * data.Level, player);
                Projectile.NewProjectile(SpawnPosition, ShotDirection * ShotSpeed, Terraria.ID.ProjectileID.BoneArrow, Damage, 3, player.whoAmI);
                data.ChangeStep();
            }
            if (data.Step >= 10)
                data.EndUse();
        }
    }
}
