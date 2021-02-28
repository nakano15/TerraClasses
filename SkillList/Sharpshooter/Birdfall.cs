using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.SkillList.Sharpshooter
{
    public class Birdfall : SkillBase
    {
        public Birdfall()
        {
            Name = "Birdfall";
            Description = "The next shot inflicts extra damage to enemies in the air.\n" +
                " Damage increase is of 60% + 20% per level.\n" +
                " Only works with guns.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Active;
            Cooldown = GetCooldown(45);
        }

        public override bool BeforeShooting(Terraria.Player player, SkillData data, Terraria.Item weapon, ref int type, ref int damage, ref float knockback, ref Microsoft.Xna.Framework.Vector2 Position, ref float SpeedX, ref float SpeedY)
        {
            if (weapon.ammo == Terraria.ID.AmmoID.Bullet)
                type = Terraria.ID.ProjectileID.BulletHighVelocity;
            return true;
        }

        public override void ModifyHitNPCWithProj(Terraria.Player player, SkillData data, Terraria.Projectile proj, Terraria.NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (target.velocity.Y != 0 && proj.type == Terraria.ID.ProjectileID.BulletHighVelocity)
            {
                damage = (int)(damage * (1.6f + 0.2f * data.Level));
                knockback += 2f;
                data.EndUse();
            }
        }
    }
}
