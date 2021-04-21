using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace TerraClasses.SkillList.Cleric
{
    public class Heal : SkillBase
    {
        public Heal()
        {
            Name = "Heal";
            Description = "Restores the health of all allies around mouse position.\n" +
                " Health restored is equal to 15% of your max health, plus\n" +
                " 40% + 8% per level of magic damage.\n" +
                " Healing range increases with the level.";
            skillType = Enum.SkillTypes.Active;
            MaxLevel = 10;
            Cooldown = GetTime(45);
            CastTime = 20;
        }

        public override SkillData GetSkillData => new HealSkillData();

        public override float GetEffectRange(SkillData sd)
        {
            return 120 + 8 * sd.Level;
        }

        public override void Update(Player player, SkillData rawdata)
        {
            HealSkillData data = (HealSkillData)rawdata;
            if (data.Time == 0)
            {
                int HealingPower = data.GetMagicDamage(0, 0.4f + 0.08f * data.Level, player) + (int)(player.statLifeMax2 * 0.15f);
                float MaxDistance = GetEffectRange(data);
                data.Targets = data.GetPossibleTargets(true, true, data.CastPosition, MaxDistance);
                foreach (TargetTranslator.Translator t in data.Targets)
                {
                    t.ChangeHealth(HealingPower);
                }
            }
            float HealEffectX = (float)Math.Sin(data.Time * 3);
            float EffectPositionY = (float)data.Time / 90;
            if (data.Time % 3 == 0)
            {
                for (int x = -1; x < 2; x += 2)
                {
                    foreach (TargetTranslator.Translator target in data.Targets)
                    {
                        Vector2 EffectPos = new Vector2(target.Center.X + HealEffectX * x * target.Width * 0.8f, target.Position.Y + target.Height * (1f - EffectPositionY));
                        Dust d = Dust.NewDustPerfect(EffectPos, Terraria.ID.DustID.HealingPlus, Vector2.UnitY * -0.01f);
                        d.noGravity = false;
                        d.noLight = false;
                        d.scale = Main.rand.Next(13, 18) * 0.1f;
                    }
                }
            }
            if (data.Time >= 90)
                data.EndUse();
        }

        public class HealSkillData : SkillData
        {
            public TargetTranslator.Translator[] Targets = new TargetTranslator.Translator[0];
        }
    }
}
