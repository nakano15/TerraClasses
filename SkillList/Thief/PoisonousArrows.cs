using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;

namespace TerraClasses.SkillList.Thief
{
    public class PoisonousArrows : SkillBase
    {
        public PoisonousArrows()
        {
            Name = "Poisonous Arrows";
            Description = "Arrows you shoot while skill is active will have a chance of poisoning the target.\n" +
                " Poisoning chance: 30%.\n" +
                "  Chance increases to 45% past level 8." +
                " Lasts 20 + 2 per level seconds.\n" +
                " Past level 5, hits from arrows have an extra 3% chance of critical.\n" +
                "  15% on poisoned targets.\n" +
                " Poison effect lasts 12 seconds.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Active;
        }

        public override void Update(Player player, SkillData data)
        {
            if (data.Time >= (20 + 2 * data.Level) * 60)
                data.EndUse();
        }

        public override void ModifyHitNPCWithProj(Player player, SkillData data, Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            switch (proj.type)
            {
                case ProjectileID.BeeArrow:
                case ProjectileID.BoneArrow:
                case ProjectileID.ChlorophyteArrow:
                case ProjectileID.CursedArrow:
                case ProjectileID.DD2BetsyArrow:
                case ProjectileID.FireArrow:
                case ProjectileID.FlamingArrow:
                case ProjectileID.FrostArrow:
                case ProjectileID.FrostburnArrow:
                case ProjectileID.HellfireArrow:
                case ProjectileID.HolyArrow:
                case ProjectileID.IchorArrow:
                case ProjectileID.JestersArrow:
                case ProjectileID.MoonlordArrow:
                case ProjectileID.PhantasmArrow:
                case ProjectileID.ShadowFlameArrow:
                case ProjectileID.UnholyArrow:
                case ProjectileID.VenomArrow:
                case ProjectileID.WoodenArrowFriendly:
                    if (data.Level >= 8)
                    {
                        if (Main.rand.NextDouble() <= 0.45)
                        {
                            target.AddBuff(Terraria.ID.BuffID.Poisoned, 12 * 60);
                        }
                    }
                    else
                    {
                        if (Main.rand.NextDouble() <= 0.3)
                        {
                            target.AddBuff(Terraria.ID.BuffID.Poisoned, 12 * 60);
                        }
                    }
                    if (!crit && data.Level > 5)
                    {
                        crit = Main.rand.NextDouble() <= (target.HasBuff(BuffID.Poisoned) ? 0.15 : 0.03);
                    }
                    break;
            }
        }
    }
}
