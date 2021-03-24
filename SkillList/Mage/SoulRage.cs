using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace TerraClasses.SkillList.Mage
{
    public class SoulRage : SkillBase
    {
        public SoulRage()
        {
            Name = "Soul Rage";
            Description = "Launches vengeful spirits at the foes 150ft near the mouse.\n" +
                " Affects 3 targets initially, number increases every 3 levels.\n" +
                " Number of souls spawned is 1 + Level / 2.\n" +
                " Souls causes 120% + 22% magic damage per level.";
            MaxLevel = 10;
            Cooldown = GetTime(35);
            CastTime = 30;
            skillType = Enum.SkillTypes.Active;
            UnallowOtherSkillUsage = true;
        }

        public override float GetEffectRange(SkillData sd)
        {
            return 300f;
        }

        public override SkillData GetSkillData => new SoulRageData();

        public override void Update(Player player, SkillData rawdata)
        {
            SoulRageData data = (SoulRageData)rawdata;
            if (rawdata.Step == 0 && rawdata.Time == 0)
            {
                Vector2 CheckPosition = GetMousePositionInTheWorld;
                data.targets.Clear();
                const float Distance = 300f;
                int MaxTargets = 3 + data.Level / 3;
                TargetTranslator.Translator[] Targets = rawdata.GetPossibleTargets(false).ToArray();
                foreach (TargetTranslator.Translator target in Targets)
                {
                    float MyDistance = (target.Center - CheckPosition).Length();
                    if (MyDistance < Distance)
                    {
                        data.targets.Add(new SoulRageData.Victim() { Target = target });
                        if (data.targets.Count >= MaxTargets)
                            break;
                    }
                }
                /*foreach (TargetTranslator.Translator target in Targets)
                {
                    data.targets.Add(new SoulRageData.Victim() { Target = target });
                }*/
                if (data.targets.Count == 0)
                {
                    rawdata.EndUse(true);
                    CombatText.NewText(player.getRect(), Color.Red, "No Target near the mouse.");
                    return;
                }
            }
            const int ShotDelay = 15;
            const float ShotSpeed = 0.9f;
            int ShotCount = 1 + data.Level / 2;
            if (data.Time >= ShotDelay)
            {
                if (data.Step < ShotCount)
                {
                    foreach (SoulRageData.Victim victim in data.targets)
                    {
                        if (victim.Target == null)
                            continue;
                        Vector2 SpawnPosition = player.Center;
                        Vector2 ShotDirection = new Vector2(-30 * player.direction, (data.Step - (ShotCount * 0.5f)) * 20);
                        //ShotDirection.Normalize();
                        SoulShotInfo Shot = new SoulShotInfo() { Position = SpawnPosition, Velocity = ShotDirection };
                        victim.SoulsPosition.Add(Shot);
                        //Projectile proj = Projectile.NewProjectileDirect(SpawnPosition, ShotDirection * ShotSpeed, 297, data.GetMagicDamage(0, 1.2f + 0.22f * data.Level, player), 4f, player.whoAmI);
                        //proj.tileCollide = false;
                    }
                }
                data.ChangeStep();
            }
            bool HasSoulActive = false;
            foreach (SoulRageData.Victim victim in data.targets)
            {
                if (victim.Target != null && (!victim.Target.IsActive() || victim.Target.IsDead()))
                    victim.Target = null;
                for (int s = 0; s < victim.SoulsPosition.Count; s++)
                {
                    SoulShotInfo shot = victim.SoulsPosition[s];
                    Vector2 MoveDirection = Vector2.Zero;
                    shot.Velocity *= 0.9f;
                    if (victim.Target != null)
                        MoveDirection = (victim.Target.Center + victim.Target.Velocity) - shot.Position;
                    MoveDirection.Normalize();
                    shot.Velocity += MoveDirection * ShotSpeed;
                    if (shot.Velocity.Length() > 12)
                    {
                        shot.Velocity.Normalize();
                        shot.Velocity *= 12;
                    }
                    if (victim.Target != null && (victim.Target.Center - shot.Position).Length() < shot.Velocity.Length() * 2)
                    {
                        if (data.HurtTarget(victim.Target, data.GetMagicDamage(0, 1.2f + 0.22f * data.Level, player), player.Center.X < victim.Target.Center.X ? 1 : -1, 4f, 4) != 0)
                        {
                            victim.SoulsPosition.RemoveAt(s);
                        }
                    }
                    else
                    {
                        shot.Position += shot.Velocity;
                        shot.LifeTime++;
                        if (shot.LifeTime >= 210)
                        {
                            victim.SoulsPosition.RemoveAt(s);
                        }
                        else
                        {
                            HasSoulActive = true;
                            if (shot.SoulFade <= 4)
                            {
                                shot.SoulFade++;
                            }
                            else
                            {
                                for (int effect = 0; effect < 5; effect++)
                                {
                                    int dustid = Dust.NewDust(shot.Position, 8, 8, 175, 0f, 0f, 100, default(Color), 2f);
                                    Main.dust[dustid].noGravity = true;
                                    Dust dust = Main.dust[dustid];
                                    dust.velocity *= 0f;
                                }
                            }
                        }
                    }
                }
            }            
            if(data.Step > 0 && !HasSoulActive)
                data.EndUse();
        }

        public override void Draw(Player player, SkillData rawdata, PlayerDrawInfo pdi)
        {
            SoulRageData data = (SoulRageData)rawdata;
            foreach (SoulRageData.Victim victim in data.targets)
            {
                foreach (SoulShotInfo soul in victim.SoulsPosition)
                {
                    Terraria.DataStructures.DrawData dd = new Terraria.DataStructures.DrawData(
                        GetProjectileTexture(297), soul.Position - Main.screenPosition, null, Color.White);
                    Main.playerDrawData.Insert(0, dd);
                }
            }
        }

        public class SoulRageData : SkillData
        {
            public class Victim
            {
                public List<SoulShotInfo> SoulsPosition = new List<SoulShotInfo>();
                public TargetTranslator.Translator Target;
            }
            public List<Victim> targets = new List<Victim>();
        }

        public class SoulShotInfo
        {
            public Vector2 Position = Vector2.Zero, Velocity = Vector2.Zero;
            public byte LifeTime = 0;
            public byte SoulFade = 0;
        }
    }
}
