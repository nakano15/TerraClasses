using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace TerraClasses.SkillList.Summoner
{
    public class SummonQueenBee : SkillBase
    {
        public const int BeeSummonTime = 15;

        public SummonQueenBee()
        {
            Name = "Summon Queen Bee";
            Description = "Invokes Queen Bee for a while, to launch bees at your foes.\n" +
                " Queen Bee stays for "+ BeeSummonTime + " seconds.\n" +
                " Each bee causes 40% + 1,5% Summon Damage.\n" +
                " Ignores 50% of enemies defense." +
                " Past level 5, critical hits ignores enemy defense.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Active;
            Cooldown = GetTime(25, 1);
        }

        public override SkillData GetSkillData => new SummonQueenBeeData();

        public override void Update(Player player, SkillData rawdata)
        {
            SummonQueenBeeData data = (SummonQueenBeeData)rawdata;
            if(data.Step == 0 && data.Time == 0)
            {
                data.Position = player.Center;
                data.Velocity = Vector2.Zero;
                data.FrameID = 4;
                data.FrameTime = 0;
                data.Direction = (sbyte)player.direction;
                Main.PlaySound(15, (int)data.Position.X, (int)data.Position.Y, 0);
            }
            //AI Part
            Vector2 BeeFollowPosition = player.Center;
            if (Math.Abs(data.Position.X - BeeFollowPosition.X) >= 1000 || Math.Abs(data.Position.Y - BeeFollowPosition.Y) >= 800)
                data.Position = BeeFollowPosition;
            bool HasHostileCreatureNearby = false;
            float NearestDistance = 800;
            for(int i = 0; i < 200; i++)
            {
                float Distance = 0;
                if(Main.npc[i].active && !Main.npc[i].friendly && Main.npc[i].CanBeChasedBy() && (Distance = Main.npc[i].Distance(player.Center)) < NearestDistance)
                {
                    Distance = NearestDistance;
                    BeeFollowPosition = Main.npc[i].Top;
                    HasHostileCreatureNearby = true;
                }
            }
            if (data.Position.X < BeeFollowPosition.X)
            {
                data.Direction = 1;
            }
            else
            {
                data.Direction = -1;
            }
            if (HasHostileCreatureNearby)
            {
                BeeFollowPosition.X -= 100 * data.Direction;
            }
            else
            {
                BeeFollowPosition.X -= 100 * player.direction;
            }
            BeeFollowPosition.Y -= 160;
            const float Speed = 0.15f, MaxSpeed = 12f;
            if (Math.Abs(data.Position.X - BeeFollowPosition.X) < 50)
            {
                data.Velocity.X *= 0.8f;
            }
            else
            {
                if (BeeFollowPosition.X < data.Position.X)
                {
                    data.Velocity.X -= Speed;
                    if (data.Velocity.X < -MaxSpeed)
                        data.Velocity.X = -MaxSpeed;
                }
                else if (BeeFollowPosition.X > data.Position.X)
                {
                    data.Velocity.X += Speed;
                    if (data.Velocity.X > MaxSpeed)
                        data.Velocity.X = MaxSpeed;
                }
            }
            if (Math.Abs(data.Position.Y - BeeFollowPosition.Y) < 50)
            {
                data.Velocity.Y *= 0.8f;
            }
            else
            {
                if (BeeFollowPosition.Y < data.Position.Y)
                {
                    data.Velocity.Y -= Speed;
                    if (data.Velocity.Y < -MaxSpeed)
                        data.Velocity.Y = -MaxSpeed;
                }
                else if (BeeFollowPosition.Y > data.Position.Y)
                {
                    data.Velocity.Y += Speed;
                    if (data.Velocity.Y > MaxSpeed)
                        data.Velocity.Y = MaxSpeed;
                }
            }
            data.Position += data.Velocity;
            if (HasHostileCreatureNearby && data.Time >= 60 && data.Time < 60 * (BeeSummonTime + 1) && data.Time % 15 == 0)
            {
                Vector2 ShotPosition = data.Position;
                ShotPosition.X += -data.Direction * Main.rand.Next(20);
                ShotPosition.Y += 66 * (0.4f * 0.5f) + Main.rand.Next(8); //Need sprite size
                Main.PlaySound(3, (int)data.Position.X, (int)data.Position.Y, 1);
                int Damage = rawdata.GetSummonDamage(0, 0.4f + 0.015f * data.Level, player);
                int ProjPos = Projectile.NewProjectile(ShotPosition, new Vector2(0.3f * data.Direction, 0.4f), player.beeType(), Damage, 0.6f, player.whoAmI);
                data.ProjectileSpawnID.Add(ProjPos);
            }
            //Frame Part
            data.FrameTime++;
            if(data.FrameTime > 4)
            {
                data.FrameTime = 0;
                data.FrameID++;
                if (data.FrameID >= 12)
                    data.FrameID = 4;
            }
            if(data.Time >= 60 * (BeeSummonTime + 2))
            {
                data.EndUse();
            }
        }

        public override void Draw(Player player, SkillData rawdata, PlayerDrawInfo pdi)
        {
            SummonQueenBeeData data = (SummonQueenBeeData)rawdata;
            if (!Main.NPCLoaded[Terraria.ID.NPCID.QueenBee])
                Main.instance.LoadNPC(Terraria.ID.NPCID.QueenBee);
            Texture2D texture = Main.npcTexture[Terraria.ID.NPCID.QueenBee];
            float Opacity = 1f;
            if (data.Time < 60)
                Opacity = data.Time * (1f / 60);
            else if(data.Time >= 60 * (BeeSummonTime + 1))
            {
                Opacity = (60 - (data.Time - 60 * (BeeSummonTime + 1))) * (1f / 60);
            }
            if (Opacity < 0)
                Opacity = 0;
            Rectangle drawRect = new Rectangle(0, 0, texture.Width, texture.Height / Main.npcFrameCount[Terraria.ID.NPCID.QueenBee]);
            drawRect.Y = drawRect.Height * data.FrameID;
            Vector2 Origin = new Vector2(drawRect.Width * 0.5f, drawRect.Height * 0.5f);
            Terraria.DataStructures.DrawData dd = new Terraria.DataStructures.DrawData(texture, data.Position - Main.screenPosition, drawRect, Lighting.GetColor((int)(data.Position.X * (1f / 16)), (int)(data.Position.Y * (1f / 16)), Color.Cyan) * Opacity, 0f, Origin, 1f, (data.Direction > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None), 0);
            Main.playerDrawData.Insert(0, dd);
        }

        public override void ModifyHitNPCWithProj(Player player, SkillData rawdata, Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            SummonQueenBeeData data = (SummonQueenBeeData)rawdata;
            if (data.ProjectileSpawnID.Contains(proj.whoAmI))
            {
                data.ProjectileSpawnID.Remove(proj.whoAmI);
                damage += (int)(target.defense * (crit && data.Level > 5 ? 1f : 0.5f));
            }
        }

        public class SummonQueenBeeData : SkillData
        {
            public Vector2 Position = Vector2.Zero, Velocity = Vector2.Zero;
            public byte FrameID = 0;
            public byte FrameTime = 0;
            public sbyte Direction = 1;
            public List<int> ProjectileSpawnID = new List<int>();
        }
    }
}
