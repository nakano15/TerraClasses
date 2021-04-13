using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace TerraClasses.SkillList.Thief
{
    public class Steal : SkillBase
    {
        public Steal()
        {
            Name = "Steal";
            Description = "Steals from your foe.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Active;
            Cooldown = GetTime(40);
        }

        public override void Update(Player player, SkillData data)
        {
            float Range = 140f;
            TargetTranslator.Translator[] Targets = data.GetPossibleTargets(false, false, player.Center, Range);
            if(Targets.Length > 0)
            {
                float NearestDistance = float.MaxValue;
                TargetTranslator.Translator NearestTarget = null;
                foreach(TargetTranslator.Translator target in Targets)
                {
                    float Distance = (player.Center - target.Center).Length();
                    if(Distance < NearestDistance)
                    {
                        NearestDistance = Distance;
                        NearestTarget = target;
                    }
                }
                if(NearestTarget == null)
                {
                    CombatText.NewText(player.getRect(), Microsoft.Xna.Framework.Color.Red, "No one to steal!", true);
                    data.EndUse(true);
                    return;
                }
                else
                {
                    if (NearestTarget.CharacterIdentifier.StartsWith("pl"))
                    {
                        Player otherplayer = (Player)NearestTarget.Target;

                    }
                    else
                    {
                        StealFromOther(NearestTarget, data, player);
                    }
                }
                data.EndUse(false);
            }
            else
            {
                data.EndUse(true);
                CombatText.NewText(player.getRect(), Microsoft.Xna.Framework.Color.Red, "Nobody nearby!", true);
            }
        }

        public static void StealFromOther(TargetTranslator.Translator Target, SkillData data, Player owner)
        {
            if (Main.rand.NextFloat() < 0.3f)
            {
                float Picked = Main.rand.NextFloat() * 2;
                if (Picked < 0.01f)
                {
                    CreateItemAtTarget(Target, owner, Terraria.ID.ItemID.LifeCrystal, 1);
                }
                else if (Picked < 0.05f)
                {
                    switch (Main.rand.Next(7))
                    {
                        case 0:
                            CreateItemAtTarget(Target, owner, Terraria.ID.ItemID.Amethyst, Main.rand.Next(2, 5));
                            break;
                        case 1:
                            CreateItemAtTarget(Target, owner, Terraria.ID.ItemID.Topaz, Main.rand.Next(2, 5));
                            break;
                        case 2:
                            CreateItemAtTarget(Target, owner, Terraria.ID.ItemID.Sapphire, Main.rand.Next(2, 5));
                            break;
                        case 3:
                            CreateItemAtTarget(Target, owner, Terraria.ID.ItemID.Emerald, Main.rand.Next(2, 5));
                            break;
                        case 4:
                            CreateItemAtTarget(Target, owner, Terraria.ID.ItemID.Ruby, Main.rand.Next(2, 5));
                            break;
                        case 5:
                            CreateItemAtTarget(Target, owner, Terraria.ID.ItemID.Diamond, Main.rand.Next(2, 5));
                            break;
                        case 6:
                            CreateItemAtTarget(Target, owner, Terraria.ID.ItemID.Amber, Main.rand.Next(2, 5));
                            break;
                    }
                }
                else if (Picked < 0.10f)
                {
                    CreateItemAtTarget(Target, owner, Terraria.ID.ItemID.HerbBag, 1);
                }
                else if (Picked < 0.15f)
                {
                    switch (Main.rand.Next(2))
                    {
                        case 0:
                            CreateItemAtTarget(Target, owner, Terraria.ID.ItemID.LockBox, 1);
                            break;
                        case 1:
                            CreateItemAtTarget(Target, owner, Terraria.ID.ItemID.GoldenKey, 1);
                            break;
                    }
                }
                else if (Picked < 0.25f)
                {
                    switch (Main.rand.Next(3))
                    {
                        case 0:
                            CreateItemAtTarget(Target, owner, Terraria.ID.ItemID.WoodenCrate, 1);
                            break;
                        case 1:
                            CreateItemAtTarget(Target, owner, Terraria.ID.ItemID.IronCrate, 1);
                            break;
                        case 2:
                            CreateItemAtTarget(Target, owner, Terraria.ID.ItemID.GoldenCrate, 1);
                            break;
                    }
                }
                else if (Picked < 0.35f)
                {
                    switch (Main.rand.Next(8))
                    {
                        case 0:
                            CreateItemAtTarget(Target, owner, Terraria.ID.ItemID.SilverOre, Main.rand.Next(12, 25));
                            break;
                        case 1:
                            CreateItemAtTarget(Target, owner, Terraria.ID.ItemID.GoldOre, Main.rand.Next(12, 25));
                            break;
                        case 2:
                            CreateItemAtTarget(Target, owner, Terraria.ID.ItemID.TungstenOre, Main.rand.Next(12, 25));
                            break;
                        case 3:
                            CreateItemAtTarget(Target, owner, Terraria.ID.ItemID.PlatinumOre, Main.rand.Next(12, 25));
                            break;
                        case 4:
                            CreateItemAtTarget(Target, owner, Terraria.ID.ItemID.SilverBar, Main.rand.Next(12, 25));
                            break;
                        case 5:
                            CreateItemAtTarget(Target, owner, Terraria.ID.ItemID.GoldBar, Main.rand.Next(12, 25));
                            break;
                        case 6:
                            CreateItemAtTarget(Target, owner, Terraria.ID.ItemID.TungstenBar, Main.rand.Next(12, 25));
                            break;
                        case 7:
                            CreateItemAtTarget(Target, owner, Terraria.ID.ItemID.PlatinumBar, Main.rand.Next(12, 25));
                            break;
                    }
                }
                else
                {
                    CreateItemAtTarget(Target, owner, Terraria.ID.ItemID.AngelStatue, 1);
                }
            }
            else
            {
                float Variation = (0.2f + data.Level * 0.6f) * (Main.rand.Next(80, 121) * 0.01f);
                int Copper = (int)((Target.Damage * 2 + Target.Defense * 4) * Variation), Silver = 0, Gold = 0, Platinum = 0;
                if (Copper >= 100)
                {
                    Silver += Copper / 100;
                    Copper -= Silver * 100;
                }
                if (Silver >= 100)
                {
                    Gold += Silver / 100;
                    Silver -= Gold * 100;
                }
                if (Gold >= 100)
                {
                    Platinum += Gold / 100;
                    Gold -= Platinum * 100;
                }
                if (Platinum > 0)
                {
                    CreateItemAtTarget(Target, owner, Terraria.ID.ItemID.PlatinumCoin, Platinum);
                }
                if (Gold > 0)
                {
                    CreateItemAtTarget(Target, owner, Terraria.ID.ItemID.GoldCoin, Gold);
                }
                if (Silver > 0)
                {
                    CreateItemAtTarget(Target, owner, Terraria.ID.ItemID.SilverCoin, Silver);
                }
                if (Copper > 0)
                {
                    CreateItemAtTarget(Target, owner, Terraria.ID.ItemID.CopperCoin, Copper);
                }
            }
        }

        private static void CreateItemAtTarget(TargetTranslator.Translator Target, Player owner, int ItemID, int Stack = 1)
        {
            int i = Item.NewItem(Target.GetRectangle, ItemID, Stack);
            Main.item[i].owner = owner.whoAmI;
        }
    }
}
