using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace TerraClasses.SkillList.Summoner
{
    public class SpiritFlame : SkillBase
    {
        public SpiritFlame()
        {
            Name = "Spirit Flames";
            Description = "Every summon you have invoked will spawn Spirit Flame to attak nearby foes.\nEach Spirit Flame inflicts 85% + 3% per level Summon Damage.\n 2nd Spirit Flame at level 5.\n 3rd Spirit Flame at level 9.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Active;
            Cooldown = GetCooldown(45, 1);
        }

        public override void Update(Terraria.Player player, SkillData data)
        {
            byte NumberOfFlames = 1;
            if (data.Level >= 5)
                NumberOfFlames++;
            if (data.Level >= 9)
                NumberOfFlames++;
            int Damage = data.GetSummonDamage(0, 0.85f + 0.03f * data.Level, player);
            for (int p = 0; p < Main.maxProjectiles; p++)
            {
                if (Main.projectile[p].active && Main.projectile[p].owner == player.whoAmI && Main.projectile[p].minion)
                {
                    Projectile proj = Main.projectile[p];
                    byte SpawnCount = NumberOfFlames;
                    Vector2 SpawnPosition = proj.Center;
                    SpawnPosition.Y -= proj.height;
                    Projectile.NewProjectile(SpawnPosition, Vector2.Zero, 659, Damage, 0.6f, player.whoAmI);
                    SpawnCount--;
                    if (SpawnCount > 0)
                    {
                        SpawnPosition = proj.Center;
                        SpawnPosition.Y -= proj.height * 0.8f;
                        SpawnPosition.X -= proj.width * 0.5f;
                        Projectile.NewProjectile(SpawnPosition, Vector2.Zero, 659, Damage, 0.6f, player.whoAmI);
                        SpawnCount--;
                        if (SpawnCount > 0)
                        {
                            SpawnPosition = proj.Center;
                            SpawnPosition.Y -= proj.height * 0.8f;
                            SpawnPosition.X += proj.width * 0.5f;
                            Projectile.NewProjectile(SpawnPosition, Vector2.Zero, 659, Damage, 0.6f, player.whoAmI);
                            SpawnCount--;
                        }
                    }
                }
            }
            data.EndUse();
        }
    }
}