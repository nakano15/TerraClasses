using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace TerraClasses.SkillList.Sharpshooter
{
    public class Marksmanship : SkillBase
    {
        public Marksmanship()
        {
            Name = "Marksmanship";
            Description = "Increases the ranged damage received by monsters 7 tiles far away from you.\n" +
                " Increases the extra damage by 2% per skill level.\n" +
                " Past level 5, increases the critical chance by 10%.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Passive;
        }

        public override void ModifyHitNPCWithProj(Player player, SkillData data, Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (proj.ranged)
            {
                float Distance = 7 * 16 - (player.Center - target.Center).Length();
                if(Distance > 0)
                {
                    Distance *= 1f / 16;
                    damage += (int)(Distance * damage * (data.Level * 0.02f));
                }
                if (!crit && data.Level > 5 && Main.rand.NextDouble() < 0.1)
                    crit = true;
            }
        }
    }
}
