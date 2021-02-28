using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            Cooldown = GetCooldown(45, 1);
            skillType = Enum.SkillTypes.Active;
        }

        public override bool BeforeShooting(Terraria.Player player, SkillData data, Terraria.Item weapon, ref int type, ref int damage, ref float knockback, ref Microsoft.Xna.Framework.Vector2 Position, ref float SpeedX, ref float SpeedY)
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
        }
    }
}
