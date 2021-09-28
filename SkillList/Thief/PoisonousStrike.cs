using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace TerraClasses.SkillList.Thief
{
    public class PoisonousStrike : SkillBase
    {
        public PoisonousStrike()
        {
            Name = "Poisonous Strike";
            Description = "Your melee attacks inflicts 5% per level extra damage.\n" +
                " Lasts 15 + 3 per level seconds.\n" +
                " Each hit has a chance of inflicting Poisoning to the target.\n" +
                "  Poisoning rate increases with level.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Active;
        }

        public override void Update(Player player, SkillData data)
        {
            if (data.Time >= (15 + 3 * data.Level) * 60)
                data.EndUse(false);
        }

        public override void OnHitNPC(Player player, SkillData data, Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (item.melee)
            {
                data.HurtNpc(target, (int)(damage * 0.05f), player.direction, 0f, 5, CountDefense: false);
                if (Main.rand.NextDouble() < 0.02f * data.Level)
                    target.AddBuff(Terraria.ID.BuffID.Poisoned, 8 * 60);
            }
        }

        public override void OnHitNPCWithProj(Player player, SkillData data, Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (proj.melee)
            {
                data.HurtNpc(target, (int)(damage * 0.05f), player.direction, 0f, 5, CountDefense:false);
                if (Main.rand.NextDouble() < 0.02f * data.Level)
                    target.AddBuff(Terraria.ID.BuffID.Poisoned, 8 * 60);
            }
        }
    }
}
