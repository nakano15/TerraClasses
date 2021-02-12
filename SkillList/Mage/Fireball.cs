using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace TerraClasses.SkillList.Mage
{
    public class Fireball : SkillBase
    {
        public Fireball()
        {
            Name = "Fireball";
            Description = "Launches Fireballs at the mouse direction.\nEach level increases the number of Fireballs.\nFireball causes 60% + 10% per level of damage.";
            MaxLevel = 20;
            Cooldown = GetCooldown(28);
            skillType = Enum.SkillTypes.Active;
        }

        public override void Update(Player player, SkillData data)
        {
            const byte TimeVar = 0;
            if (data.Time == 0)
            {
                data.SetFloat(TimeVar, 20);
            }
            float FireballDelay = data.GetFloat(TimeVar);
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
                Projectile.NewProjectile(SpawnPosition, ShotDirection * ShotSpeed, Terraria.ID.ProjectileID.Flamelash, Damage, 3, player.whoAmI);
                FireballDelay -= FireballDelay * 0.2f;
                if (FireballDelay < 1)
                    FireballDelay = 1;
                data.SetFloat(TimeVar, FireballDelay);
                data.ChangeStep();
            }
            if (data.Step >= data.Level)
                data.EndUse();
        }
    }
}
