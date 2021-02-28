using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace TerraClasses.SkillList.Merchant
{
    public class Avarice : SkillBase
    {
        private const byte ChestXVar = 0, ChestYVar = 1, CooldownVar = 2;

        public Avarice()
        {
            Name = "Avarice";
            Description = "Sometimes your character may sense nearby chests with valuable and unique items inside." +
                " \nLevel increases the range." +
                " \nThere is 30 seconds before you can find another chest nearby.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Passive;
        }

        public override void Update(Player player, SkillData data)
        {
            int ChestX = data.GetInteger(ChestXVar), ChestY = data.GetInteger(ChestYVar), Cooldown = data.GetInteger(CooldownVar);
            if (ChestX == 0)
            {
                ChestX = -1;
                ChestY = -1;
            }
            if (ChestX == -1 || ChestY == -1)
            {
                if (Cooldown > 0)
                    Cooldown--;
                else
                {
                    int PlayerX = (int)player.Center.X / 16, PlayerY = (int)player.Center.Y / 16;
                    for (int i = 0; i < data.Level; i++)
                    {
                        int CheckX = PlayerX + Main.rand.Next(-(20 + 2 * data.Level), 21 + 2 * data.Level), CheckY = PlayerY + Main.rand.Next(-(10 + data.Level), 11 + data.Level);
                        if (CheckX >= Main.leftWorld && CheckX + 1 < Main.rightWorld && CheckY >= Main.topWorld && CheckY + 1 < Main.bottomWorld && (Math.Abs(PlayerX - CheckX) >= 6 ||
                            Math.Abs(PlayerY - CheckY) >= 6))
                        {
                            Tile tile = Framing.GetTileSafely(CheckX, CheckY);
                            if (tile.active() && tile.type == Terraria.ID.TileID.Containers)
                            {
                                int ChestPos = Chest.FindChest(CheckX, CheckY);
                                bool ValidChest = ChestPos > -1 && ChestPos < Main.maxChests && Main.chest[ChestPos].item.Any(x => x.accessory || x.damage > 0 || x.value >= 10000);
                                if (tile.frameX >= 18)
                                    CheckX--;
                                if (tile.frameY % 36 >= 18)
                                    CheckY--;
                                if (ValidChest)
                                {
                                    ChestX = CheckX;
                                    ChestY = CheckY;
                                    if (player.whoAmI == Main.myPlayer)
                                    {
                                        CombatText.NewText(player.getRect(), Color.LightCyan, "Chest Nearby!", true);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                int PlayerX = (int)player.Center.X / 16, PlayerY = (int)player.Center.Y / 16;
                if (Math.Abs(PlayerX - ChestX) >= 30 || Math.Abs(PlayerY - ChestY) >= 25 || player.chest == Chest.FindChest(ChestX, ChestY))
                {
                    ChestX = ChestY = -1;
                    Cooldown = 60 * 30;
                }
                else
                {
                    Lighting.AddLight((ChestX + 1), (ChestY + 1), 0.8f, 0.8f, 0.8f);
                }
            }
            data.SetInteger(ChestXVar, ChestX);
            data.SetInteger(ChestYVar, ChestY);
            data.SetInteger(CooldownVar, Cooldown);
        }

        public override void Draw(Player player, SkillData data, Terraria.ModLoader.PlayerDrawInfo pdi)
        {
            int ChestX = data.GetInteger(ChestXVar), ChestY = data.GetInteger(ChestYVar);
            if (ChestX > -1 && ChestY > -1 && Main.rand.Next(5) == 0)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(ChestX * 16, ChestY * 16), 32, 32, Terraria.ID.DustID.Gold);
                dust.velocity.X = 0;
                dust.velocity.Y = -2;
                dust.noGravity = true;
                dust.noLight = false;
            }
        }
    }
}
