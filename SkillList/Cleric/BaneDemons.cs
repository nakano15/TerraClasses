using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace TerraClasses.SkillList.Cleric
{
    public class BaneDemons : SkillBase
    {
        public BaneDemons()
        {
            Name = "Bane Demons";
            Description = "Increases damage inflicted on Demons by 5% + 2% per level of damage.\n" +
                " Past level 5, Demons will inflict 10% less damage on you.";
            MaxLevel = 10;
        }

        public override void ModifyHitByNPC(Player player, SkillData data, NPC npc, ref int damage, ref bool crit)
        {
            if (data.Level > 5 && MainMod.IsDemon(npc.type))
            {
                damage -= (int)(damage * 0.1f);
            }
        }

        public override void ModifyHitNPC(Player player, SkillData data, Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            if (MainMod.IsDemon(target.type))
            {
                damage += (int)(damage * (0.05f + 0.02f * data.Level));
                for (int i = 0; i < 3; i++)
                {
                    Dust.NewDust(target.position, target.width, target.height, 270);
                }
            }
        }

        public override void ModifyHitNPCWithProj(Player player, SkillData data, Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (MainMod.IsDemon(target.type))
            {
                damage += (int)(damage * (0.05f + 0.02f * data.Level));
                for (int i = 0; i < 3; i++)
                {
                    Dust.NewDust(target.position, target.width, target.height, 270);
                }
            }
        }
    }
}
