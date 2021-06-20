using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace TerraClasses.SkillList.Summoner
{
    class SummonDemon : SkillBase
    {
        public const int Duration = 25;

        public SummonDemon()
        {
            Name = "Summon Demon";
            Description = "Summons Demons, to launch Demon Scythes on your nearby foes.\n" +
                " Demons stays for "+ Duration + " seconds.\n" +
                " Increases the number of demons every 3 levels.\n" +
                "  Maximum of 8 Demons." +
                " Each scythe inflicts 55% + 2% * Level Summon Damage.\n" +
                " Decreases delay between shots past level 6 and 12.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Active;
            GetTime(55);
        }

        public override SkillData GetSkillData => new SummonDemonData();

        public override void Update(Player player, SkillData rawdata)
        {
            SummonDemonData data = (SummonDemonData)rawdata;
            if (data.Step == 0 && data.Time == 0)
            {
                List<SummonDemonData.DemonInfo> Demons = new List<SummonDemonData.DemonInfo>();
                int MaxDemons = (int)(data.Level * 0.3334f + 1);
                if (MaxDemons > 8)
                    MaxDemons = 8;
                for (int i = 0; i < MaxDemons; i++)
                {
                    SummonDemonData.DemonInfo d = new SummonDemonData.DemonInfo();
                    d.VoodooDemon = Main.rand.NextDouble() < 0.05;
                    d.FrameTime = (byte)(i * 2);
                    d.Position = player.Center;
                    d.ShotCooldown = (byte)(8 * (i + 1));
                    Demons.Add(d);
                }
                data.DemonMinions = Demons.ToArray();
            }
            bool CanAttack = data.Time >= 60 && data.Time < (Duration + 1) * 60;
            const float RadiusAroundPlayer = 1.48353f;
            float RadiusDistancingPerMob = RadiusAroundPlayer * (1f / data.DemonMinions.Length);
            const float DistanceFromPlayer = 120;
            byte MaxShotCooldown = 8;
            if (data.Level > 6)
                MaxShotCooldown--;
            if (data.Level > 12)
                MaxShotCooldown--;
            MaxShotCooldown = (byte)(data.DemonMinions.Length * MaxShotCooldown);
            for (int i = 0; i < data.DemonMinions.Length; i++)
            {
                SummonDemonData.DemonInfo di = data.DemonMinions[i];
                Vector2 FinalPosition = player.Center;
                float MyRadians = -(RadiusAroundPlayer - RadiusDistancingPerMob) * 0.5f + RadiusDistancingPerMob * i - (float)Math.PI;// - 0.785398f;
                FinalPosition.X += (float)Math.Sin(MyRadians) * DistanceFromPlayer;
                FinalPosition.Y += (float)Math.Cos(MyRadians) * DistanceFromPlayer;
                const float FlightSpeed = 0.1f, MaxSpeed = 4f;
                if(Main.rand.Next(700) == 0)
                {
                    Main.PlaySound(29, (int)di.Position.X, (int)di.Position.Y, Main.rand.Next(26, 30));
                }
                if (Math.Abs(FinalPosition.X - di.Position.X) < 5)
                {
                    di.Velocity.X *= 0.8f;
                }
                else
                {
                    if (FinalPosition.X < di.Position.X)
                    {
                        di.Velocity.X -= FlightSpeed;
                        if (di.Velocity.X < -MaxSpeed)
                            di.Velocity.X = -MaxSpeed;
                    }
                    else
                    {
                        di.Velocity.X += FlightSpeed;
                        if (di.Velocity.X > MaxSpeed)
                            di.Velocity.X = MaxSpeed;
                    }
                }
                if (Math.Abs(FinalPosition.Y - di.Position.Y) < 5)
                {
                    di.Velocity.Y *= 0.8f;
                }
                else
                {
                    if (FinalPosition.Y < di.Position.Y)
                    {
                        di.Velocity.Y -= FlightSpeed;
                        if (di.Velocity.Y < -MaxSpeed)
                            di.Velocity.Y = -MaxSpeed;
                    }
                    else
                    {
                        di.Velocity.Y += FlightSpeed;
                        if (di.Velocity.Y > MaxSpeed)
                            di.Velocity.Y = MaxSpeed;
                    }
                }
                di.Position += di.Velocity;
                float NearestAttackDistance = 800;
                int FoundNpc = -1;
                for (int n = 0; n < 200; n++)
                {
                    float Distance = 0;
                    if (Main.npc[n].active && !Main.npc[n].friendly && Main.npc[n].CanBeChasedBy() && (Distance = Main.npc[n].Distance(player.Center)) < NearestAttackDistance)
                    {
                        NearestAttackDistance = Distance;
                        FoundNpc = n;
                        FinalPosition = Main.npc[n].Center;
                    }
                }
                if(CanAttack && FoundNpc > -1)
                {
                    di.Direction = (sbyte)(FinalPosition.X < di.Position.X ? -1 : 1);
                }
                else
                {
                    di.Direction = (sbyte)player.direction;
                }
                di.ShotCooldown--;
                if(di.ShotCooldown == 0)
                {
                    di.ShotCooldown = MaxShotCooldown;
                    if(FoundNpc > -1)
                    {
                        Vector2 TargetPosition = FinalPosition - di.Position;
                        TargetPosition.X += Main.rand.Next(-100, 101);
                        TargetPosition.Y += Main.rand.Next(-100, 101);
                        TargetPosition.Normalize();
                        int ProjPos = Projectile.NewProjectile(di.Position, TargetPosition * 8f, Terraria.ID.ProjectileID.DemonScythe,
                            data.GetSummonDamage(0, 0.55f + 0.02f * data.Level, player), 6f, player.whoAmI);
                    }
                }

                //Frame
                di.Rotation = di.Velocity.X * 0.1f;
                di.FrameTime++;
                if (di.FrameTime < 6)
                {
                    di.FrameID = 0;
                }
                else
                {
                    di.FrameID = 1;
                    if (di.FrameTime >= 11)
                        di.FrameTime = 0;
                }
            }

            if (data.Time >= 60 * (Duration + 2))
                data.EndUse();
        }

        public override void Draw(Player player, SkillData rawdata, PlayerDrawInfo pdi)
        {
            SummonDemonData data = (SummonDemonData)rawdata;
            if (!Main.NPCLoaded[Terraria.ID.NPCID.Demon])
            {
                Main.instance.LoadNPC(Terraria.ID.NPCID.Demon);
            }
            if (!Main.NPCLoaded[Terraria.ID.NPCID.VoodooDemon])
            {
                Main.instance.LoadNPC(Terraria.ID.NPCID.VoodooDemon);
            }
            float Opacity = 1f;
            if (data.Time < 60)
                Opacity = data.Time * (1f / 60);
            else if(data.Time >= (Duration + 1) * 60)
            {
                Opacity = (60 - (data.Time - (Duration + 1)* 60)) * (1f /60);
            }
            foreach (SummonDemonData.DemonInfo di in data.DemonMinions)
            {
                int DemonID = di.VoodooDemon ? Terraria.ID.NPCID.VoodooDemon : Terraria.ID.NPCID.Demon;
                Texture2D texture = Main.npcTexture[DemonID];
                Rectangle drawrect = new Rectangle(0, 0, texture.Width, texture.Height / Main.npcFrameCount[DemonID]);
                drawrect.Y = di.FrameID * drawrect.Height;
                Vector2 Origin = new Vector2(drawrect.Width * 0.5f, drawrect.Height * 0.5f);
                Terraria.DataStructures.DrawData dd = new Terraria.DataStructures.DrawData(texture, di.Position - Main.screenPosition, drawrect, 
                    Lighting.GetColor((int)(di.Position.X * (1f / 16)), (int)(di.Position.Y * (1f / 16)), Color.Cyan) * Opacity, di.Rotation, Origin, 1f, (di.Direction > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None), 
                    0);
                Main.playerDrawData.Insert(0, dd);
            }
        }

        public class SummonDemonData : SkillData
        {
            public class DemonInfo
            {
                public Vector2 Position = Vector2.Zero, Velocity = Vector2.Zero;
                public sbyte Direction = 1;
                public float Rotation = 0f;
                public byte FrameID = 0;
                public byte FrameTime = 0;
                public byte ShotCooldown = 0;
                public bool VoodooDemon = false;
            }
            public DemonInfo[] DemonMinions = new DemonInfo[0];
        }
    }
}
