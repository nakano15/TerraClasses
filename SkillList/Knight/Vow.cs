using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using TerraClasses.TargetTranslator;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerraClasses.SkillList.Knight
{
    public class Vow : SkillBase
    {
        public Vow()
        {
            Name = "Vow";
            Description = "Makes a vow to protect an ally.\n" +
                " While having that ally alive, your maximum health is enhanced by 1.5% per level.\n" +
                " When the ally you vowed to protect is in danger, your melee damage increases by 15% + 2% per level.\n" +
                " You gain Weak, Slow and Broken Armor debuffs when the one you're protecting dies.\n" +
                " Staying close to the one you vowed for increases your health regen rate.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Active;
            Cooldown = GetTime(0, 1);
        }

        public override SkillData GetSkillData => new VowSkillData();

        public override float GetEffectRange(SkillData sd)
        {
            return 48f;
        }

        public override void UpdateStatus(Player player, SkillData rawdata)
        {
            VowSkillData data = (VowSkillData)rawdata;
            if(data.Target.Health < data.Target.MaxHealth * 0.9f)
            {
                player.meleeDamage += 0.15f + 0.02f * data.Level;
            }
            player.statLifeMax2 += (int)(player.statLifeMax2 * 0.015f *data.Level);
            if ((player.Center - data.Target.Center).Length() < 250f)
                player.lifeRegen += 2;
        }

        public override void Update(Player player, SkillData rawdata)
        {
            VowSkillData data = (VowSkillData)rawdata;
            if(data.Time == 0)
            {
                float Range = GetEffectRange(data);
                Translator[] Targets = data.GetPossibleTargets(true, false, data.CastPosition, Range);
                Translator NearestTarget = null;
                float NearestDistance = Range;
                foreach(Translator Target in Targets)
                {
                    float Distance = (Target.Center - data.CastPosition).Length();
                    if(Distance < NearestDistance)
                    {
                        NearestTarget = Target;
                        NearestDistance = Distance;
                    }
                }
                if(NearestTarget == null)
                {
                    CombatText.NewText(player.getRect(), Microsoft.Xna.Framework.Color.Red, "There's no one to vow to.");
                    data.EndUse(true);
                    return;
                }
                data.Target = NearestTarget;
            }
            if(data.Target.IsDead())
            {
                data.EndUse(false);
                if(player.whoAmI == Main.myPlayer)
                    Main.NewText("You failed to protect the one you vowed for.", Microsoft.Xna.Framework.Color.Red);
                player.AddBuff(Terraria.ID.BuffID.Weak, 30 * 3600);
                player.AddBuff(Terraria.ID.BuffID.BrokenArmor, 30 * 3600);
                player.AddBuff(Terraria.ID.BuffID.Slow, 30 * 3600);
                return;
            }
            if (!data.Target.IsActive())
            {
                if (player.whoAmI == Main.myPlayer)
                    Main.NewText("The one you vowed for left.", Microsoft.Xna.Framework.Color.Red);
                data.EndUse(false);
            }
        }

        public override void Draw(Player player, SkillData rawdata, PlayerDrawInfo pdi)
        {
            VowSkillData data = (VowSkillData)rawdata;
            Vector2 DrawPosition = data.Target.Center - Main.screenPosition;
            DrawPosition.Y -= data.Target.Height * 0.5f + 20f;
            Terraria.DataStructures.DrawData dd = new Terraria.DataStructures.DrawData(Main.heartTexture, DrawPosition, null,
                Color.White, 0f, new Vector2(Main.heartTexture.Width, Main.heartTexture.Height) * 0.5f, 0.5f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
            Main.playerDrawData.Insert(0, dd);
        }

        public class VowSkillData : SkillData
        {
            public Translator Target;
        }
    }
}
