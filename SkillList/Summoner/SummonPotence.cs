using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.SkillList.Summoner
{
    public class SummonPotence : SkillBase
    {
        public SummonPotence()
        {
            Name = "Summon Potence";
            Description = "Allows companions to inflict critical hit.\nEvery level increase Critical Rate by 1%.\nCritical Rate increases by 5% after level 5.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Passive;
        }

        public override void ModifyHitNPCWithProj(Terraria.Player player, SkillData data, Terraria.Projectile proj, Terraria.NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (proj.minion || proj.type == 376 || proj.type == 378 || proj.type == 379 || proj.type == 389 || proj.type == 408 || proj.type == 614)
            {
                int CriticalRate = data.Level;
                if (data.Level > 5)
                    CriticalRate += 5;
                if (Terraria.Main.rand.Next(100) < CriticalRate)
                {
                    damage *= 2;
                    crit = true;
                }
            }
        }
    }
}
