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
        public const byte AttackTurnVar = 0, FireDelayVar = 1, OverdriveCounterVar = 2;
        public const int FireDelayMax = 60;

        public ElementalBreath()
        {
            Name = "Elemental Breath";
            Description = "Breathes elemental attacks from the mouth every 1 second.\nAlternates between mouths while spitting, launching different elements.\nInflicts 80% + 4% per level damage.\nEach mouth damage is related to a attack type.\n1/4 chance of spitting based on Summon damage, instead of Melee, ranged or magic damage.\nAfter 4 shots, It enters a overdrive mode for 8 shots, and then resets.\nFire delay is lower when with lower than half health.";
            skillType = Enum.SkillTypes.Attack;
            MaxLevel = 10;
        }

        public override void Update(Terraria.Player player, SkillData data)
        {
            int FireDelay = data.GetInteger(FireDelayVar);
            if (FireDelay > 0)
                data.ChangeInteger(FireDelayVar, -1);
        }

        public override void UpdateItemUse(Terraria.Player player, SkillData data, bool JustUsed)
        {
            if (JustUsed)
            {
                int AttackTurn = data.GetInteger(AttackTurnVar), FireDelay = data.GetInteger(FireDelayVar), OverdriveValue = data.GetInteger(OverdriveCounterVar);
                if (FireDelay <= 0)
                {
                    SkillData sd = PlayerMod.GetPlayerSkillData(player, 22);
                    if (sd != null)
                    {
                        sd.SetInteger((byte)AttackTurn, 30);
                    }
                    Vector2 ShotSpawnPosition = player.Center;
                    ShotSpawnPosition.Y -= 8 * player.gravDir;
                    ShotSpawnPosition.X += player.direction * 12;
                    int ShotType = 0;
                    int Damage = 0;
                    float DamageMod = 0.8f + 0.04f * data.Level;
                    switch (AttackTurn)
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
                    Projectile.NewProjectile(ShotSpawnPosition, ProjDirection * ShotSpeed, ShotType, Damage, 0.8f, player.whoAmI);
                    AttackTurn++;
                    if (AttackTurn > 2)
                        AttackTurn = 0;
                    OverdriveValue++;
                    data.SetInteger(AttackTurnVar, AttackTurn);
                    FireDelay = FireDelayMax;
                    if (OverdriveValue >= 4)
                    {
                        FireDelay /= 2;
                        if (OverdriveValue >= 12)
                            OverdriveValue -= 12;
                    }
                    if (player.statLife < player.statLifeMax2 * 0.5f)
                        FireDelay /= 2;
                    data.SetInteger(FireDelayVar, FireDelay);
                    data.SetInteger(OverdriveCounterVar, OverdriveValue);
                }
            }
        }
    }
}
