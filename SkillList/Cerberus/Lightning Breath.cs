using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace TerraClasses.SkillList.Cerberus
{
    public class Lightning_Breath : SkillBase
    {
        public Lightning_Breath()
        {
            Name = "Lightning Breath";
            Description = "Launches Lightning Bolts at the foes in front of you.\n" +
                " Bolts causes 102% + 6% Magic damage.\n" +
                " Hurts foes that are in range of where the lightning passes through.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Active;
            Cooldown = GetTime(25, 1);
            CastTime = 30;
            UnallowOtherSkillUsage = true;
        }

        public override SkillData GetSkillData => new LightningBreathData();

        public override void UpdateStatus(Player player, SkillData data)
        {
            player.delayUseItem = true;
        }

        public override void Update(Player player, SkillData rawdata)
        {
            LightningBreathData data = (LightningBreathData)rawdata;
            if (data.Time == 0)
                return;
            if(data.Time < 10)
            {
                player.direction = (Main.mouseX + Main.screenPosition.X < player.Center.X ? -1 : 1);
            }
            if(data.Time > 0 && data.LastTime == 0)
            {
                CerberusFormData cfd = (CerberusFormData)PlayerMod.GetPlayerSkillData(player, 22);
                if (cfd != null)
                    cfd.HeadFrame[2] = 30;
                Vector2 LightningSpawnPos = CerberusForm.GetMouthPosition(player, true);
                List<LightningBreathData.LightningPath> Paths = new List<LightningBreathData.LightningPath>();
                for (int i = 0; i < 200; i++)
                {
                    if(Main.npc[i].active && !Main.npc[i].dontTakeDamage && !Main.npc[i].friendly)
                    {
                        if(Math.Abs(Main.npc[i].Center.X - player.Center.X) < 400 &&
                            Math.Abs(Main.npc[i].Center.Y - player.Center.Y) < 300)
                        {
                            if((player.direction > 0 && player.Center.X < Main.npc[i].Center.X) ||
                                (player.direction < 0 && player.Center.X > Main.npc[i].Center.X))
                            {
                                //if (Collision.CanHitLine(player.position, player.width, player.height, Main.npc[i].position, Main.npc[i].width, Main.npc[i].height))
                                //{
                                    float Prediction = (Main.npc[i].Center - LightningSpawnPos).Length() * 0.0625f;
                                    Paths.Add(new LightningBreathData.LightningPath(LightningSpawnPos, Main.npc[i].Center + Main.npc[i].velocity * Prediction));
                                //}
                            }
                        }
                    }
                }
                while(Paths.Count < 3)
                {
                    Vector2 BoltPosition = player.Center;
                    BoltPosition.X += player.direction * Main.rand.Next(300);
                    BoltPosition.Y += Main.rand.Next(-150, 151);
                    Paths.Add(new LightningBreathData.LightningPath(LightningSpawnPos, BoltPosition));
                }
                data.Paths = Paths.ToArray();
                /*TargetTranslator.Translator[] Targets = data.GetPossibleTargets(false, false, rawdata.CastPosition, GetEffectRange(data));
                List<LightningBreathData.LightningPath> Paths = new List<LightningBreathData.LightningPath>();
                foreach(TargetTranslator.Translator Target in Targets)
                {
                    Paths.Add(new LightningBreathData.LightningPath(LightningSpawnPos, Target.Center + Target.Velocity));
                }
                data.Paths = Paths.ToArray();*/
            }
            else
            {
                bool AllReady = true;
                foreach(LightningBreathData.LightningPath path in data.Paths)
                {
                    if(data.Time >= path.Path.Length + 10)
                    {
                        continue;
                    }
                    AllReady = false;
                    if(data.Time < path.Path.Length)
                    {
                        //Do Damage
                        int Damage = data.GetMagicDamage(0, 1.02f + 0.06f * data.Level, player);
                        const float Distance = 30;
                        for (int i = 0; i < 200; i++)
                        {
                            if (Main.npc[i].active && !Main.npc[i].dontTakeDamage && !Main.npc[i].friendly && (Main.npc[i].Center - path.Path[data.Time]).Length() < Distance)
                            {
                                Main.npc[i].StrikeNPC(Damage, 1f, player.direction);
                            }
                        }
                    }
                }
                if (AllReady && data.Time >= 20)
                {
                    if (data.Step < data.Level * 0.2f + 1)
                    {
                        data.ChangeStep();
                        data.CastPosition = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition;
                    }
                    else
                    {
                        data.EndUse();
                    }
                }
            }
        }

        public override void Draw(Player player, SkillData rawdata, PlayerDrawInfo pdi)
        {
            LightningBreathData data = (LightningBreathData)rawdata;
            foreach (LightningBreathData.LightningPath path in data.Paths)
            {
                int Length = path.Path.Length;
                float Opacity = data.Time - Length;
                if (Opacity < 0)
                    Opacity = 1;
                else
                {
                    Opacity = 1f - (Opacity * 0.1f);
                    if (Opacity < 0)
                        continue;
                }
                for (int p = 0; p < Length; p++)
                {
                    byte Frame = 0;
                    if (p > rawdata.Time || p == Length - 1)
                        continue;
                    if (p == rawdata.Time || p == Length - 2)
                        Frame = 2;
                    else if (p > 0)
                        Frame = 1;
                    Vector2 DrawPos = path.Path[p];
                    Vector2 MoveDirection = path.Path[p + 1] - DrawPos;
                    float Rotation = (float)Math.Atan2(MoveDirection.Y, MoveDirection.X);
                    if (p < Length - 2)
                    {
                        Terraria.DataStructures.DrawData dd2 =
                            new Terraria.DataStructures.DrawData(MainMod.LightningBolt.GetTexture, path.Path[p + 1] - Main.screenPosition, new Rectangle(48, 0, 16, 16),
                            Color.White * Opacity, 0f, new Vector2(8, 8), 1f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
                        Main.playerDrawData.Add(dd2);
                    }
                    DrawPos.X += MoveDirection.X * 0.5f;
                    DrawPos.Y += MoveDirection.Y * 0.5f;
                    Terraria.DataStructures.DrawData dd =
                        new Terraria.DataStructures.DrawData(MainMod.LightningBolt.GetTexture, DrawPos - Main.screenPosition, new Rectangle(16 * Frame, 0, 16, 16),
                        Color.White * Opacity, Rotation, new Vector2(8, 8), 1f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
                    Main.playerDrawData.Add(dd);
                }
            }
        }

        public class LightningBreathData : SkillData
        {
            public LightningPath[] Paths = new LightningPath[0];

            public struct LightningPath
            {
                public Vector2[] Path;

                public LightningPath(Vector2 SpawnPosition, Vector2 TargetPosition)
                {
                    List<Vector2> Path = new List<Vector2>();
                    Vector2 Position = SpawnPosition;
                    Path.Add(Position);
                    while(Position != TargetPosition)
                    {
                        Vector2 NewPosition = Position + Vector2.Normalize(TargetPosition - Position).RotatedByRandom(0.785398f) * 16;
                        Path.Add(NewPosition);
                        if ((NewPosition - TargetPosition).Length() < 16)
                        {
                            break;
                        }
                        Position = NewPosition;
                    }
                    this.Path = Path.ToArray();
                }
            }
        }
    }
}
