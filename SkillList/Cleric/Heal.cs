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
            Cooldown = GetCooldown(45);
            CastTime = 20;
        }

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
                    t.Health += HealingPower;
                    if (t.Health > t.MaxHealth)
                        t.Health = t.MaxHealth;
                    CombatText.NewText(t.GetRectangle, CombatText.HealLife, HealingPower, false, true);
                }
                /*for (int p = 0; p < 255; p++)
                {
                    if (p == player.whoAmI || (Main.player[p].active && !Main.player[p].dead && player.hostile == Main.player[p].hostile && player.team == Main.player[p].team && Main.player[p].Distance(data.CastPosition) < MaxDistance))
                    {
                        data.ApplyPlayerInteraction(Main.player[p]);
                        player.statLife += HealingPower;
                        player.HealEffect(HealingPower);
                        data.ApplyPlayerInteraction(Main.player[p]);
                    }
                }*/
                //data.EndUse();
            }
            float HealEffectX = (float)Math.Sin(data.Time * 3);
            float EffectPositionY = (float)data.Time / 90;
            for (int x = -1; x < 2; x += 2)
            {
                foreach (TargetTranslator.Translator target in data.Targets)
                {
                    Vector2 EffectPos = new Vector2(target.Center.X + HealEffectX * x * target.Width * 0.6f, target.Position.Y + target.Height * (1f - EffectPositionY));
                    Dust d = Dust.NewDustPerfect(EffectPos - Main.screenPosition, Terraria.ID.DustID.HealingPlus);
                    d.noGravity = false;
                    d.noLight = false;
                    d.scale = Main.rand.Next(13, 18) * 0.1f;
                }
                /*foreach (Player p in data.GetPlayersInteractedWith())
                {
                    Vector2 EffectPos = new Vector2(player.Center.X + HealEffectX * x, player.position.Y + EffectPositionY);
                    Dust d = Dust.NewDustPerfect(EffectPos - Main.screenPosition, Terraria.ID.DustID.GreenBlood);
                    d.noGravity = false;
                    d.noLight = false;
                    d.scale = Main.rand.Next(13, 18) * 0.1f;
                }*/
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
