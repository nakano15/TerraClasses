using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace TerraClasses.SkillList.Fighter
{
    public class Endure : SkillBase
    {
        public Endure()
        {
            Name = "Endure";
            Description = "Rises defense by 15, and gives Knockback Immunity.\n" +
                " Past level 5, starts giving 3% extra defense.\n" +
                " Lasts for 45 seconds.";
            Cooldown = 35 * 60;
            skillType = Enum.SkillTypes.Active;
            MaxLevel = 10;
        }

        public override void Update(Player player, SkillData data)
        {
            if (data.Time >= 45 * 60)
                data.EndUse();
            if (data.Time % 15 == 0)
            {
                Vector2 EffectOrientation = Vector2.Zero;
                int Cycle = (data.Time / 15) % 4;
                switch (Cycle)
                {
                    case 0:
                        EffectOrientation.X = -1f;
                        break;
                    case 1:
                        EffectOrientation.X = -0.85f;
                        EffectOrientation.Y = -0.85f;
                        break;
                    case 2:
                        EffectOrientation.Y = -1f;
                        break;
                    case 3:
                        EffectOrientation.X = 0.85f;
                        EffectOrientation.Y = -0.85f;
                        break;
                }
                for (int dir = -1; dir <= 1; dir += 2)
                {
                    Vector2 EffectPosition = player.Center + EffectOrientation * dir * 32f;
                    int d = Dust.NewDust(EffectPosition, 1, 1, Terraria.ID.DustID.Grass);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity = Vector2.Zero;
                    Main.dust[d].scale *= 2;
                }
            }
        }

        public override void UpdateStatus(Player player, SkillData data)
        {
            int DefBonus = 15;
            if (data.Level > 5)
                DefBonus += (int)(player.statDefense * 0.03f * (data.Level - 5));
            player.statDefense += DefBonus;
            player.noKnockback = true;
        }
    }
}
