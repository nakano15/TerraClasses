using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace TerraClasses.SkillList.Vilemancer
{
    class Vilemancy : SkillBase
    {
        public Vilemancy() //Causes massive lag for some reason.
        {
            Name = "Vilemancy";
            Description = 
                " Generates Eater of Souls as you collect parts.\n" +
                "  Overtime parts are generated.\n" +
                "  Hurting enemies also generates parts.\n" +
                "  The eaters lasts for 10 seconds." +
                "  Their attacks causes 76% + 4% per level summon damage.\n" +
                "  Hits ignores half of the target defense.";
            MaxLevel = 10;
        }

        public override SkillData GetSkillData => new VilemancyData();

        public override void Update(Player player, SkillData rawdata)
        {
            VilemancyData data = (VilemancyData)rawdata;
            data.PartsCollected += 0.001f;
            if(data.PartsCollected >= 1)
            {
                data.PartsCollected -= 1;
                EaterOfSouls eater = new EaterOfSouls() { Position = player.Center, Velocity = new Vector2(-1f * player.direction, 0), LifeTime = 600 };
                data.Eaters.Add(eater);
            }
            foreach(EaterOfSouls eater in data.Eaters)
            {
                eater.Update(player, data);
            }
        }

        public override void OnHitNPC(Player player, SkillData data, Item item, NPC target, int damage, float knockback, bool crit)
        {
            IncreasePartCollection(damage, data);
        }

        public override void OnHitNPCWithProj(Player player, SkillData data, Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            IncreasePartCollection(damage, data);
        }

        public void IncreasePartCollection(int Damage, SkillData sd)
        {
            VilemancyData data = (VilemancyData)sd;
            data.PartsCollected += Damage * 0.01f;
        }

        public override void Draw(Player player, SkillData rawdata, PlayerDrawInfo pdi)
        {
            VilemancyData data = (VilemancyData)rawdata;
            Texture2D EaterTexture = GetNPCTexture(6);
            int SpriteHeight = EaterTexture.Height / Main.npcFrameCount[6];
            Vector2 Origin = new Vector2(EaterTexture.Width * 0.5f, EaterTexture.Height / (SpriteHeight * 0.5f));
            foreach(EaterOfSouls eater in data.Eaters)
            {
                Vector2 DrawPosition = new Vector2(eater.Position.X, eater.Position.Y) - Main.screenPosition;
                Terraria.DataStructures.DrawData dd = new Terraria.DataStructures.DrawData(EaterTexture, DrawPosition, new Rectangle(0, (int)(SpriteHeight * (eater.Frame * (1f / 8))), EaterTexture.Width, SpriteHeight), Color.White,eater.Rotation, Origin, 1f, SpriteEffects.None, 0);
            }
        }

        public class VilemancyData : SkillData
        {
            public List<EaterOfSouls> Eaters = new List<EaterOfSouls>();
            public float PartsCollected = 0;
        }

        public class EaterOfSouls
        {
            public Vector2 Position = Vector2.Zero, Velocity = Vector2.Zero;
            public uint LifeTime = 0;
            private int Ai0 = 0;
            public float Rotation = 0;
            public byte Frame = 0;

            public void Update(Player player, VilemancyData sd)
            {
                Frame++;
                if (Frame >= Main.npcFrameCount[6] * 8)
                    Frame = 0;
                const int Width = 30, Height = 30;
                TargetTranslator.Translator Target = null;
                bool TagPlayer = false;
                {
                    TargetTranslator.Translator[] Targets = sd.GetPossibleTargets(false, false, Position, 1200, 1);
                    if (Targets.Length == 0)
                    {
                        Target = new TargetTranslator.PlayerTarget(player);
                        TagPlayer = true;
                    }
                    else
                    {
                        Target = Targets[0];
                    }
                }
                float Speed = 6f;
                float Initiative = Main.expertMode ? 0.035f : 0.02f;
                Vector2 TargetPosition = Target.Center, MyPosition = Position;
                if (TagPlayer)
                    TargetPosition.Y -= 80;
                TargetPosition.X -= TargetPosition.X % 8; //= (int)(TargetPosition.X / 8) * 8;
                TargetPosition.Y -= TargetPosition.Y % 8; //= (int)(TargetPosition.Y / 8) * 8;
                MyPosition.X -= MyPosition.X % 8;
                MyPosition.Y -= MyPosition.Y % 8;
                TargetPosition.X -= MyPosition.X;
                TargetPosition.Y -= MyPosition.Y;
                float sqrt = (float)Math.Sqrt(TargetPosition.X * TargetPosition.X + TargetPosition.Y * TargetPosition.Y);
                float sqrt2 = sqrt;
                bool Farway = sqrt > 600;
                if(sqrt == 0)
                {
                    TargetPosition.X = Velocity.X;
                    TargetPosition.Y = Velocity.Y;
                }
                else
                {
                    sqrt = Speed / sqrt;
                    TargetPosition.X *= sqrt;
                    TargetPosition.Y *= sqrt;
                }
                //
                if(sqrt2 > 100)
                {
                    Ai0++;
                    if(Ai0 > 0)
                    {
                        Velocity.Y += 0.023f;
                    }
                    else
                    {
                        Velocity.Y -= 0.023f;
                    }
                    if(Ai0 < -100 || Ai0 > 100)
                    {
                        Velocity.X += 0.023f;
                    }
                    else
                    {
                        Velocity.X -= 0.023f;
                    }
                    if (Ai0 >= 200)
                        Ai0 = -200;
                }
                if(sqrt2 < 150)
                {
                    Velocity.X += TargetPosition.X * 0.007f;
                    Velocity.Y += TargetPosition.Y * 0.007f;
                }
                //
                if (Velocity.X < TargetPosition.X)
                {
                    Velocity.X += Initiative;
                }
                else if (Velocity.X > TargetPosition.X)
                {
                    Velocity.X -= Initiative;
                }
                if (Velocity.Y < TargetPosition.Y)
                {
                    Velocity.Y += Initiative;
                }
                else if (Velocity.Y > TargetPosition.Y)
                {
                    Velocity.Y -= Initiative;
                }
                Rotation = (float)Math.Atan2(TargetPosition.Y, TargetPosition.X) - 1.57f;
                Rectangle Collision = new Rectangle((int)TargetPosition.X - Width / 2, (int)TargetPosition.Y - Height / 2, Width, Height);
                TargetTranslator.Translator[] AffectedOnes = sd.GetPossibleTargets(false, Collision);
                foreach(TargetTranslator.Translator Affected in AffectedOnes)
                {
                    if (!sd.ContainsTargetCooldown(Affected))
                    {
                        sd.HurtTarget(Affected, sd.GetSummonDamage(0, 0.76f + 0.04f * sd.Level, player), Velocity.X > 0 ? 1 : -1, 4, DefensePercentage: 0.5f);
                        break;
                    }
                }
            }
        }
    }
}
