using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace TerraClasses.SkillList.Knight
{
    public class ShieldBash : SkillBase
    {
        private const byte ShieldBashDirectionVar = 0;

        public ShieldBash()
        {
            Name = "Shield Bash";
            Description = "Hits targets in front of you with your shield, knocking them backwards.\n" +
                " Inflicts 120% + 2.6% per level of Melee Damage.\n" +
                " High Knockback rate.\n";
            MaxLevel = 10;
            Cooldown = GetTime(15);
            CastTime = 8;
            skillType = Enum.SkillTypes.Active;
        }

        public override void Update(Player player, SkillData data)
        {
            if (data.Time == 0)
            {
                data.SetInteger(ShieldBashDirectionVar, player.direction > 0 ? 1 : -1);
            }
            if (data.Time == 2)
            {
                Rectangle rect = new Rectangle((int)player.Center.X, (int)player.Center.Y, 60 + 20, 44 + 20);
                rect.Y -= rect.Height / 2;
                if (data.GetInteger(ShieldBashDirectionVar) < 0)
                    rect.X -= rect.Width;
                TargetTranslator.Translator[] Targets = data.GetPossibleTargets(false, rect);
                int Damage = data.GetMeleeDamage(0, 1.2f + 0.026f * data.Level, player);
                foreach (TargetTranslator.Translator Target in Targets)
                {
                    Target.Hurt(Damage, 15, false, true, " was launched way too far.");
                }
            }
            if (data.Time >= 20)
                data.EndUse();
        }

        public override void Draw(Player player, SkillData data, PlayerDrawInfo pdi)
        {
            Vector2 EffectPosition = player.Center - Main.screenPosition;
            Color color = Color.White;
            EffectPosition.X += data.GetInteger(ShieldBashDirectionVar) * 3.6f * data.Time;
            if (data.Time > 10)
            {
                color *= 1f - ((data.Time - 10) * 0.1f);
            }
            Terraria.DataStructures.DrawData dd = new Terraria.DataStructures.DrawData(MainMod.ShieldBashEffect.GetTexture, EffectPosition, null, color,
                0f, new Vector2(24, 44) * 0.5f, 1f, (data.GetInteger(ShieldBashDirectionVar) > 0 ? Microsoft.Xna.Framework.Graphics.SpriteEffects.None : Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally), 0);
            Main.playerDrawData.Add(dd);
        }
    }
}
