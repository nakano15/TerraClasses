using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace TerraClasses.SkillList.Vilemancer
{
    public class CorruptQuills : SkillBase
    {
        public CorruptQuills()
        {
            Name = "Corrupt Quills";
            Description = "Launches several quills at foes at the aimed position.\n" +
                " Quills causes 34% + 2.3% per level of magic damage. Hits ignores defense.\n" +
                " Spawns 9 + Level * 1.5 quills." +
                " The quills can affect up to 5 targets, but will hit enemies more times if there's less targets in the area.";
            skillType = Enum.SkillTypes.Active;
            PositionToTake = PositionToTakeOnCastEnum.Mouse;
        }

        public override SkillData GetSkillData => new CorruptQuillsData();

        public override float GetEffectRange(SkillData sd)
        {
            return 94f;
        }

        public override void Update(Player player, SkillData rawdata)
        {
            CorruptQuillsData data = (CorruptQuillsData)rawdata;
            int QuillCount = 9 + (int)(data.Level * 1.5f);
            if (data.Time == 0)
            {
                data.Targets = data.GetPossibleTargets(false, false, data.CastPosition, GetEffectRange(data), 5);
                data.SpawnDirection = (sbyte)(data.CastPosition.X < player.Center.X ? 1 : -1);
                if(data.Targets.Length == 0)
                {
                    Main.NewText("Skill failed", Color.Red);
                    data.EndUse(true);
                    return;
                }
            }
            if(data.Time >= 70)
            {
                int QuillNumber = (data.Time - 70) / 10;
                if (QuillNumber >= QuillCount)
                    data.EndUse();
                else
                {
                    int Target = QuillNumber % data.Targets.Length;
                    data.HurtTarget(data.Targets[Target], data.GetMagicDamage(0, 0.34f + 0.023f * data.Level, player), 0, 0.1f, 0, false, false);
                }
            }
        }

        public override void Draw(Player player, SkillData rawdata, PlayerDrawInfo pdi)
        {
            CorruptQuillsData data = (CorruptQuillsData)rawdata;
            int QuillCount = 9 + (int)(data.Level * 1.5f);
            Texture2D QuillTexture = MainMod.CorruptQuills.GetTexture;
            for(int q = 0; q < QuillCount; q++)
            {
                int QuillTime = data.Time - q * 10;
                if (QuillTime < 0 || QuillTime >= 70)
                    continue;
                Vector2 QuillPosition = player.Center;
                {
                    float HoverTime = QuillTime / 40;
                    if (HoverTime > 1)
                        HoverTime = 1;
                    QuillPosition.X += data.SpawnDirection * 40;
                    QuillPosition.Y += (-3 + q % 7) * 12;
                }
                TargetTranslator.Translator Target = data.Targets[q % data.Targets.Length];
                float TargetDirection = (float)Math.Atan2(QuillPosition.Y - Target.Center.Y, QuillPosition.X - Target.Center.X);
                if(QuillTime >= 60)
                {
                    QuillPosition += (Target.Center - QuillPosition) * ((QuillTime - 60) * 0.1f);
                }
                Terraria.DataStructures.DrawData dd = new Terraria.DataStructures.DrawData(QuillTexture, QuillPosition, null, Color.White, TargetDirection, new Vector2(3, 8), 1f, SpriteEffects.None, 0 );
                Main.playerDrawData.Insert(0, dd);
            }
        }

        public class CorruptQuillsData: SkillData
        {
            public TargetTranslator.Translator[] Targets = new TargetTranslator.Translator[0];
            public sbyte SpawnDirection = 0;
        }
    }
}
