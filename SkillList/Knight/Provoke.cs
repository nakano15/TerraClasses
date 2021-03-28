using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TerraClasses.SkillList.Knight
{
    public class Provoke : SkillBase
    {
        public Provoke()
        {
            Name = "Provoke";
            Description = "Increases your aggro for a number of time.\n" +
                " Increases your Aggro rate by 40 per level.\n" +
                " Provoke lasts 30 seconds, +30 every 5 levels.\n" +
                " Past level 5, you gain 5% extra defense.";
            skillType = Enum.SkillTypes.Active;
            Cooldown = GetTime(0, 1);
        }

        public override void Update(Player player, SkillData data)
        {
            if (data.Time == 0)
            {
                data.ApplySkillBuff(player, ModContent.BuffType<Buffs.Provoke>(), (30 * (1 + data.Level / 5)) * 60);
                player.chatOverhead.NewMessage("Who will challenge me?!", Main.chatLength / 2);
            }
            if (data.Time >= 90)
            {
                data.EndUse(false);
                return;
            }
            float Scale = (float)data.Time / 60, Opacity = 1f;
            if (data.Time > 60)
            {
                Opacity = 1f - (float)(data.Time - 60) / 30;
            }
            Vector2 Position = player.Center - Main.screenPosition;
            Position.Y -= player.height * 0.25f;
            Main.spriteBatch.Draw(MainMod.ProvokeAudioEffect.GetTexture, Position, null, Color.White * Opacity, 0f, new Vector2(32, 32) * 0.5f, Scale, SpriteEffects.None, 0f);
        }
    }
}
