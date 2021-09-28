using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace TerraClasses.SkillList.Arachnomancer
{
    public class VenomousAmmo : SkillBase
    {
        public VenomousAmmo()
        {
            Name = "Venomous Attack";
            Description = "Imbues your ammo and weapons with venomous properties.\n" +
                " Imbuing effects lasts for 15 + 3 per level seconds.\n" +
                " After level 5, increases the chance of inflicting extra damage to Venom inflicted foes.";
            skillType = Enum.SkillTypes.Active;
            Cooldown = GetTime(60);
            MaxLevel = 10;
        }

        public override void Update(Player player, SkillData data)
        {
            float EffectDistancing = (float)Math.Cos(data.Time) * 25;
            if (Main.rand.Next(3) == 0)
            {
                for (int x = -1; x <= 1; x += 2)
                {
                    Vector2 DustPosition = player.Center;
                    DustPosition.X += x * EffectDistancing;
                    Dust.NewDust(DustPosition, 4, 4, 171);
                }
            }
            if (data.Time >= (15 + 3 * data.Level) * 60)
                data.EndUse(false);
        }

        public override void ModifyHitNPC(Player player, SkillData data, Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            if (data.Level > 5 && target.venom && Main.rand.NextFloat() * 10 < (data.Level - 5) * 2)
            {
                data.HurtNpc(target, (int)(damage * 0.08f), player.direction, 0, 0, CountDefense: false);
            }
        }

        public override void ModifyHitNPCWithProj(Player player, SkillData data, Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (data.Level > 5 && target.venom && Main.rand.NextFloat() * 10 < (data.Level - 5) * 2)
            {
                data.HurtNpc(target, (int)(damage * 0.08f), player.direction, 0, 0, CountDefense: false);
            }
        }

        public override void OnHitNPCWithProj(Player player, SkillData data, Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(Terraria.ID.BuffID.Venom, (crit ? 12 : 5) * 60);
        }

        public override void OnHitNPC(Player player, SkillData data, Item item, NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(Terraria.ID.BuffID.Venom, (crit ? 12 : 5) * 60);
        }
    }
}
