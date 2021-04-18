using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace TerraClasses.SkillList.Cleric
{
    public class BaneUndead : SkillBase
    {
        public BaneUndead()
        {
            Name = "Bane Undead";
            Description = "Increases damage inflicted on Undeads by 5% + 2% per level of damage.\n" +
                " Past level 5, Undeads will inflict 10% less damage on you.";
            MaxLevel = 10;
        }

        public override void ModifyHitByNPC(Player player, SkillData data, NPC npc, ref int damage, ref bool crit)
        {
            if(data.Level > 5 && MainMod.IsUndead(npc.type))
            {
                damage -= (int)(damage * 0.1f);
            }
        }

        public override void ModifyHitNPC(Player player, SkillData data, Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            if (MainMod.IsUndead(target.type))
            {
                damage += (int)(damage * (0.05f + 0.02f * data.Level));
                for(int i = 0; i < 3; i++)
                {
                    Dust.NewDust(target.position, target.width, target.height, 57);
                }
            }
        }

        public override void ModifyHitNPCWithProj(Player player, SkillData data, Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (MainMod.IsUndead(target.type))
            {
                damage += (int)(damage * (0.05f + 0.02f * data.Level));
                for (int i = 0; i < 3; i++)
                {
                    Dust.NewDust(target.position, target.width, target.height, 57);
                }
            }
        }
    }
}
