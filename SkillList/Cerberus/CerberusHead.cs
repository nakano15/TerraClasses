using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace TerraClasses.SkillList.Cerberus
{
    public class CerberusHead : SkillBase
    {
        public CerberusHead()
        {
            Name = "Cerberus Head";
            Description = "Empowers Cerberus Heads.\n" +
                " Attacking foes and being hurt can invoke a head.\n" +
                " Causes 67% + 23% per level of melee damage.\n" +
                " Foes in the range of the head's mouth will have velocity penalty.\n" +
                " Weak allies in the range of the head will be pulled inside It.\n" +
                "  Allies inside the mouth are immune to damage, and are healed overtime.";
            skillType = Enum.SkillTypes.Passive;
            MaxLevel = 10;
        }

        public override SkillData GetSkillData => new CerberusBiteData();

        public override void Update(Player player, SkillData rawdata)
        {
            CerberusBiteData data = (CerberusBiteData)rawdata;
            /*if (player.itemAnimation > 0 && player.itemAnimation == player.itemAnimationMax - 1 && Main.rand.NextFloat() < 1f - data.Heads.Count * 0.334f)
            {
                //Attack
                data.AddCerberusHead(new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition);
            }*/
            {
                byte[] Keys = data.ShroudCooldown.Keys.ToArray();
                foreach(byte k in Keys)
                {
                    data.ShroudCooldown[k]--;
                    if (data.ShroudCooldown[k] <= 0)
                        data.ShroudCooldown.Remove(k);
                }
            }
            for(int h = 0; h < data.Heads.Count; h++)
            {
                CerberusBiteData.CerberusHead head = data.Heads[h];
                head.AttackTime--;
                if (head.AttackTime == 10)
                {
                    //Dmg
                    Vector2 BiteOrientation = new Vector2((float)Math.Cos(head.Rotation), (float)Math.Sin(head.Rotation));
                    for (int x = 0; x < 12; x++)
                    {
                        Vector2 SpawnPosition = BiteOrientation * Main.rand.Next(-14, 15) * head.Scale + head.Position;
                        Dust.NewDust(SpawnPosition, 4, 4, Terraria.ID.DustID.Fire);
                    }
                    Rectangle HitPosition = new Rectangle((int)head.Position.X - (int)(16 * head.Scale), (int)head.Position.Y - (int)(16 * head.Scale), (int)(32 * head.Scale), (int)(32 * head.Scale));
                    List<byte> HitNpcs = new List<byte>();
                    int Damage = data.GetMeleeDamage(0, 0.67f + 0.023f * data.Level, player);
                    for(byte n = 0; n < 255; n++)
                    {
                        if(n < 200 && Main.npc[n].active && !Main.npc[n].friendly && !Main.npc[n].dontTakeDamage && Main.npc[n].getRect().Intersects(HitPosition))
                        {
                            HitNpcs.Add(n);
                            if(data.HurtNpc(Main.npc[n], Damage, 0, 0) > 0 && Main.rand.Next(3) == 0)
                            {
                                Main.npc[n].AddBuff(Terraria.ID.BuffID.Burning, 7 * 60);
                            }
                        }
                        if(!data.ShroudCooldown.ContainsKey(n) && Main.player[n].active && !Main.player[n].dead && Main.player[n].statLife < Main.player[n].statLifeMax2 * 0.3f && Main.player[n].getRect().Intersects(HitPosition))
                        {
                            bool IsPlayerShrouded = false, ShroudedInThisHead = false;
                            for(int h2 = 0; h2 < data.Heads.Count; h2++)
                            {
                                if (data.Heads[h2].ShroudedPlayers.Contains(n))
                                {
                                    IsPlayerShrouded = true;
                                    ShroudedInThisHead = h2 == h;
                                    break;
                                }
                            }
                            if (!IsPlayerShrouded)
                            {
                                IsPlayerShrouded = true;
                                ShroudedInThisHead = true;
                                head.ShroudedPlayers.Add(n);
                            }
                            int HealthRegen = (int)(Main.player[n].statLifeMax2 * (0.05f + 0.005f * data.Level));
                            Main.player[n].statLife += HealthRegen;
                            Main.player[n].HealEffect(HealthRegen);
                            if (Main.player[n].statLife > Main.player[n].statLifeMax2)
                                Main.player[n].statLife = Main.player[n].statLifeMax2;
                        }
                    }
                    head.AffectedNpcs = HitNpcs;
                }
                head.ShroudedFoe = false;
                if (head.BiteTimes > 0 || head.AttackTime > 10)
                {
                    foreach(byte n in head.AffectedNpcs)
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active)
                        {
                            if (data.IsTargetSmallForMouth(head, npc.width, npc.height))
                            {
                                head.ShroudedFoe = true;
                                npc.position.X = head.Position.X - npc.width * 0.5f;
                                npc.position.Y = head.Position.Y - npc.height * 0.5f;
                                npc.velocity = Vector2.Zero;
                            }
                            else
                            {
                                npc.velocity *= 0.3f;
                            }
                            /*Vector2 MouthCenter = head.Position - npc.Center;
                            MouthCenter.Normalize();
                            npc.velocity += MouthCenter;*/
                        }
                    }
                    foreach(byte b in head.ShroudedPlayers)
                    {
                        Player shrouded = Main.player[b];
                        shrouded.AddBuff(Terraria.ID.BuffID.Cursed, 5);
                        shrouded.itemAnimation = 0;
                        shrouded.immuneTime = 5;
                        shrouded.lifeRegen = 10;
                        shrouded.Center = head.Position;
                        shrouded.velocity = Vector2.Zero;
                        shrouded.immuneAlpha = 255;
                        shrouded.immuneNoBlink = true;
                    }
                }
                if (head.BiteTimes == 0 && head.AttackTime <= 0 && head.ShroudedPlayers.Count > 0)
                {
                    foreach (byte b in head.ShroudedPlayers)
                    {
                        Player shrouded = Main.player[b];
                        shrouded.immuneTime = 200;
                        shrouded.velocity = new Vector2(Main.rand.Next(-250, 251) * 0.01f, Main.rand.Next(-250, 251) * 0.01f);
                        shrouded.immuneAlpha = 0;
                        shrouded.immuneNoBlink = true;
                        data.ShroudCooldown.Add(b, 120 * 60);
                    }
                }
                if(head.AttackTime == 10 && head.BiteTimes > 0)
                    data.ResetBite(head);
                if (head.AttackTime <= 0)
                {
                    data.Heads.RemoveAt(h);
                }
            }
        }

        public override void OnHitByNPC(Player player, SkillData rawdata, NPC npc, int damage, bool crit)
        {
            CerberusBiteData data = (CerberusBiteData)rawdata;
            Vector2 Position = player.Center;
            if (Main.rand.NextFloat() < (1f - data.Heads.Count * 0.334f) * 0.3f && CanCreateHead(Position, data))
            {
                CerberusBiteData.CerberusHead head = data.AddCerberusHead(Position);
                if (player.statLife < player.statLifeMax2 * 0.3f)
                    head.ShroudedPlayers.Add((byte)player.whoAmI);
            }
        }

        public override void OnHitByProjectile(Player player, SkillData rawdata, Projectile proj, int damage, bool crit)
        {
            CerberusBiteData data = (CerberusBiteData)rawdata;
            Vector2 Position = player.Center;
            if (Main.rand.NextFloat() < (1f - data.Heads.Count * 0.334f) * 0.3f && CanCreateHead(Position, data))
            {
                CerberusBiteData.CerberusHead head = data.AddCerberusHead(Position);
                if (player.statLife < player.statLifeMax2 * 0.3f)
                    head.ShroudedPlayers.Add((byte)player.whoAmI);
            }
        }

        public override void OnHitNPC(Player player, SkillData rawdata, Item item, NPC target, int damage, float knockback, bool crit)
        {
            CerberusBiteData data = (CerberusBiteData)rawdata;
            Vector2 Position = target.Center + target.velocity * 5;
            if (Main.rand.NextFloat() < (1f - data.Heads.Count * 0.334f) * 0.1f && CanCreateHead(Position, data))
            {
                data.AddCerberusHead(target.Center + target.velocity * 5);
            }
        }

        public override void OnHitNPCWithProj(Player player, SkillData rawdata, Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            CerberusBiteData data = (CerberusBiteData)rawdata;
            Vector2 Position = target.Center + target.velocity * 5;
            if (Main.rand.NextFloat() < (1f - data.Heads.Count * 0.334f) * 0.1f && CanCreateHead(Position, data))
            {
                data.AddCerberusHead(target.Center + target.velocity * 5);
            }
        }

        public bool CanCreateHead(Vector2 Position, CerberusBiteData data)
        {
            foreach(CerberusBiteData.CerberusHead head in data.Heads)
            {
                if (Math.Abs(head.Position.X - Position.X) < 32 ||
                    Math.Abs(head.Position.Y - Position.Y) < 32)
                    return false;
            }
            return true;
        }

        public override void Draw(Player player, SkillData rawdata, PlayerDrawInfo pdi)
        {
            CerberusBiteData data = (CerberusBiteData)rawdata;
            //if (data.AttackTime == 0) return;
            byte PosSum = 0;
            foreach (CerberusBiteData.CerberusHead head in data.Heads)
            {
                float Opacity = 1f;
                if (head.BiteTimes == 0 && head.AttackTime < 10)
                    Opacity = head.AttackTime * 0.1f;
                else if (!head.IsResetBite && head.AttackTime > head.MaxAttackTime - 10)
                {
                    Opacity = 1f - (head.AttackTime - (head.MaxAttackTime - 10)) * 0.1f;
                }
                Vector2 UpperMouthOrigin = new Vector2(15, 22) * 2,
                    LowerMouthOrigin = new Vector2(15, 22) * 2;
                float MouthMovementPercentage = 1f;
                if (head.AttackTime < 10)
                    MouthMovementPercentage = 0;
                else if (head.AttackTime >= 10 && head.AttackTime < 20)
                {
                    MouthMovementPercentage = (float)(head.AttackTime - 10) / 10;
                }
                else if(head.IsResetBite && head.AttackTime > head.MaxAttackTime - 10)
                {
                    MouthMovementPercentage = 1f - (float)(head.AttackTime - (head.MaxAttackTime - 10)) / 10;
                }
                float PixelMovementOfMouth = 8;
                if (head.MouthFull)
                    PixelMovementOfMouth = 2;
                Vector2 MouthMoveOrientationByRotation = Vector2.UnitY * PixelMovementOfMouth * MouthMovementPercentage; //new Vector2((float)Math.Sin(data.Rotation), (float)Math.Cos(data.Rotation))
                UpperMouthOrigin += MouthMoveOrientationByRotation;
                LowerMouthOrigin -= MouthMoveOrientationByRotation;
                int FrameX = head.MouthFull ? 64 : 0;
                Terraria.DataStructures.DrawData dd = new Terraria.DataStructures.DrawData(MainMod.CerberusBiteHead.GetTexture, head.Position - Main.screenPosition,
                    new Rectangle(FrameX, 0, 64, 64), Color.White * Opacity, head.Rotation, LowerMouthOrigin, head.Scale, SpriteEffects.None, 0);
                dd.ignorePlayerRotation = true;
                Main.playerDrawData.Insert(0 + PosSum * 2, dd);
                dd = new Terraria.DataStructures.DrawData(MainMod.CerberusBiteHead.GetTexture, head.Position - Main.screenPosition,
                     new Rectangle(FrameX, 64, 64, 64), Color.White * Opacity, head.Rotation, UpperMouthOrigin, head.Scale, SpriteEffects.None, 0);
                dd.ignorePlayerRotation = true;
                Main.playerDrawData.Insert(1 + PosSum * 2, dd);
                PosSum++;
            }
        }

        public class CerberusBiteData : SkillData
        {
            public List<CerberusHead> Heads = new List<CerberusHead>();
            public Dictionary<byte, int> ShroudCooldown = new Dictionary<byte, int>();

            public CerberusHead AddCerberusHead(Vector2 Position)
            {
                CerberusHead head = new CerberusHead();
                head.BiteTimes = (byte)Main.rand.Next(1 + Level / 5, Math.Min(1 + Level / 3, 10) + 1);
                //head.BiteTimes = 3;
                head.Position = Position;
                head.AttackTime = head.MaxAttackTime = 40;
                head.Rotation = (Main.rand.NextFloat() - 0.5f) * (float)Math.PI * 0.5f;
                head.Scale = (Main.rand.Next(110, 136) + Level * 12) * 0.01f;
                Heads.Add(head);
                return head;
            }

            public void ResetBite(CerberusHead head)
            {
                head.BiteTimes--;
                head.IsResetBite = true;
                head.AttackTime = head.MaxAttackTime;
                head.Rotation = (Main.rand.NextFloat() - 0.5f) * (float)Math.PI * 0.5f;
            }

            public bool IsTargetSmallForMouth(CerberusHead head, int Width, int Height)
            {
                float HeadDimension = 32 * head.Scale;
                return Width < HeadDimension && Height < HeadDimension;
            }

            public class CerberusHead
            {
                public int AttackTime = 0, MaxAttackTime = 0;
                public byte BiteTimes = 0;
                public Vector2 Position = Vector2.Zero;
                public float Rotation = 0f, Scale = 1f;
                public bool IsResetBite = false;
                public List<byte> AffectedNpcs = new List<byte>();
                public List<byte> ShroudedPlayers = new List<byte>();
                public bool ShroudedFoe = false;

                public bool MouthFull
                {
                    get
                    {
                        return ShroudedFoe || ShroudedPlayers.Count > 0;
                    }
                }
            }
        }
    }
}
