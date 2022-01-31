using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace TerraClasses.SkillList.Vilemancer
{
    public class Infestation : SkillBase
    {
        public Infestation()
        {
            Name = "Infestation";
            Description = "Mini eaters spawns from your body to attack nearby foes.\n" +
                " Eaters causes 65% + 1.5% per level of summon damage.\n" +
                " Mini eaters spawn waves increases with skill level.";
            skillType = Enum.SkillTypes.Active;
            MaxLevel = 10;
            Cooldown = 40;
        }

        public override void Update(Player player, SkillData data)
        {
            if(data.Time >= 10)
            {
                float Damage = 0.65f + 0.015f * data.Level;
                for(int i = 0; i < 5; i++)
                {
                    Vector2 Velocity = new Vector2(1f, 0);
                    Velocity.Y = Main.rand.NextFloat() * Velocity.X;
                    Velocity.X -= Velocity.Y;
                    if (Main.rand.NextFloat() < 0.5f)
                        Velocity.X *= -1;
                    if (Main.rand.NextFloat() < 0.5f)
                        Velocity.Y *= -1;
                    Projectile.NewProjectile(player.Center, Velocity * 5, Terraria.ID.ProjectileID.TinyEater, data.GetSummonDamage(0, Damage, player), 0.6f, player.whoAmI);
                }
                if (data.Step < data.Level)
                    data.ChangeStep();
                else
                    data.EndUse();
            }
        }
    }
}
