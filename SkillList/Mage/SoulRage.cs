using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace TerraClasses.SkillList.Mage
{
    public class SoulRage : SkillBase
    {
        public SoulRage()
        {
            Name = "Soul Rage";
            Description = "Launches vengeful spirits at the nearby foes.\nNumber of souls spawned is 1 + Level / 2.\nSouls causes 120% + 22% damage per level.";
            MaxLevel = 20;
            Cooldown = GetCooldown(12);
            skillType = Enum.SkillTypes.Active;
        }

        public override void Update(Player player, SkillData data)
        {
            const int ShotDelay = 15;
            const float ShotSpeed = 2;
            int ShotCount = 1 + data.Level / 2;
            if (data.Time >= ShotDelay)
            {
                Vector2 SpawnPosition = player.Center;
                Vector2 ShotDirection = new Vector2(-100 * player.direction, (data.Step - (ShotCount * 0.5f)) * 20);
                ShotDirection.Normalize();
                Projectile proj = Projectile.NewProjectileDirect(SpawnPosition, ShotDirection * ShotSpeed, 297, data.GetMagicDamage(0, 1.2f + 0.22f * data.Level, player), 4f, player.whoAmI);
                proj.tileCollide = false;
                data.ChangeStep();
            }
            if (data.Step >= ShotCount)
                data.EndUse();
        }
    }
}
