using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;

namespace TerraClasses.SkillList.Summoner
{
    public class PiercingAttacks : SkillBase
    {
        public PiercingAttacks()
        {
            Name = "Piercing Attacks";
            Description = "Companion attacks returns 1% of enemy defense as damage per level.\nDefense ignored increases by 5% at max level.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Passive;
        }

        public override void ModifyHitNPCWithProj(Player player, SkillData data, Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if(proj.minion || proj.type == ProjectileID.ImpFireball || proj.type == ProjectileID.HornetStinger)
            {
                float SkillDamage = 0.01f * data.Level;
                if (data.Level >= 10)
                    SkillDamage += 0.05f;
                damage += (int)(target.defense * SkillDamage);
            }
        }
    }
}
