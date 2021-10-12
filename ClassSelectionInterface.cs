using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TerraClasses
{
    public class ClassSelectionInterface
    {
        public static bool ShowHud = true;
        public static bool Active = false;
        public static int SelectedClass = -1, SelectedSkill = -1;
        public static int ScrollY = 0;
        public static SkillSlot HeldSkill = null;
        public const int Width = 520, Height = 240;
        public static int CheckedClassID = -1, CheckedSkillID = -1;
        public static int ClassListPage = 0, SkillListPage = 0;
        public static string CheckedClassModID = "", CheckedSkillModID = "";
        private static bool AskIfShouldResetThis = false, StatusButtonClicked = false;

        public static void Open()
        {
            Active = true;
            SelectedClass = -1;
            SelectedSkill = -1;
            CheckedClassID = CheckedSkillID = -1;
            if (MainMod.NExperienceMod != null)
            {
                MainMod.NExperienceMod.Call(new string[] { "hidehud" });
            }
        }

        public static void Close()
        {
            Active = false;
            AskIfShouldResetThis = false;
            if (MainMod.NExperienceMod != null)
            {
                MainMod.NExperienceMod.Call(new string[] { "showhud" });
            }
        }

        public static void UpdateAndDraw()
        {
            if (!Main.playerInventory)
            {
                if (!ShowHud)
                    ShowHud = true;
                if (Active)
                {
                    Close();
                }
                return;
            }
            if (!ShowHud)
                return;
            PlayerMod playerMod = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>();
            const int SkillIconSpacing = 48;
            Vector2 SkillInterfacePos = new Vector2(Main.screenWidth * 0.5f - Width * 0.5f, Main.screenHeight - Height);
            string MouseOver = "";
            if(Active)
            {
                if (Main.mouseX >= SkillInterfacePos.X && Main.mouseX < SkillInterfacePos.X + Width &&
                    Main.mouseY >= SkillInterfacePos.Y && Main.mouseY < SkillInterfacePos.Y + Height)
                    Main.player[Main.myPlayer].mouseInterface = true;
                Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle((int)SkillInterfacePos.X - 2, (int)SkillInterfacePos.Y - 2, Width + 4, Height + 2), Color.Black);
                Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle((int)SkillInterfacePos.X, (int)SkillInterfacePos.Y, Width, Height), Color.OrangeRed);
                //Wiki page
                if (Active && SelectedClass > -1 && SelectedClass < Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().Classes.Count)
                {
                    Vector2 WikiPageButtonPosition = new Vector2(Main.screenWidth * 0.5f + Width * 0.5f - 2, Main.screenHeight - Height);
                    Color color = Color.White;
                    if (Main.mouseX >= WikiPageButtonPosition.X - 40 && Main.mouseX < WikiPageButtonPosition.X &&
                        Main.mouseY >= WikiPageButtonPosition.Y + 2 && Main.mouseY < WikiPageButtonPosition.Y + 20)
                    {
                        color = Color.Yellow;
                        if (Main.mouseLeft && Main.mouseLeftRelease)
                        {
                            string WikiLink = "";
                            if (CheckedSkillID > -1)
                            {
                                SkillBase sb = MainMod.GetSkill(CheckedSkillID, CheckedSkillModID);
                                if (sb.WikiPage != null)
                                    WikiLink = sb.WikiPage;
                                else
                                    WikiLink = sb.Name;
                            }
                            else if (CheckedClassID > -1)
                            {
                                ClassBase sb = MainMod.GetClass(CheckedClassID, CheckedClassModID);
                                if (sb.WikiPage != null)
                                    WikiLink = sb.WikiPage;
                                else
                                    WikiLink = sb.Name;
                            }
                            else if (SelectedClass > -1)
                            {
                                ClassBase c = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().Classes[SelectedClass].GetClass;
                                if (SelectedSkill > -1)
                                {
                                    SkillBase b = MainMod.GetSkill(c.SkillList[SelectedSkill].SkillID, c.SkillList[SelectedSkill].SkillMod);
                                    if (c.WikiPage != null)
                                        WikiLink = b.WikiPage;
                                    else
                                        WikiLink = b.Name;
                                }
                                else
                                {
                                    if (c.WikiPage != null)
                                        WikiLink = c.WikiPage;
                                    else
                                        WikiLink = c.Name;
                                }
                            }
                            if (WikiLink != "")
                            {
                                WikiLink = "https://nakano15-mods.fandom.com/wiki/" + WikiLink;
                                System.Diagnostics.Process.Start(WikiLink);
                            }
                        }
                    }
                    Utils.DrawBorderString(Main.spriteBatch, "Wiki", WikiPageButtonPosition, color, 0.75f, 1);
                }
                if (CheckedClassID > -1)
                {
                    SkillInterfacePos.X += 4;
                    SkillInterfacePos.Y += 4;
                    ClassBase cb = MainMod.GetClass(CheckedClassID, CheckedClassModID);
                    Utils.DrawBorderString(Main.spriteBatch, cb.Name, SkillInterfacePos, Color.White, 1.2f);
                    {
                        Vector2 StatusButtonPos = new Vector2(SkillInterfacePos.X + Width - 80, SkillInterfacePos.Y);
                        Color color = Color.White;
                        if (StatusButtonClicked)
                        {
                            color = Color.Yellow;
                        }
                        if (Main.mouseX >= StatusButtonPos.X && Main.mouseX < StatusButtonPos.X + 60 &&
                            Main.mouseY >= StatusButtonPos.Y && Main.mouseY < StatusButtonPos.Y + 22)
                        {
                            color = Color.Cyan;
                            if (Main.mouseLeft && Main.mouseLeftRelease)
                            {
                                StatusButtonClicked = !StatusButtonClicked;
                            }
                        }
                        Utils.DrawBorderString(Main.spriteBatch, "Status", StatusButtonPos, color);
                    }
                    SkillInterfacePos.Y += 25;
                    Utils.DrawBorderString(Main.spriteBatch, "Type: " + cb.ClassType.ToString(), SkillInterfacePos, Color.White, 1.2f);
                    SkillInterfacePos.Y += 25;
                    Utils.DrawBorderString(Main.spriteBatch, "Max Level: " + cb.MaxLevel, SkillInterfacePos, Color.White);
                    SkillInterfacePos.Y += 22;
                    if (StatusButtonClicked)
                    {
                        Utils.DrawBorderString(Main.spriteBatch, "Max Health Bonus: " + cb.MaxHealthBonus, SkillInterfacePos, Color.White);
                        SkillInterfacePos.Y += 22;
                        Utils.DrawBorderString(Main.spriteBatch, "Max Mana Bonus: " + cb.MaxManaBonus, SkillInterfacePos, Color.White);
                        SkillInterfacePos.Y += 22;
                        Utils.DrawBorderString(Main.spriteBatch, "Physical Damage Bonus: " + cb.PhysicalDamageBonus, SkillInterfacePos, Color.White);
                        SkillInterfacePos.Y += 22;
                        Utils.DrawBorderString(Main.spriteBatch, "Magical Damage Bonus: " + cb.MagicalDamageBonus, SkillInterfacePos, Color.White);
                        SkillInterfacePos.Y += 22;
                        Utils.DrawBorderString(Main.spriteBatch, "Defense: " + cb.DefenseBonus, SkillInterfacePos, Color.White);
                        SkillInterfacePos.Y += 22;
                    }
                    else
                    {
                        if (CheckedSkillID > -1)
                        {
                            SkillBase sb = MainMod.GetSkill(CheckedSkillID, CheckedSkillModID);
                            float XBackup = SkillInterfacePos.X;
                            SkillInterfacePos.Y += 8;
                            sb.DrawSkillIcon(SkillInterfacePos, cb: cb);
                            SkillInterfacePos.Y -= 8;
                            SkillInterfacePos.X += SkillIconSpacing;
                            Utils.DrawBorderString(Main.spriteBatch, sb.Name, SkillInterfacePos, Color.White, 1.2f);
                            SkillInterfacePos.Y += 25;
                            Utils.DrawBorderString(Main.spriteBatch, "Type: " + sb.skillType.ToString(), SkillInterfacePos, Color.White, 1);
                            SkillInterfacePos.Y += 22;
                            SkillInterfacePos.X = XBackup;
                            string[] Desc = sb.Description.Split('\n');
                            foreach (string desc in Desc)
                            {
                                Utils.DrawBorderString(Main.spriteBatch, desc, SkillInterfacePos, Color.White, 0.8f, 0, 0);
                                SkillInterfacePos.Y += 18;
                            }
                            Vector2 ButtonPosition = new Vector2(Main.screenWidth * 0.5f - Width * 0.5f + 4 + Width - 68f, Main.screenHeight - 22);
                            Color color = Color.White;
                            if (Main.mouseX >= ButtonPosition.X && Main.mouseX < ButtonPosition.X + 68 && Main.mouseY >= ButtonPosition.Y && Main.mouseY < ButtonPosition.Y + 22)
                            {
                                color = Color.Yellow;
                                if (Main.mouseLeft && Main.mouseLeftRelease)
                                {
                                    CheckedSkillID = -1;
                                    CheckedSkillModID = "";
                                    AskIfShouldResetThis = false;
                                }
                            }
                            Utils.DrawBorderString(Main.spriteBatch, "Return", ButtonPosition, color);
                        }
                        else
                        {
                            bool CanChangeClass = true;
                            if (cb.ClassType == ClassBase.ClassTypes.Aspect && Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().Classes.Any(x => x.GetClass.ClassType == ClassBase.ClassTypes.Aspect))
                            {
                                CanChangeClass = false;
                                Utils.DrawBorderString(Main.spriteBatch, "You can only have one Aspect class on your character.", SkillInterfacePos, Color.White, 0.85f);
                                SkillInterfacePos.Y += 22f;
                            }
                            string[] DescriptionConcat = cb.Description.Split('\n');
                            foreach (string s in DescriptionConcat)
                            {
                                Utils.DrawBorderString(Main.spriteBatch, s, SkillInterfacePos, Color.White, 0.8f);
                                SkillInterfacePos.Y += 18;
                            }
                            if (true)
                            {
                                int SkillRows = (int)((Main.screenHeight - SkillInterfacePos.Y - 22) / SkillIconSpacing),
                                    SkillColumns = (int)((Width - 16) / SkillIconSpacing),
                                    TotalSkills = SkillRows * SkillColumns;
                                int PageCount = cb.SkillList.Count / TotalSkills;
                                for (int y = 0; y < SkillRows; y++)
                                {
                                    for (int x = 0; x < SkillColumns; x++)
                                    {
                                        int index = x + y * SkillColumns + TotalSkills * SkillListPage;
                                        if (index >= cb.SkillList.Count)
                                            continue;
                                        Vector2 Position = new Vector2(SkillInterfacePos.X + 4 + x * SkillIconSpacing, SkillInterfacePos.Y + 6 + y * SkillIconSpacing);
                                        ClassSkillInfo csi = cb.SkillList[index];
                                        SkillBase sb = csi.GetSkillBase;
                                        sb.DrawSkillIcon(Position, cb: cb);
                                        if (Main.mouseX >= Position.X && Main.mouseX < Position.X + 32 && Main.mouseY >= Position.Y && Main.mouseY < Position.Y + 32)
                                        {
                                            MouseOver = sb.Name;
                                            if (Main.mouseLeft && Main.mouseLeftRelease)
                                            {
                                                CheckedSkillID = csi.SkillID;
                                                CheckedSkillModID = csi.SkillMod;
                                                AskIfShouldResetThis = false;
                                            }
                                        }
                                    }
                                }
                                {
                                    Vector2 PagesPosition = new Vector2(Main.screenWidth * 0.5f, Main.screenHeight - 30);
                                    Utils.DrawBorderString(Main.spriteBatch, "Page " + (SkillListPage + 1) + "/" + (PageCount + 1), PagesPosition, Color.White, anchorx: 0.5f);
                                    if (SkillListPage > 0)
                                    {
                                        Vector2 LeftPosition = new Vector2(PagesPosition.X - 32, PagesPosition.Y);
                                        Color c = Color.White;
                                        if (Main.mouseX >= LeftPosition.X - 4 && Main.mouseX < LeftPosition.X + 4 && Main.mouseY >= LeftPosition.Y && Main.mouseY < LeftPosition.Y + 22)
                                        {
                                            c = Color.Yellow;
                                            if (Main.mouseLeft && Main.mouseLeftRelease)
                                            {
                                                SkillListPage--;
                                            }
                                        }
                                        Utils.DrawBorderString(Main.spriteBatch, "<", PagesPosition, c, anchorx: 0.5f);
                                    }
                                    if (SkillListPage < PageCount - 1)
                                    {
                                        Vector2 RightPosition = new Vector2(PagesPosition.X + 32, PagesPosition.Y);
                                        Color c = Color.White;
                                        if (Main.mouseX >= RightPosition.X - 4 && Main.mouseX < RightPosition.X + 4 && Main.mouseY >= RightPosition.Y && Main.mouseY < RightPosition.Y + 22)
                                        {
                                            c = Color.Yellow;
                                            if (Main.mouseLeft && Main.mouseLeftRelease)
                                            {
                                                SkillListPage++;
                                            }
                                        }
                                        Utils.DrawBorderString(Main.spriteBatch, ">", PagesPosition, c, anchorx: 0.5f);
                                    }
                                }
                            }
                            /*else
                            {
                                const int SkillColumns = 2;
                                int SkillRows = (int)((Main.screenHeight - SkillInterfacePos.Y) / 20);
                                for (int row = 0; row < SkillRows; row++)
                                {
                                    for (int column = 0; column < SkillColumns; column++)
                                    {
                                        int SkillIndex = column + SkillColumns * (row + ScrollY);
                                        string SkillName = "Skill#" + SkillIndex;
                                        SkillBase sb = null;
                                        ClassSkillInfo csi;
                                        if (SkillIndex < cb.SkillList.Count)
                                        {
                                            csi = cb.SkillList[SkillIndex];
                                            sb = csi.GetSkillBase;
                                            SkillName = sb.Name;
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                        Vector2 SkillPosition = new Vector2(128f * column, row * 25) + SkillInterfacePos;
                                        Color c = Color.White;
                                        if (Main.mouseX >= SkillPosition.X && Main.mouseX < SkillPosition.X + 128 &&
                                            Main.mouseY >= SkillPosition.Y && Main.mouseY < SkillPosition.Y + 25)
                                        {
                                            c = Color.Cyan;
                                            if (Main.mouseLeft && Main.mouseLeftRelease)
                                            {
                                                CheckedSkillID = csi.SkillID;
                                                CheckedSkillModID = csi.SkillMod;
                                                AskIfShouldResetThis = false;
                                            }
                                        }
                                        Utils.DrawBorderString(Main.spriteBatch, SkillName, SkillPosition, c);

                                    }
                                }
                            }*/
                            //
                            Vector2 ButtonPosition = new Vector2(Main.screenWidth * 0.5f - Width * 0.5f + 4, Main.screenHeight - 22);
                            Color color = Color.White;
                            if (Main.mouseX >= ButtonPosition.X && Main.mouseX < ButtonPosition.X + 86 && Main.mouseY >= ButtonPosition.Y && Main.mouseY < ButtonPosition.Y + 22)
                            {
                                color = Color.Yellow;
                                if (CanChangeClass && Main.mouseLeft && Main.mouseLeftRelease)
                                {
                                    Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().AddClass(CheckedClassID, CheckedClassModID);
                                    Main.NewText("You changed into " + cb.Name + " class!");
                                    CheckedClassID = -1;
                                    CheckedClassModID = "";
                                    Close();
                                    AskIfShouldResetThis = false;
                                }
                            }
                            Utils.DrawBorderString(Main.spriteBatch, "Change Class", ButtonPosition, color);
                            //
                            ButtonPosition.X += Width - 68f;
                            color = Color.Red;
                            if (Main.mouseX >= ButtonPosition.X && Main.mouseX < ButtonPosition.X + 68 && Main.mouseY >= ButtonPosition.Y && Main.mouseY < ButtonPosition.Y + 22)
                            {
                                color = Color.Yellow;
                                if (Main.mouseLeft && Main.mouseLeftRelease)
                                {
                                    CheckedClassID = -1;
                                    CheckedClassModID = "";
                                    Close();
                                }
                            }
                            Utils.DrawBorderString(Main.spriteBatch, "Close", ButtonPosition, color);
                        }
                    }
                }
                else
                {
                    if (SelectedSkill > -1)
                    {
                        if (SelectedClass == -1)
                        {
                            SelectedSkill = -1;
                            return;
                        }
                        SkillInterfacePos.X += 4;
                        SkillInterfacePos.Y += 4;
                        ClassData cd = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().Classes[SelectedClass];
                        SkillData sd = cd.Skills[SelectedSkill];
                        SkillBase sb = sd.GetBase;
                        float XBackup = SkillInterfacePos.X;
                        SkillInterfacePos.Y += 8;
                        SkillInterfacePos.X += 4;
                        bool IsDragged = SelectedSkillsInterface.HeldSkill != null && playerMod.GetSkillInfo(SelectedSkillsInterface.HeldSkill) == sd;
                        sd.DrawSkillIcon(SkillInterfacePos, DrawForeground: !IsDragged, DrawBackground: !IsDragged, cb: cd.GetClass);
                        if (sd.SkillTypes != Enum.SkillTypes.Passive && sd.Level > 0 &&
                            Main.mouseX >= SkillInterfacePos.X && Main.mouseX < SkillInterfacePos.X + 32 &&
                            Main.mouseY >= SkillInterfacePos.Y && Main.mouseY < SkillInterfacePos.Y + 32)
                        {
                            Vector2 TextPosition = new Vector2(SkillInterfacePos.X + 16, SkillInterfacePos.Y - 26);
                            Utils.DrawBorderString(Main.spriteBatch, "Drag to a skill slot.", TextPosition, Color.White, 1f, 0.5f);
                            if (Main.mouseLeftRelease && Main.mouseLeft)
                            {
                                SelectedSkillsInterface.HeldSkill = new SkillSlot() { ClassPosition = SelectedClass, SkillPosition = SelectedSkill };
                            }
                        }
                        SkillInterfacePos.X -= 4;
                        SkillInterfacePos.Y -= 8;
                        SkillInterfacePos.X += SkillIconSpacing;
                        Utils.DrawBorderString(Main.spriteBatch, sb.Name, SkillInterfacePos, Color.White, 1.2f);
                        SkillInterfacePos.Y += 25;
                        string t = "";
                        if (sd.Level == 0)
                        {
                            t = "Spend Skill Point to Unlock.";
                        }
                        else if (sd.Level > sb.MaxLevel)
                            t = "Overlevel " + (sd.Level - sb.MaxLevel);
                        else if (sd.Level == sb.MaxLevel)
                            t = "Mastered";
                        else
                            t = "Level [" + sd.RealLevel + "/" + sb.MaxLevel + "]";
                        Color textColor = Color.White;
                        if (sd.Level < sb.MaxLevel && Main.mouseX >= SkillInterfacePos.X && Main.mouseX < SkillInterfacePos.X + 128 &&
                            Main.mouseY >= SkillInterfacePos.Y && Main.mouseY < SkillInterfacePos.Y + 20)
                        {
                            textColor = Color.Yellow;
                            if (Main.mouseLeft && Main.mouseLeftRelease && cd.SkillPoint > 0)
                            {
                                cd.SpendPointOnSkill(SelectedSkill);
                            }
                        }
                        Utils.DrawBorderString(Main.spriteBatch, t, SkillInterfacePos, textColor, 0.9f);
                        SkillInterfacePos.Y += 20;
                        SkillInterfacePos.X -= SkillIconSpacing;
                        Utils.DrawBorderString(Main.spriteBatch, sb.skillType.ToString(), SkillInterfacePos, Color.White, 0.9f);
                        SkillInterfacePos.Y += 26;
                        string[] DescriptionConcat = sb.Description.Split('\n');
                        foreach (string s in DescriptionConcat)
                        {
                            Utils.DrawBorderString(Main.spriteBatch, s, SkillInterfacePos, Color.White, 0.9f);
                            SkillInterfacePos.Y += 20;
                        }
                        SkillInterfacePos.Y += 26;
                        if (sd.Level == 0)
                        {
                            foreach (ClassSkillInfo csi in cd.GetClass.SkillList)
                            {
                                if (csi.SkillID == sd.ID && csi.SkillMod == sd.ModID)
                                {
                                    for (int sqreq = 0; sqreq < 3; sqreq++)
                                    {
                                        if (csi.SkillReqPosition[sqreq] == -1)
                                            continue;
                                        SkillData sdtwo = cd.Skills[csi.SkillReqPosition[sqreq]];
                                        if (sdtwo.Level < csi.SkillLevel[sqreq])
                                        {
                                            string T = "Requires " + MainMod.GetSkill(sdtwo.ID, sdtwo.ModID).Name + " Level " + csi.SkillLevel[sqreq];
                                            Utils.DrawBorderString(Main.spriteBatch, T, SkillInterfacePos, Color.Gray, 0.9f);
                                            SkillInterfacePos.Y += 21f;
                                        }
                                    }
                                }
                            }
                        }
                        Vector2 SkillPointsPosition = new Vector2(Main.screenWidth * 0.5f - Width * 0.5f, Main.screenHeight - 22);
                        Utils.DrawBorderString(Main.spriteBatch, "Skill Points: " + cd.SkillPoint, SkillPointsPosition, Color.White);
                        SkillPointsPosition.X += Width - 56;
                        textColor = Color.White;
                        if (Main.mouseX >= SkillPointsPosition.X && Main.mouseX < SkillPointsPosition.X + 56 && Main.mouseY >= SkillPointsPosition.Y && Main.mouseY < SkillPointsPosition.Y + 25)
                        {
                            textColor = Color.Red;
                            if (Main.mouseLeft && Main.mouseLeftRelease)
                            {
                                SelectedSkill = -1;
                            }
                        }
                        Utils.DrawBorderString(Main.spriteBatch, "Close", SkillPointsPosition, textColor);
                        //
                        if (sd.Level > 0)
                        {
                            SkillPointsPosition.X = Main.screenWidth * 0.5f;
                            if (AskIfShouldResetThis)
                            {
                                SkillPointsPosition.X -= 10;
                                textColor = Color.White;
                                if (Main.mouseX >= SkillPointsPosition.X - 40 && Main.mouseX < SkillPointsPosition.X && Main.mouseY >= SkillPointsPosition.Y && Main.mouseY < SkillPointsPosition.Y + 22)
                                {
                                    textColor = Color.Yellow;
                                    if (Main.mouseLeft && Main.mouseLeftRelease)
                                    {
                                        if (!Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().ResetSkill(SelectedClass, SelectedSkill))
                                        {
                                            Main.NewText("Something went wrong.");
                                        }
                                        AskIfShouldResetThis = false;
                                    }
                                }
                                Utils.DrawBorderString(Main.spriteBatch, "Yes", SkillPointsPosition, textColor, 1f, 1f);

                                SkillPointsPosition.X = Main.screenWidth * 0.5f + 10;
                                textColor = Color.White;
                                if (Main.mouseX >= SkillPointsPosition.X && Main.mouseX < SkillPointsPosition.X + 40 && Main.mouseY >= SkillPointsPosition.Y && Main.mouseY < SkillPointsPosition.Y + 22)
                                {
                                    textColor = Color.Yellow;
                                    if (Main.mouseLeft && Main.mouseLeftRelease)
                                    {
                                        AskIfShouldResetThis = false;
                                    }
                                }
                                Utils.DrawBorderString(Main.spriteBatch, "No", SkillPointsPosition, textColor, 1f, 0);
                                SkillPointsPosition.X -= 10;
                                SkillPointsPosition.Y -= 25;
                                Utils.DrawBorderString(Main.spriteBatch, "Are you sure that want to reset this skill?", SkillPointsPosition, Color.White, 1f, 0.5f);
                            }
                            else
                            {
                                textColor = Color.White;
                                if (Main.mouseX >= SkillPointsPosition.X - 30 && Main.mouseX < SkillPointsPosition.X + 30 && Main.mouseY >= SkillPointsPosition.Y && Main.mouseY < SkillPointsPosition.Y + 22)
                                {
                                    textColor = Color.Yellow;
                                    if (Main.mouseLeft && Main.mouseLeftRelease)
                                    {
                                        AskIfShouldResetThis = true;
                                    }
                                }
                                Utils.DrawBorderString(Main.spriteBatch, "Reset Skill", SkillPointsPosition, textColor, 1f, 0.5f);
                            }
                        }
                        return;
                    }
                    Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle((int)SkillInterfacePos.X + 4, (int)SkillInterfacePos.Y + 4, 128, Height - 8), Color.Goldenrod);
                    bool LastClassIsMaxed = false;
                    List<string> ClassList = new List<string>();
                    bool First = true;
                    foreach (ClassData c in Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().Classes)
                    {
                        LastClassIsMaxed = c.Level >= c.MaxLevel;
                        if (First)
                        {
                            First = false;
                            continue;
                        }
                        ClassList.Add((LastClassIsMaxed ? "*" : "") + c.Name);
                    }
                    if (LastClassIsMaxed && Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().ClassChangeUnlocked && ClassList.Count < MainMod.MaxClasses + 1)
                    {
                        ClassList.Add("New Class");
                    }
                    int MaxClasses = Height / 25;
                    if (ClassList.Count < MaxClasses)
                        MaxClasses = ClassList.Count;
                    for (int i = 0; i < MaxClasses; i++)
                    {
                        int Index = i + 1;
                        Vector2 ClassNamePosition = SkillInterfacePos + new Vector2(4, 4 + i * 25f);
                        bool IsMouseOver = Main.mouseX >= ClassNamePosition.X && Main.mouseX < ClassNamePosition.X + 96 && Main.mouseY >= ClassNamePosition.Y && Main.mouseY < ClassNamePosition.Y + 25;
                        Color color = Color.White;
                        bool IsSelected = SelectedClass == Index;
                        if (IsSelected)
                            color = Color.Yellow;
                        if (IsMouseOver)
                        {
                            color = Color.Cyan;
                            if (Main.mouseLeft && Main.mouseLeftRelease)
                            {
                                SelectedSkill = -1;
                                if (IsSelected)
                                    SelectedClass = -1;
                                else
                                    SelectedClass = Index;
                            }
                        }
                        Utils.DrawBorderString(Main.spriteBatch, ClassList[i], ClassNamePosition, color);
                    }
                    SkillInterfacePos.X += 128 + 12f;
                    if (SelectedClass > -1)
                    {
                        List<ClassData> classes = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().Classes;
                        if (SelectedClass >= classes.Count)
                        {
                            Utils.DrawBorderString(Main.spriteBatch, "Class Advancement Disponible.", SkillInterfacePos, Color.White);
                            SkillInterfacePos.Y += 25f;
                            int Rows = 2, Columns = (Height - 25) / 25;
                            for (int c = 0; c < Columns; c++)
                            {
                                for (int r = 0; r < Rows; r++)
                                {
                                    int Index = r + c * Rows;
                                    if (Index >= MainMod.UnlockedClasses.Count)
                                    {
                                        continue;
                                    }
                                    Vector2 Position = SkillInterfacePos + new Vector2(r * 128, 25 * c);
                                    Color color = Color.White;
                                    int ClassID = MainMod.UnlockedClasses[Index].Key;
                                    string ClassModID = MainMod.UnlockedClasses[Index].Value;
                                    string ClassName = MainMod.GetClass(ClassID, ClassModID).Name;//"Class#" + Index;
                                    bool RepeatedClass = classes.Any(x => x.ClassID == ClassID && x.ClassModID == ClassModID);
                                    if (RepeatedClass)
                                        color = Color.Gray;
                                    else
                                    {
                                        if (Main.mouseX >= Position.X && Main.mouseX < Position.X + 128 &&
                                            Main.mouseY >= Position.Y && Main.mouseY < Position.Y + 25)
                                        {
                                            color = Color.Yellow;
                                            if (Main.mouseLeft && Main.mouseLeftRelease)
                                            {
                                                StatusButtonClicked = false;
                                                CheckedClassID = ClassID;
                                                CheckedClassModID = ClassModID;
                                            }
                                        }
                                    }
                                    Utils.DrawBorderString(Main.spriteBatch, ClassName, Position, color);
                                }
                            }
                        }
                        else
                        {
                            ClassData c = classes[SelectedClass];
                            Utils.DrawBorderString(Main.spriteBatch, c.Name, SkillInterfacePos, Color.White);
                            SkillInterfacePos.Y += 25f;
                            {
                                Vector2 StatusButtonPos = new Vector2(SkillInterfacePos.X + Width - 80 - 128, SkillInterfacePos.Y);
                                Color color = Color.White;
                                if (StatusButtonClicked)
                                {
                                    color = Color.Yellow;
                                }
                                if (Main.mouseX >= StatusButtonPos.X && Main.mouseX < StatusButtonPos.X + 60 &&
                                    Main.mouseY >= StatusButtonPos.Y && Main.mouseY < StatusButtonPos.Y + 22)
                                {
                                    color = Color.Cyan;
                                    if (Main.mouseLeft && Main.mouseLeftRelease)
                                    {
                                        StatusButtonClicked = !StatusButtonClicked;
                                    }
                                }
                                Utils.DrawBorderString(Main.spriteBatch, "Status", StatusButtonPos, color);
                            }
                            Utils.DrawBorderString(Main.spriteBatch, (c.Level < c.MaxLevel ? "Level: " + c.Level + "  Exp [" + c.Exp + "/" + c.MaxExp + "]" : "Mastered"), SkillInterfacePos, Color.White, 0.85f);
                            SkillInterfacePos.Y += 22f;
                            if (StatusButtonClicked)
                            {
                                Utils.DrawBorderString(Main.spriteBatch, "Max Health Bonus: " + c.GetClass.MaxHealthBonus, SkillInterfacePos, Color.White);
                                SkillInterfacePos.Y += 22;
                                Utils.DrawBorderString(Main.spriteBatch, "Max Mana Bonus: " + c.GetClass.MaxManaBonus, SkillInterfacePos, Color.White);
                                SkillInterfacePos.Y += 26;
                                Utils.DrawBorderString(Main.spriteBatch, "Physical Damage Bonus: " + c.GetClass.PhysicalDamageBonus, SkillInterfacePos, Color.White);
                                SkillInterfacePos.Y += 22;
                                Utils.DrawBorderString(Main.spriteBatch, "Magical Damage Bonus: " + c.GetClass.MagicalDamageBonus, SkillInterfacePos, Color.White);
                                SkillInterfacePos.Y += 22;
                                Utils.DrawBorderString(Main.spriteBatch, "Defense Bonus: " + c.GetClass.DefenseBonus, SkillInterfacePos, Color.White);
                                SkillInterfacePos.Y += 22;
                            }
                            else if (true)
                            {
                                int SkillRows = (int)((Main.screenHeight - SkillInterfacePos.Y - 22) / SkillIconSpacing),
                                           SkillColumns = (int)((Width - 16) / SkillIconSpacing),
                                           TotalSkills = SkillRows * SkillColumns;
                                int PageCount = c.Skills.Count / TotalSkills;
                                for (int y = 0; y < SkillRows; y++)
                                {
                                    for (int x = 0; x < SkillColumns; x++)
                                    {
                                        int index = x + y * SkillColumns + TotalSkills * SkillListPage;
                                        if (index >= c.Skills.Count)
                                            continue;
                                        Vector2 Position = new Vector2(SkillInterfacePos.X + 4 + x * SkillIconSpacing, SkillInterfacePos.Y + 6 + y * SkillIconSpacing);
                                        SkillData sd = c.Skills[index];
                                        bool IsDragged = SelectedSkillsInterface.HeldSkill != null && playerMod.GetSkillInfo(SelectedSkillsInterface.HeldSkill) == sd;
                                        sd.DrawSkillIcon(Position, DrawForeground: !IsDragged, DrawBackground: !IsDragged, cb: c.GetClass);
                                        if (Main.mouseX >= Position.X && Main.mouseX < Position.X + 32 && Main.mouseY >= Position.Y && Main.mouseY < Position.Y + 32)
                                        {
                                            MouseOver = sd.Name + " Lv [" + sd.Level + "/" + sd.MaxLevel + "]";
                                            bool Dragging = false;
                                            if (sd.SkillTypes != Enum.SkillTypes.Passive && sd.Level > 0)
                                            {
                                                Vector2 TextPosition = new Vector2(Position.X, Position.Y - 26);
                                                Utils.DrawBorderString(Main.spriteBatch, "Drag to a skill slot.", TextPosition, Color.White, 1f);
                                                if (Main.mouseLeftRelease && Main.mouseLeft)
                                                {
                                                    SelectedSkillsInterface.HeldSkill = new SkillSlot() { ClassPosition = SelectedClass, SkillPosition = index };
                                                    Dragging = true;
                                                }
                                            }
                                            if (!Dragging && !Main.mouseLeft && !Main.mouseLeftRelease)
                                            {
                                                SelectedSkill = index;
                                            }
                                        }
                                    }
                                }
                                {
                                    Vector2 PagesPosition = new Vector2(Main.screenWidth * 0.5f, Main.screenHeight - 48);
                                    Utils.DrawBorderString(Main.spriteBatch, "Page " + (SkillListPage + 1) + "/" + (PageCount + 1), PagesPosition, Color.White, anchorx: 0.5f);
                                    if (SkillListPage > 0)
                                    {
                                        Vector2 LeftPosition = new Vector2(PagesPosition.X - 32, PagesPosition.Y);
                                        Color color = Color.White;
                                        if (Main.mouseX >= LeftPosition.X - 4 && Main.mouseX < LeftPosition.X + 4 && Main.mouseY >= LeftPosition.Y && Main.mouseY < LeftPosition.Y + 22)
                                        {
                                            color = Color.Yellow;
                                            if (Main.mouseLeft && Main.mouseLeftRelease)
                                            {
                                                SkillListPage--;
                                            }
                                        }
                                        Utils.DrawBorderString(Main.spriteBatch, "<", PagesPosition, color, anchorx: 0.5f);
                                    }
                                    if (SkillListPage < PageCount - 1)
                                    {
                                        Vector2 RightPosition = new Vector2(PagesPosition.X + 32, PagesPosition.Y);
                                        Color color = Color.White;
                                        if (Main.mouseX >= RightPosition.X - 4 && Main.mouseX < RightPosition.X + 4 && Main.mouseY >= RightPosition.Y && Main.mouseY < RightPosition.Y + 22)
                                        {
                                            color = Color.Yellow;
                                            if (Main.mouseLeft && Main.mouseLeftRelease)
                                            {
                                                SkillListPage++;
                                            }
                                        }
                                        Utils.DrawBorderString(Main.spriteBatch, ">", PagesPosition, color, anchorx: 0.5f);
                                    }
                                }
                            }
                            /*else if (false)
                            {
                                const int SkillColumns = 2;
                                int SkillRows = (int)((Main.screenHeight - SkillInterfacePos.Y) / 25 - (AskIfShouldResetThis ? 2 : 1));
                                for (int row = 0; row < SkillRows; row++)
                                {
                                    for (int column = 0; column < SkillColumns; column++)
                                    {
                                        int SkillIndex = column + SkillColumns * (row + ScrollY);
                                        string SkillName = "Skill#" + SkillIndex;
                                        if (SkillIndex < c.Skills.Count)
                                        {
                                            SkillName = c.Skills[SkillIndex].GetBase.Name;
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                        Vector2 SkillPosition = new Vector2(128f * column, row * 25) + SkillInterfacePos;
                                        Color color = Color.White;
                                        if (Main.mouseX >= SkillPosition.X && Main.mouseX < SkillPosition.X + 128 &&
                                            Main.mouseY >= SkillPosition.Y && Main.mouseY < SkillPosition.Y + 25)
                                        {
                                            color = Color.Cyan;
                                            if (Main.mouseLeft && Main.mouseLeftRelease)
                                            {
                                                SelectedSkill = SkillIndex;
                                            }
                                        }
                                        Utils.DrawBorderString(Main.spriteBatch, SkillName, SkillPosition, color);
                                    }
                                }
                            }*/
                            //
                            Vector2 SkillResetPos = new Vector2(Main.screenWidth * 0.5f, Main.screenHeight - 22);
                            Color textColor;
                            if (AskIfShouldResetThis)
                            {
                                SkillResetPos.X -= 10;
                                textColor = Color.White;
                                if (Main.mouseX >= SkillResetPos.X - 40 && Main.mouseX < SkillResetPos.X && Main.mouseY >= SkillResetPos.Y && Main.mouseY < SkillResetPos.Y + 22)
                                {
                                    textColor = Color.Red;
                                    if (Main.mouseLeft && Main.mouseLeftRelease)
                                    {
                                        if (!Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().DeleteClass(SelectedClass))
                                        {
                                            Main.NewText("Something went wrong.");
                                        }
                                        else
                                        {
                                            SelectedClass = SelectedSkill = -1;
                                        }
                                        AskIfShouldResetThis = false;
                                    }
                                }
                                Utils.DrawBorderString(Main.spriteBatch, "Yes", SkillResetPos, textColor, 1f, 1f);

                                SkillResetPos.X = Main.screenWidth * 0.5f + 10;
                                textColor = Color.White;
                                if (Main.mouseX >= SkillResetPos.X && Main.mouseX < SkillResetPos.X + 40 && Main.mouseY >= SkillResetPos.Y && Main.mouseY < SkillResetPos.Y + 22)
                                {
                                    textColor = Color.Yellow;
                                    if (Main.mouseLeft && Main.mouseLeftRelease)
                                    {
                                        AskIfShouldResetThis = false;
                                    }
                                }
                                Utils.DrawBorderString(Main.spriteBatch, "No", SkillResetPos, textColor, 1f, 0);
                                SkillResetPos.X -= 10;
                                SkillResetPos.Y -= 25;
                                Utils.DrawBorderString(Main.spriteBatch, "Do you really want to delete this class?", SkillResetPos, Color.White, 1f, 0.5f);
                            }
                            else
                            {
                                textColor = Color.White;
                                if (Main.mouseX >= SkillResetPos.X - 30 && Main.mouseX < SkillResetPos.X + 30 && Main.mouseY >= SkillResetPos.Y && Main.mouseY < SkillResetPos.Y + 22)
                                {
                                    textColor = Color.Red;
                                    if (Main.mouseLeft && Main.mouseLeftRelease)
                                    {
                                        AskIfShouldResetThis = true;
                                    }
                                }
                                Utils.DrawBorderString(Main.spriteBatch, "Delete Class", SkillResetPos, textColor, 1f, 0.5f);
                            }
                        }
                    }
                    else
                    {
                        SkillInterfacePos.X += (156 * 0.5f);
                        SkillInterfacePos.Y += (128 * 0.5f);
                        Utils.DrawBorderString(Main.spriteBatch, "No class selected.", SkillInterfacePos, Color.White, 1, 0.5f, 0.5f);
                    }
                }
            }
            {
                Vector2 ButtonPosition = new Vector2(Main.screenWidth * 0.5f - Width * 0.5f - 2, Main.screenHeight);
                if (Active)
                    ButtonPosition.Y -= Height + 32;
                else
                    ButtonPosition.Y -= 32;
                if (Main.mouseX >= ButtonPosition.X && Main.mouseX < ButtonPosition.X + 96 &&
                    Main.mouseY >= ButtonPosition.Y && Main.mouseY < ButtonPosition.Y + 32)
                {
                    Main.player[Main.myPlayer].mouseInterface = true;
                    if (Main.mouseLeft && Main.mouseLeftRelease)
                    {
                        if (Active)
                            Close();
                        else
                            Open();
                    }
                }
                Main.spriteBatch.Draw(MainMod.ClassButtonTexture, ButtonPosition, Color.White);
            }
            if(MouseOver != "")
            {
                Utils.DrawBorderString(Main.spriteBatch, MouseOver, new Vector2(Main.mouseX + 16, Main.mouseY + 16), Color.White);
            }
        }
    }
}
