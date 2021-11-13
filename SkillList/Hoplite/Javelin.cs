using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace TerraClasses.SkillList.Hoplite
{
    public class Javelin : SkillBase
    {
        public Javelin()
        {
            Name = "Javelin";
            Description = "Hurl javelins at the mouse direction.\n" +
                " Number of javelins increases with level.\n" +
                " Javelins inflicts 65% + 2% per level of Ranged Damage.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Active;
            Cooldown = 50;
            CastTime = 25;
        }

        public override void Update(Player player, SkillData data)
        {
            if (data.Step == 0 || data.Time >= 30)
            {
                int Stack = 1;
                if (data.Step > 0)
                    Stack += 2;
                if (data.Step > 1)
                    Stack += 3;
                if (data.Step > 2)
                    Stack += 4;
                Vector2 LaunchOffset = new Vector2(0, -8);
                const int ProjectileID = Terraria.ID.ProjectileID.JavelinFriendly;
                const float ProjectileSpeed = 12;
                int Damage = data.GetRangedDamage(0, 0.65f + 0.02f * data.Level, player);
                float LaunchDirection = (GetMousePositionInTheWorld - player.Center + LaunchOffset).ToRotation();
                switch (data.Step)
                {
                    case 0:
                        {
                            Vector2 Velocity = new Vector2((float)Math.Sin(LaunchDirection), (float)Math.Cos(LaunchDirection)) * ProjectileSpeed;
                            Projectile.NewProjectile(player.Center, Velocity, ProjectileID, Damage, 7.6f, player.whoAmI);
                            if (data.Level < Stack)
                            {
                                data.EndUse(false);
                            }
                        }
                        break;
                    case 1:
                        {
                            for (int x = -1; x < 2; x += 2)
                            {
                                float LaunchRotation = LaunchDirection - 0.069813f * x;
                                Vector2 Velocity = new Vector2((float)Math.Sin(LaunchRotation), (float)Math.Cos(LaunchRotation)) * ProjectileSpeed;
                                Projectile.NewProjectile(player.Center, Velocity, ProjectileID, Damage, 7.6f, player.whoAmI);
                            }
                            if (data.Level < Stack)
                            {
                                data.EndUse(false);
                            }
                        }
                        break;
                    case 2:
                        {
                            for (int x = -1; x < 2; x++)
                            {
                                float LaunchRotation = LaunchDirection - 0.174533f * x;
                                Vector2 Velocity = new Vector2((float)Math.Sin(LaunchRotation), (float)Math.Cos(LaunchRotation)) * ProjectileSpeed;
                                Projectile.NewProjectile(player.Center, Velocity, ProjectileID, Damage, 7.6f, player.whoAmI);
                            }
                            if (data.Level < Stack)
                            {
                                data.EndUse(false);
                            }
                        }
                        break;
                    case 3:
                        {
                            for (int x = -3; x < 4; x += 2)
                            {
                                float LaunchRotation = LaunchDirection - 0.10472f * x;
                                Vector2 Velocity = new Vector2((float)Math.Sin(LaunchRotation), (float)Math.Cos(LaunchRotation)) * ProjectileSpeed;
                                Projectile.NewProjectile(player.Center, Velocity, ProjectileID, Damage, 7.6f, player.whoAmI);
                            }
                            data.EndUse(false);
                        }
                        break;
                }
                data.ChangeStep();
            }
        }
    }
}
