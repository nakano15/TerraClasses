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
        public static int CheckedClassID = -1;
        public static string CheckedClassModID = "";

        public static void Open()
        {
            Active = true;
            SelectedClass = -1;
            SelectedSkill = -1;
            Mod NExperienceMod = ModLoader.GetMod("NExperience");
            if (NExperienceMod != null)
            {
                NExperienceMod.Call(new string[] { "hidehud" });
            }
        }

        public static void Close()
        {
            Active = false;
            Mod NExperienceMod = ModLoader.GetMod("NExperience");
            if (NExperienceMod != null)
            {
                NExperienceMod.Call(new string[] { "showhud" });
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
            Vector2 SkillInterfacePos = new Vector2(Main.screenWidth * 0.5f - Width * 0.5f, Main.screenHeight - Height);
            if(Active)
            {
                if (Main.mouseX >= SkillInterfacePos.X && Main.mouseX < SkillInterfacePos.X + Width &&
                    Main.mouseY >= SkillInterfacePos.Y && Main.mouseY < SkillInterfacePos.Y + Height)
                    Main.player[Main.myPlayer].mouseInterface = true;
                Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle((int)SkillInterfacePos.X - 2, (int)SkillInterfacePos.Y - 2, Width + 4, Height + 2), Color.Black);
                Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle((int)SkillInterfacePos.X, (int)SkillInterfacePos.Y, Width, Height), Color.OrangeRed);
                if (CheckedClassID > -1)
                {
                    SkillInterfacePos.X += 4;
                    SkillInterfacePos.Y += 4;
                    ClassBase cb = MainMod.GetClass(CheckedClassID, CheckedClassModID);
                    Utils.DrawBorderString(Main.spriteBatch, cb.Name, SkillInterfacePos, Color.White, 1.2f);
                    SkillInterfacePos.Y += 25;
                    Utils.DrawBorderString(Main.spriteBatch, "Type: " + cb.ClassType.ToString(), SkillInterfacePos, Color.White, 1.2f);
                    SkillInterfacePos.Y += 25;
                    Utils.DrawBorderString(Main.spriteBatch, "Max Level: " + cb.MaxLevel, SkillInterfacePos, Color.White);
                    SkillInterfacePos.Y += 22;
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
                        Utils.DrawBorderString(Main.spriteBatch, s, SkillInterfacePos, Color.White, 0.9f);
                        SkillInterfacePos.Y += 20;
                    }
                    Vector2 ButtonPosition = new Vector2(Main.screenWidth * 0.5f - Width * 0.5f + 4, Main.screenHeight - 22);
                    Color color = Color.White;
                    if (Main.mouseX >= ButtonPosition.X && Main.mouseX < ButtonPosition.X + 86 && Main.mouseY >= ButtonPosition.Y && Main.mouseY < ButtonPosition.Y + 22)
                    {
                        color = Color.Yellow;
                        if (CanChangeClass && Main.mouseLeft && Main.mouseLeftRelease)
                        {
                            /*if (cb.ClassType == ClassBase.ClassTypes.Aspect)
                            {
                                Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().Classes[0].ChangeClass(CheckedClassID, CheckedClassModID);
                            }
                            else
                            {*/
                                Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().AddClass(CheckedClassID, CheckedClassModID);
                            //}
                            Main.NewText("You changed into " + cb.Name + " class!");
                            CheckedClassID = -1;
                            CheckedClassModID = "";
                            Active = false;
                        }
                    }
                    Utils.DrawBorderString(Main.spriteBatch, "Change Class", ButtonPosition, color);
                    color = Color.Red;
                    ButtonPosition.X += Width - 68f;
                    if (Main.mouseX >= ButtonPosition.X && Main.mouseX < ButtonPosition.X + 68 && Main.mouseY >= ButtonPosition.Y && Main.mouseY < ButtonPosition.Y + 22)
                    {
                        color = Color.Yellow;
                        if (Main.mouseLeft && Main.mouseLeftRelease)
                        {
                            CheckedClassID = -1;
                            CheckedClassModID = "";
                        }
                    }
                    Utils.DrawBorderString(Main.spriteBatch, "Close", ButtonPosition, color);
                    return;
                }
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
                    Utils.DrawBorderString(Main.spriteBatch, sb.Name, SkillInterfacePos, Color.White, 1.2f);
                    SkillInterfacePos.Y += 25;
                    string t = "";
                    if (sd.Level == 0)
                    {
                        t = "Spend Skill Point to Unlock.";
                    }
                    else if (sd.Level > sb.MaxLevel)
                        t = "Overlevel " + (sb.MaxLevel - sd.Level);
                    else if (sd.Level == sb.MaxLevel)
                        t = "Mastered";
                    else
                        t = "Level [" + sd.Level + "/" + sb.MaxLevel + "]";
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
                    return;
                }
                Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle((int)SkillInterfacePos.X + 4, (int)SkillInterfacePos.Y + 4, 128, Height - 8), Color.Goldenrod);
                bool LastClassIsMaxed = false;
                List<string> ClassList = new List<string>();
                foreach (ClassData c in Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().Classes)
                {
                    LastClassIsMaxed = c.Level >= c.MaxLevel;
                    ClassList.Add((LastClassIsMaxed ? "*" : "") + c.Name);
                }
                if (LastClassIsMaxed && Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().ClassChangeUnlocked && ClassList.Count < MainMod.MaxClasses)
                {
                    ClassList.Add("New Class");
                }
                int MaxClasses = Height / 25;
                if (ClassList.Count < MaxClasses)
                    MaxClasses = ClassList.Count;
                for (int i = 0; i < MaxClasses; i++)
                {
                    int Index = i;
                    Vector2 ClassNamePosition = SkillInterfacePos + new Vector2(4, 4 + i * 25f);
                    bool MouseOver = Main.mouseX >= ClassNamePosition.X && Main.mouseX < ClassNamePosition.X + 96 && Main.mouseY >= ClassNamePosition.Y && Main.mouseY < ClassNamePosition.Y + 25;
                    Color color = Color.White;
                    bool IsSelected = SelectedClass == i;
                    if (IsSelected)
                        color = Color.Yellow;
                    if (MouseOver)
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
                                        //Clicksch for classch infosch
                                        if (Main.mouseLeft && Main.mouseLeftRelease)
                                        {
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
                        Utils.DrawBorderString(Main.spriteBatch, (c.Level < c.MaxLevel ? "Level: " + c.Level + "  Exp [" + c.Exp + "/" + c.MaxExp + "]" : "Mastered"), SkillInterfacePos, Color.White, 0.85f);
                        SkillInterfacePos.Y += 22f;
                        const int SkillColumns = 2;
                        int SkillRows = (int)((Main.screenHeight - SkillInterfacePos.Y) / 25);
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
                    }

                }
                else
                {
                    SkillInterfacePos.X += (156 * 0.5f);
                    SkillInterfacePos.Y += (128 * 0.5f);
                    Utils.DrawBorderString(Main.spriteBatch, "No class selected.", SkillInterfacePos, Color.White, 1, 0.5f, 0.5f);
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
        }
    }
}
