using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace TerraClasses.SkillList.Berseker
{
    class Bully : SkillBase
    {
        public Bully()
        {
            Name = "Bully";
            Description = "Deal more damage on small targets.\n" +
                " Damage inflicted increases by 12% per level.\n" +
                " At level 5 and above, half of the damage bonus affects medium monsters.\n" +
                " At level 10 and above, a fourth of the damage bonus affects large monsters.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Passive;
        }

        public override void ModifyHitNPC(Player player, SkillData data, Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            ApplyDamageBonus(data, target, ref damage);
        }

        public override void ModifyHitNPCWithProj(Player player, SkillData data, Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            ApplyDamageBonus(data, target, ref damage);
        }

        private void ApplyDamageBonus(SkillData data, NPC target, ref int damage)
        {
            float DamageBonus = 0.12f * data.Level;
            float TargetDimension = target.width * target.height;
            byte MonsterSize = 0;
            if (TargetDimension >= 54 * 54)
            {
                MonsterSize = 2;
            }
            else if (TargetDimension >= 40 * 50)
            {
                MonsterSize = 1;
            }
            switch (MonsterSize)
            {
                case 0:
                    damage += (int)(damage * DamageBonus);
                    break;
                case 1:
                    if (data.Level >= 5)
                        damage += (int)(damage * DamageBonus * 0.5f);
                    break;
                case 2:
                    if (data.Level >= 10)
                        damage += (int)(damage * DamageBonus * 0.25f);
                    break;
            }
        }
    }
}
