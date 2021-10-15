using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace TerraClasses
{
    class SelectedSkillsInterface
    {
        public static int SkillChangeSlot = -1;
        public static SkillSlot HeldSkill = null;

        public static void NewDraw()
        {
            try
            {
                Vector2 DrawPositionStart = new Vector2(Main.screenWidth * 0.5f, Main.screenHeight);
                if (ClassSelectionInterface.Active)
                    DrawPositionStart.Y -= ClassSelectionInterface.Height;
                PlayerMod playerMod = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>();
                if (!ClassSelectionInterface.ShowHud)
                {
                    return;
                }
                const int IconsDimension = 32 + 12;
                if (playerMod.CombatSkillsUnlocked)
                {
                    Vector2 SkillIconPosition = new Vector2(DrawPositionStart.X - IconsDimension * 0.5f + 6, DrawPositionStart.Y + 6 - IconsDimension * 2);
                    SkillData sd = playerMod.GetSkillInfo(playerMod.CombatSkill);
                    if (sd == null)
                    {
                        Main.spriteBatch.Draw(MainMod.SkillSlotTexture, SkillIconPosition, Color.White);
                        MainMod.DrawSkillIconBorder(SkillIconPosition, Color.Red);
                    }
                    else
                    {
                        sd.DrawSkillIcon(SkillIconPosition, cb: playerMod.Classes[playerMod.CombatSkill.ClassPosition].GetClass);
                    }
                    if (Main.mouseX >= SkillIconPosition.X && Main.mouseX < SkillIconPosition.X + 32 &&
                        Main.mouseY >= SkillIconPosition.Y && Main.mouseY < SkillIconPosition.Y + 32)
                    {
                        if (HeldSkill != null)
                        {
                            SkillData HeldSkillData = playerMod.GetSkillInfo(HeldSkill);
                            if (HeldSkillData.SkillTypes == Enum.SkillTypes.Attack && !Main.mouseLeft)
                            {
                                playerMod.CombatSkill = HeldSkill;
                            }
                        }
                        if (ClassSelectionInterface.Active && Main.mouseRight && Main.mouseRightRelease)
                            playerMod.CombatSkill = new SkillSlot() { ClassPosition = -1 };
                        Vector2 TextPosition = new Vector2(SkillIconPosition.X + 16, SkillIconPosition.Y - 26);
                        string Text = "None";
                        if (sd != null)
                            Text = sd.Name;
                        Utils.DrawBorderString(Main.spriteBatch, Text, TextPosition, Color.White, anchorx: 0.5f);
                    }
                }
                if (playerMod.ActiveSkillsUnlocked)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Vector2 SkillIconPosition = new Vector2(DrawPositionStart.X - IconsDimension * 2 + 6 + IconsDimension * i, DrawPositionStart.Y + 6 - IconsDimension);
                        SkillData sd = playerMod.GetSkillInfo(playerMod.ActiveSkill[i]);
                        if (sd == null)
                        {
                            Main.spriteBatch.Draw(MainMod.SkillSlotTexture, SkillIconPosition, Color.White);
                            MainMod.DrawSkillIconBorder(SkillIconPosition, Color.Green);
                        }
                        else
                        {
                            sd.DrawSkillIcon(SkillIconPosition, cb: playerMod.Classes[playerMod.ActiveSkill[i].ClassPosition].GetClass);
                        }
                        if (Main.mouseX >= SkillIconPosition.X && Main.mouseX < SkillIconPosition.X + 32 &&
                            Main.mouseY >= SkillIconPosition.Y && Main.mouseY < SkillIconPosition.Y + 32)
                        {
                            Vector2 TextPosition = new Vector2(SkillIconPosition.X + 16, SkillIconPosition.Y - 26);
                            string Text = "None";
                            if (sd != null)
                                Text = sd.Name;
                            if (HeldSkill != null)
                            {
                                SkillData HeldSkillData = playerMod.GetSkillInfo(HeldSkill);
                                if (HeldSkillData.SkillTypes == Enum.SkillTypes.Active && !Main.mouseLeft)
                                {
                                    playerMod.ActiveSkill[i] = HeldSkill;
                                }
                            }
                            if (ClassSelectionInterface.Active && Main.mouseRight && Main.mouseRightRelease)
                                playerMod.ActiveSkill[i] = new SkillSlot() { ClassPosition = -1 };
                            Utils.DrawBorderString(Main.spriteBatch, Text, TextPosition, Color.White, anchorx: 0.5f);
                        }
                    }
                }
                if (HeldSkill != null)
                {
                    if (!Main.mouseLeft)
                        HeldSkill = null;
                    else
                        playerMod.GetSkillInfo(HeldSkill).DrawSkillIcon(new Vector2(Main.mouseX, Main.mouseY), 0.5f, 0.5f, true, true, false, false, playerMod.Classes[HeldSkill.ClassPosition].GetClass);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*public static void Draw()
        {
            Vector2 SkillInfoStartPosition = new Vector2(Main.screenWidth * 0.5f - 128 * 2, Main.screenHeight - 22);
            if (ClassSelectionInterface.Active)
            {
                SkillInfoStartPosition.Y -= ClassSelectionInterface.Height;
            }
            if (Main.playerInventory)
            {
                SkillInfoStartPosition.Y -= 32;
                if (!ClassSelectionInterface.Active)
                {
                    SkillInfoStartPosition.X -= Main.screenWidth * 0.2f;
                }
            }
            SkillInfoStartPosition.X += 128;
            PlayerMod playerMod = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>();
            if (playerMod.ActiveSkillsUnlocked && ClassSelectionInterface.ShowHud)
            {
                for (int s = 0; s < 4; s++)
                {
                    Color c = Color.White;
                    Color barColor = Color.Black;
                    SkillData sd = playerMod.GetSkillInfo(playerMod.ActiveSkill[s]);
                    float Charge = 1f;
                    if (sd != null)
                    {
                        if (sd.Active)
                        {
                            c = Color.Gray;
                            Charge = 1f;
                        }
                        else if (sd.Cooldown > 0)
                        {
                            c = Color.Gray;
                            Charge = 1f - (float)sd.Cooldown / sd.GetBase.Cooldown;
                            barColor = Color.Red;
                        }
                        else
                        {
                            barColor = Color.Green;
                        }
                    }
                    Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle((int)SkillInfoStartPosition.X - 4, (int)SkillInfoStartPosition.Y, (int)(120 * Charge), 18), barColor);
                    MainMod.DrawTextBorder(SkillInfoStartPosition, 128 - 16, Color.Yellow);
                    if ((Main.playerInventory && ClassSelectionInterface.Active) || SkillChangeSlot > -1)
                    {
                        if (Main.mouseX >= SkillInfoStartPosition.X && Main.mouseX < SkillInfoStartPosition.X + 128 &&
                            Main.mouseY >= SkillInfoStartPosition.Y && Main.mouseY < SkillInfoStartPosition.Y + 24)
                        {
                            playerMod.player.mouseInterface = true;
                            c = Color.Yellow;
                            if (Main.mouseLeft && Main.mouseLeftRelease)
                            {
                                if (SkillChangeSlot == s)
                                {
                                    SkillChangeSlot = -1;
                                }
                                else
                                {
                                    SkillChangeSlot = s;
                                    Main.playerInventory = false;
                                }
                            }
                        }
                    }
                    string Text = "Active#" + s;
                    if (playerMod.ActiveSkill[s].ClassPosition > -1)
                        Text = playerMod.Classes[playerMod.ActiveSkill[s].ClassPosition].Skills[playerMod.ActiveSkill[s].SkillPosition].Name;
                    Utils.DrawBorderString(Main.spriteBatch, Text, SkillInfoStartPosition, c);
                    SkillInfoStartPosition.X += 128;
                }
                SkillInfoStartPosition.Y -= 24;
            }
            SkillInfoStartPosition.X = Main.screenWidth * 0.5f - 128 * 0.5f;
            if (Main.playerInventory)
            {
                if (!ClassSelectionInterface.Active)
                {
                    SkillInfoStartPosition.X -= Main.screenWidth * 0.20f;
                }
            }
            if (playerMod.CombatSkillsUnlocked)
            {
                Color c = Color.White;
                SkillData sd = playerMod.GetSkillInfo(playerMod.CombatSkill);
                float Charge = 1f;
                Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle((int)SkillInfoStartPosition.X - 4, (int)SkillInfoStartPosition.Y, 120, 18), Color.Black);
                MainMod.DrawTextBorder(SkillInfoStartPosition, 128 - 16, Color.Red);
                if ((Main.playerInventory && ClassSelectionInterface.Active) || SkillChangeSlot > -1)
                {
                    if (Main.mouseX >= SkillInfoStartPosition.X && Main.mouseX < SkillInfoStartPosition.X + 128 &&
                        Main.mouseY >= SkillInfoStartPosition.Y && Main.mouseY < SkillInfoStartPosition.Y + 24)
                    {
                        c = Color.Yellow;
                        playerMod.player.mouseInterface = true;
                        if (Main.mouseLeft && Main.mouseLeftRelease)
                        {
                            if (SkillChangeSlot == 4)
                            {
                                SkillChangeSlot = -1;
                            }
                            else
                            {
                                SkillChangeSlot = 4;
                                Main.playerInventory = false;
                            }
                        }
                    }
                }
                Utils.DrawBorderString(Main.spriteBatch, (playerMod.CombatSkill.ClassPosition > -1 ? playerMod.Classes[playerMod.CombatSkill.ClassPosition].Skills[playerMod.CombatSkill.SkillPosition].Name : "Combat Skill"), SkillInfoStartPosition, c);
            }
            if (SkillChangeSlot > -1)
            {
                Enum.SkillTypes Types = Enum.SkillTypes.Active;
                if (SkillChangeSlot == 4)
                    Types = Enum.SkillTypes.Attack;
                List<KeyValuePair<int, int>> AbleSkills = new List<KeyValuePair<int, int>>();
                for (int c = 0; c < playerMod.Classes.Count; c++)
                {
                    Vector2 SkillStartPosition = new Vector2(Main.screenWidth * 0.5f - (128 * playerMod.Classes.Count * 0.5f) + 128 * c, Main.screenHeight - 75);
                    for (int s = 0; s < playerMod.Classes[c].Skills.Count; s++)
                    {
                        SkillData sd = playerMod.Classes[c].Skills[s];
                        if (sd.SkillTypes == Types && sd.Level > 0)
                        {
                            Color color = Color.White;
                            if (Main.mouseX >= SkillStartPosition.X && Main.mouseX < SkillStartPosition.X + 128 &&
                                Main.mouseY >= SkillStartPosition.Y && Main.mouseY < SkillStartPosition.Y + 25)
                            {
                                color = Color.Yellow;
                                playerMod.player.mouseInterface = true;
                                if (Main.mouseLeft && Main.mouseLeftRelease)
                                {
                                    if (SkillChangeSlot == 4)
                                    {
                                        playerMod.CombatSkill.ClassPosition = c;
                                        playerMod.CombatSkill.SkillPosition = s;
                                    }
                                    else
                                    {
                                        playerMod.ActiveSkill[SkillChangeSlot].ClassPosition = c;
                                        playerMod.ActiveSkill[SkillChangeSlot].SkillPosition = s;
                                    }
                                    SkillChangeSlot = -1;
                                }
                            }
                            MainMod.DrawTextBorder(SkillStartPosition, 128 - 16, Color.White);
                            Utils.DrawBorderString(Main.spriteBatch, sd.Name, SkillStartPosition, color);
                            SkillStartPosition.Y -= 25;
                        }
                    }
                    SkillStartPosition.Y -= 3;
                    Utils.DrawBorderString(Main.spriteBatch, playerMod.Classes[c].Name, SkillStartPosition, Color.White, 1.2f);
                }
            }
        }*/
    }
}
