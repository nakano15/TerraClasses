using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace TerraClasses.SkillList.Arachnomancer
{
    public class SpiderSwarm : SkillBase
    {
        public SpiderSwarm()
        {
            Name = "Spider Swarm";
            Description = "Drop spider eggs as you move.\n" +
                " Spiders spawned from the eggs causes 40% + 3% per level of summon damage.\n" +
                " Using this skill before level 5 may cause your character to feel pain.\n" +
                " Lays 5 + Level / 2 eggs.\n" +
                " Time before laying another egg is of 1.5 seconds - 0.1 seconds per level.";
            MaxLevel = 10;
            Cooldown = GetTime(35);
            skillType = Enum.SkillTypes.Active;
        }

        public override void Update(Player player, SkillData data) //Is producing insane damage instead of the intended.
        {
            int EggLayingTime = 90 - data.Level * 6;
            if (EggLayingTime < 20)
                EggLayingTime = 20;
            if (data.Time >= EggLayingTime)
            {
                int Damage = data.GetSummonDamage(0, 0.4f + 0.03f * data.Level, player);
                Projectile.NewProjectile(player.Center, Microsoft.Xna.Framework.Vector2.UnitX * -player.direction * 0.05f, Terraria.ID.ProjectileID.SpiderEgg, Damage, 0.6f, player.whoAmI);
                data.ChangeStep();
                if(data.Level < 5 && Main.rand.NextFloat() * 10 < (5 - data.Level) * 2)
                {
                    player.Hurt(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(" was unnable to endure..."), Main.rand.Next(2, 5), 0, false, false, false);
                }
                if (data.Step >= data.Level / 2 + 5)
                    data.EndUse();
            }
        }
    }
}
