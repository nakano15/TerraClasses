using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace TerraClasses.SkillList.Arachnomancer
{
    public class SpiderAssault : SkillBase
    {
        private const int DropSpeed = 16, DropHeight = 1000;

        public SpiderAssault()
        {
            Name = "Spider Assault";
            Description = "Summon spiders to drop on nearby enemies.";
            MaxLevel = 10;
            Cooldown = GetTime(50);
            skillType = Enum.SkillTypes.Active;
            PositionToTake = PositionToTakeOnCastEnum.Mouse;
            CastTime = 45;
        }

        public override SkillData GetSkillData => new SpiderAssaultData();

        public override void Update(Player player, SkillData rawdata)
        {
            SpiderAssaultData data = (SpiderAssaultData)rawdata;
            switch (data.Step)
            {
                case 0:
                    {
                        Vector2 SkillPosition = data.CastPosition;
                        int MaxSpawns = 1 + (int)(data.Level * 0.25f);
                        TargetTranslator.Translator[] Targets = data.GetPossibleTargets(false, Position: SkillPosition, Distance: 320, MaxTargets: MaxSpawns);
                        data.SpiderPositions.Clear();
                        foreach(TargetTranslator.Translator Target in Targets)
                        {
                            SpiderAssaultData.DropPosition drop = new SpiderAssaultData.DropPosition(Target.Position + Target.Velocity * ((float)DropHeight / DropSpeed));
                            data.SpiderPositions.Add(drop);
                        }
                        data.ChangeStep();
                    }
                    break;
                case 1:
                    {
                        byte IncompleteDrops = 0;
                        int Damage = data.GetSummonDamage(0, 0.65f + 0.05f * data.Level, player);
                        foreach(SpiderAssaultData.DropPosition drop in data.SpiderPositions)
                        {
                            if (drop.DropCompleted) continue;
                            IncompleteDrops++;
                            if (drop.Dropping)
                            {
                                drop.Position.Y += DropSpeed;
                                if(drop.Position.Y > drop.DestinationY)
                                {
                                    drop.Dropping = false;
                                }
                                TargetTranslator.Translator[] Targets = data.GetPossibleTargets(false, new Rectangle((int)drop.Position.X - 28, (int)drop.Position.Y - 26, 56, 52), false, 7);
                                foreach (TargetTranslator.Translator target in Targets)
                                {
                                    data.HurtTarget(target, Damage, 0, 1f, 20);
                                }
                            }
                            else
                            {
                                drop.Position.Y -= DropSpeed * 0.5f;
                                if(drop.Position.Y < drop.DestinationY - 2000)
                                {
                                    drop.DropCompleted = true;
                                }
                            }
                        }
                        if (IncompleteDrops == 0)
                            data.EndUse();
                    }
                    break;
            }
        }

        public override void Draw(Player player, SkillData rawdata, PlayerDrawInfo pdi)
        {
            SpiderAssaultData data = (SpiderAssaultData)rawdata;
            if(data.Step == 1)
            {
                Texture2D SpiderTexture = GetNPCTexture(165);
                Vector2 Origin = new Vector2(28, 26);
                foreach (SpiderAssaultData.DropPosition d in data.SpiderPositions)
                {
                    if (d.DropCompleted) continue;
                    Vector2 Position = d.Position - Main.screenPosition;
                    Terraria.DataStructures.DrawData dd = new Terraria.DataStructures.DrawData(SpiderTexture, Position, new Rectangle(0, 0, 56, 52), Color.White, MathHelper.ToRadians(90), Origin, 1f, SpriteEffects.FlipVertically, 0);
                    Main.playerDrawData.Insert(0, dd);
                }
            }
        }

        public class SpiderAssaultData : SkillData
        {
            public List<DropPosition> SpiderPositions = new List<DropPosition>();

            public class DropPosition
            {
                public Vector2 Position;
                public float DestinationY;
                public bool Dropping = true, DropCompleted = false;

                public DropPosition(Vector2 Destination)
                {
                    DestinationY = Destination.Y;
                    Position = new Vector2(Destination.X, Destination.Y - DropHeight);
                }
            }
        }
    }
}
