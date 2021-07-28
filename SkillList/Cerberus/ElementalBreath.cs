using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace TerraClasses.SkillList.Cerberus
{
    public class ElementalBreath : SkillBase
    {
        public const int FireDelayMax = 60;

        public ElementalBreath()
        {
            Name = "Elemental Breath";
            Description = "Breathes elemental attacks from the mouth every 1 second.\n" +
                " Alternates between mouths while spitting, launching different elements.\n" +
                " Inflicts 80% + 4% per level damage.\n" +
                " Each mouth damage is related to a attack type.\n" +
                " 1/4 chance of spitting based on Summon damage, instead of Melee, ranged or magic damage.\n" +
                " After 4 shots, It enters a overdrive mode for 8 shots, and then resets.\n" +
                " Fire delay is lower when with lower than half health.";
            skillType = Enum.SkillTypes.Attack;
            MaxLevel = 10;
        }

        public override SkillData GetSkillData => new ElementalBreathData();

        public override void Update(Player player, SkillData rawdata)
        {
            ElementalBreathData data = (ElementalBreathData)rawdata;
            if (data.FireDelay > 0)
            {
                data.FireDelay--;
            }
        }

        public override void UpdateItemUse(Player player, SkillData rawdata, bool JustUsed)
        {
            if (player.itemAnimation == 0)
                return;
            ElementalBreathData data = (ElementalBreathData)rawdata;
            if (data.FireDelay == 0)
            {
                CerberusFormData sd = (CerberusFormData)PlayerMod.GetPlayerSkillData(player, 22);
                if (sd != null)
                {

                    sd.HeadFrame[data.AttackTurn] = 30;
                }
                Vector2 ShotSpawnPosition = player.Center;
                ShotSpawnPosition.Y -= 8 * player.gravDir;
                ShotSpawnPosition.X += player.direction * 12;
                int ShotType = 0;
                int Damage = 0;
                float DamageMod = 0.8f + 0.04f * data.Level;
                switch (data.AttackTurn)
                {
                    case 0:
                        ShotType = 34;
                        Damage = data.GetMeleeDamage(0, DamageMod, player);
                        break;
                    case 1:
                        ShotType = 27;
                        Damage = data.GetRangedDamage(0, DamageMod, player);
                        break;
                    case 2:
                        ShotType = 95;
                        Damage = data.GetMagicDamage(0, DamageMod, player);
                        break;
                }
                if (Main.rand.Next(4) == 0)
                {
                    Damage = data.GetSummonDamage(0, DamageMod, player);
                    ShotType = Terraria.ID.ProjectileID.MagicMissile;
                }
                Vector2 ProjDirection = GetDirection(ShotSpawnPosition, new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition);
                const float ShotSpeed = 12f;
                int proj = Projectile.NewProjectile(ShotSpawnPosition, ProjDirection * ShotSpeed, ShotType, Damage, 0.8f, player.whoAmI);
                data.AttackTurn++;
                if (data.AttackTurn > 2)
                    data.AttackTurn = 0;
                data.OverdriveValue++;
                data.FireDelay = FireDelayMax;
                if (data.OverdriveValue >= 4)
                {
                    data.FireDelay = (byte)(data.FireDelay * 0.5f);
                    if (data.OverdriveValue >= 12)
                        data.OverdriveValue -= 12;
                }
                if (player.statLife < player.statLifeMax2 * 0.5f)
                    data.FireDelay /= 2;
            }
        }
    
        public class ElementalBreathData : SkillData
        {
            public byte AttackTurn = 0, FireDelay = 0, OverdriveValue = 0;
        }
    }
}
