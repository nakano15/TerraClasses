using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerraClasses.TargetTranslator;
using Terraria.ModLoader;

namespace TerraClasses.SkillList.Mage
{
    public class ChainLightning : SkillBase
    {
        public ChainLightning()
        {
            Name = "Chain Lightning";
            Description = "Releases lightning bolts that target one enemy after another dealing magic damage.\n" +
                " Affects enemies at mouse position.\n" +
                " Electricity hits targets 10 + Level / 2 times.\n" +
                " Inflicts 80% + 2.5% per level Magic Damage per hit.\n" +
                " Chain Lightning damage drops as It hits more targets, but gains a 10% damage bonus per extra target to compensate."; //(At max rank can chain up to 8 enemies)
            MaxLevel = 10;
            Cooldown = GetTime(40);
            CastTime = GetTime(1) + 20;
            skillType = Enum.SkillTypes.Active;
            UnallowOtherSkillUsage = true;
        }

        public override float GetEffectRange(SkillData sd)
        {
            return 80f + 15f * sd.Level;
        }

        public const float DamageDelay = 10;

        public override void Update(Player player, SkillData rawdata)
        {
            ChainLightningSkillData data = (ChainLightningSkillData)rawdata;
            if(data.Time == 0 && data.Step == 0)
            {
                Vector2 MousePosition = data.CastPosition;
                int MaxTargets = 3 + data.Level / 2;
                Translator[] Targets = data.GetPossibleTargets(false);
                float LightningDistance = GetEffectRange(data);
                List<Translator> AffectedOnes = Targets.ToList();
                if(AffectedOnes.Count == 0)
                {
                    data.EndUse(true);
                    CombatText.NewText(player.getRect(), Color.Red, "No target nearby!", true);
                    return;
                }
                data.Targets = AffectedOnes;
            }
            if(data.Time >= DamageDelay)
            {
                bool SomeoneAlive = false;
                float DamagePercentage = 0.8f + 0.025f * data.Level;
                if (data.Targets.Count > 1)
                {
                    DamagePercentage = DamagePercentage / data.Targets.Count + ((data.Targets.Count - 1) * 0.1f * DamagePercentage);
                }
                int DamageToInflict = data.GetMagicDamage(0, DamagePercentage, player);
                foreach(Translator target in data.Targets)
                {
                    if (target.IsActive() && !target.IsDead())
                    {
                        SomeoneAlive = true;
                        if (player.whoAmI == Main.myPlayer)
                            target.Hurt(DamageToInflict, 0.1f, false, true, (Main.rand.Next(2) == 0 ? " got shocked!" : " danced to death!"));
                    }
                }
                if (!SomeoneAlive)
                {
                    data.EndUse();
                    return;
                }
                data.ChangeStep();
            }
            if (data.Step >= 10 + data.Level / 2)
                data.EndUse();
        }

        public override void Draw(Player player, SkillData rawdata, PlayerDrawInfo pdi)
        {
            ChainLightningSkillData data = (ChainLightningSkillData)rawdata;
            bool First = true;
            float Percentage = data.Time / DamageDelay;
            for(int t = 0; t < data.Targets.Count; t++)
            {
                Translator Target = data.Targets[t];
                if (!Target.IsActive() || Target.IsDead())
                    continue;
                if (First)
                {
                    DrawCurrentChain(player.Center, Target.Center, Percentage);
                    First = false;
                }
                for (int t2 = 0; t2 < data.Targets.Count; t2++)
                {
                    if (t == t2)
                        continue;
                    Translator SubTarget = data.Targets[t2];
                    if (!SubTarget.IsActive() || SubTarget.IsDead())
                        continue;
                    DrawCurrentChain(Target.Center, SubTarget.Center, Percentage);
                }
            }
        }

        public void DrawCurrentChain(Vector2 Start, Vector2 End, float Percentage)
        {
            Vector2 ChainPosition = Start + (End - Start) * Percentage - Main.screenPosition;
            Vector2 ChainOrientation = End - Start;
            float Rotation =(float)Math.Atan2(ChainOrientation.Y, ChainOrientation.X) + 1.570796f; //1.570796‬f
            Main.spriteBatch.Draw(MainMod.Electricity.GetTexture, ChainPosition, null, Color.White, Rotation, new Vector2(6, 10),1f, SpriteEffects.None, 0f);
        }

        public override SkillData GetSkillData => new ChainLightningSkillData();

        public class ChainLightningSkillData : SkillData
        {
            public List<TargetTranslator.Translator> Targets = new List<TargetTranslator.Translator>();
        }
    }
}
