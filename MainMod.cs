using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace TerraClasses
{
	public class MainMod : Mod
	{
        public const int MaxClasses = 5;
        private static Dictionary<string, ModContentContainer> ModContentList = new Dictionary<string, ModContentContainer>();
        public static Texture2D LevelArrowTexture, BorderTexture, ClassButtonTexture;
        public static ModHotKey[] SkillHotKeys = new ModHotKey[4];
        public static List<KeyValuePair<int, string>> UnlockedClasses = new List<KeyValuePair<int, string>>();
        public static List<ClassUnlockLoreData> LoreDatas = new List<ClassUnlockLoreData>();
        public static int SkillChangeSlot = -1;
        public static bool DebugMode = false;
        public static Mod mod;
        public static SkillSprite Longsword, CerberusSprite, Electricity;
        public static Texture2D MagicCircle, CastBar;
        public const int ModVersion = 1;
        public static bool LoadedCustomModClasses = false;
        public const string ModClassAndSkillListCallName = "modandskillclasslist";
        public static Vector2 ExpBarOffset = Vector2.Zero;
        public static bool NExperienceLoaded = false;
        public static string UnlockedClassedSaveFileName = Main.SavePath + "/unlockedclasses.sav";
        public static string ClassLoreDataSaveFileName = Main.SavePath + "/classloredatas.sav";
        public const string ModGetTargetListCallName = "getothermodtargets";

        /// <summary>
        /// To add a kind of support to other things having classes too, maybe 
        /// I could make use of interfaces to translate what is what for the mod, then 
        /// create an interface for each entity type, like is the case of Terrarians and 
        /// TerraGuardians.
        /// </summary>
        public MainMod()
        {

        }

        public static void SaveUnlockedClasses()
        {
            if (DebugMode)
                return;
            if (File.Exists(UnlockedClassedSaveFileName))
            {
                File.Delete(UnlockedClassedSaveFileName);
            }
            using (FileStream stream = File.Create(UnlockedClassedSaveFileName))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    foreach (KeyValuePair<int, string> c in UnlockedClasses)
                    {
                        writer.Write("[" + c.Key + "]" + c.Value);
                    }
                }
            }
        }

        public static void LoadUnlockedClasses()
        {
            if (File.Exists(UnlockedClassedSaveFileName))
            {
                using (FileStream stream = new FileStream(UnlockedClassedSaveFileName, FileMode.Open))
                {
                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        UnlockedClasses.Clear();
                        while (reader.PeekChar() > -1)
                        {
                            string Code = reader.ReadString();
                            int Id = 0;
                            string ModID = "";
                            Id = int.Parse(Code.Substring(1, Code.IndexOf(']') - 1));
                            ModID = Code.Substring(Code.IndexOf(']') + 1);
                            UnlockedClasses.Add(new KeyValuePair<int, string>(Id, ModID));
                        }
                    }
                }
            }
        }

        public static void SaveLoreDatas()
        {
            if (DebugMode)
            {
                LoreDatas.Clear();
                return;
            }
            if (File.Exists(ClassLoreDataSaveFileName))
            {
                File.Delete(ClassLoreDataSaveFileName);
            }
            using (FileStream stream = File.Create(ClassLoreDataSaveFileName))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(ModVersion);
                    writer.Write(LoreDatas.Count);
                    for (int d = 0; d < LoreDatas.Count; d++)
                    {
                        writer.Write(LoreDatas[d].ClassID);
                        writer.Write(LoreDatas[d].ClassModID);
                        LoreDatas[d].Save(writer);
                    }
                }
            }
        }

        public static void LoadLoreDatas()
        {
            if (DebugMode)
            {
                LoreDatas.Clear();
                return;
            }
            if (File.Exists(ClassLoreDataSaveFileName))
            {
                using (FileStream stream = new FileStream(ClassLoreDataSaveFileName, FileMode.Open))
                {
                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        int LastModVersion = reader.ReadInt32();
                        int DataCounts = reader.ReadInt32();
                        LoreDatas.Clear();
                        for (int d = 0; d < DataCounts; d++)
                        {
                            int ClassID = reader.ReadInt32();
                            string ClassModID = reader.ReadString();
                            ClassUnlockLoreData loredata = new ClassUnlockLoreData(ClassID, ClassModID);
                            loredata.Load(reader, LastModVersion);
                            LoreDatas.Add(loredata);
                        }
                    }
                }
            }
        }

        public static Texture2D LoadTexture(string File)
        {
            return mod.GetTexture(File);
        }

        public static bool TextureExists(string File)
        {
            return mod.TextureExists(File);
        }

        public static void AddClassUnlocked(int ID, string ModID)
        {
            if (ModID == "")
                ModID = mod.Name;
            if (!UnlockedClasses.Any(x => x.Key == ID && x.Value == ModID))
            {
                UnlockedClasses.Add(new KeyValuePair<int, string>(ID, ModID));
                string ClassName = GetClass(ID, ModID).Name;
                Main.NewText("You unlocked [" + ClassName + "] class knowledge!", Color.Cyan);
                SaveUnlockedClasses();
            }
        }

        public static bool HasClassUnlocked(int ID, string ModID = "")
        {
            if (ModID == "")
                ModID = mod.Name;
            return UnlockedClasses.Any(x => x.Key == ID && x.Value == ModID);
        }

        public static void TryLoadingCustomClasses()
        {
            LoadCustomModClasses();
            LoadedCustomModClasses = true;
        }

        public static ClassUnlockLoreData GetClassLoreData(int ID, string ModID = "")
        {
            if (ModID == "")
            {
                ModID = mod.Name;
            }
            foreach (ClassUnlockLoreData ld in LoreDatas)
            {
                if (ld.ClassID == ID && ld.ClassModID == ModID)
                    return ld;
            }
            ClassUnlockLoreData nld = new ClassUnlockLoreData(ID, ModID);
            LoreDatas.Add(nld);
            return nld;
        }

        public static void DeleteClassLoreData(int ID, string ModID = "")
        {
            if (ModID == "")
            {
                ModID = mod.Name;
            }
            for (int c = 0; c < LoreDatas.Count; c++)
            {
                if (LoreDatas[c].ClassID == ID && LoreDatas[c].ClassModID == ModID)
                    LoreDatas.RemoveAt(c);
            }
        }

        public static ClassBase GetClass(int ID, string ModID = "")
        {
            if (!LoadedCustomModClasses)
            {
                TryLoadingCustomClasses();
            }
            if (ModID == "")
                ModID = mod.Name;
            ModContentContainer container;
            if (!ModContentList.ContainsKey(ModID))
            {
                container = new ModContentContainer(ModID);
                ModContentList.Add(ModID, container);
            }
            else
            {
                container = ModContentList[ModID];
            }
            return container.GetClassBase(ID);
        }

        public static SkillBase GetSkill(int ID, string ModID = "")
        {
            if (!LoadedCustomModClasses)
            {
                TryLoadingCustomClasses();
            }
            if (ModID == "")
                ModID = mod.Name;
            ModContentContainer container;
            if (!ModContentList.ContainsKey(ModID))
            {
                container = new ModContentContainer(ModID);
                ModContentList.Add(ModID, container);
            }
            else
            {
                container = ModContentList[ModID];
            }
            return container.GetSkillBase(ID);
        }

        public static void LoadCustomModClasses()
        {
            ModContentList.Clear();
            foreach (Mod mod in ModLoader.Mods)
            {
                try
                {
                    mod.Call(new string[] { ModClassAndSkillListCallName });
                }
                catch { }
            }
        }

        public static List<TargetTranslator.Translator> GetOtherModTargets(Player caster, bool Allies)
        {
            List<TargetTranslator.Translator> targets = new List<TargetTranslator.Translator>();
            foreach (Mod mod in ModLoader.Mods)
            {
                try
                {
                    object returned = mod.Call(new object[] { ModGetTargetListCallName, caster, Allies });
                    if (returned is List<TargetTranslator.Translator>)
                        targets.AddRange((List<TargetTranslator.Translator>)returned);
                }
                catch { }
            }
            return targets;
        }

        public void AddModContent(Mod mod, ModContentContainer container)
        {
            ModContentList.Add(mod.Name, container);
        }

        public override void Load()
        {
            mod = this;
            for (int s = 0; s < 4; s++)
            {
                SkillHotKeys[s] = RegisterHotKey("Skill " + (s + 1) + " Hotkey", (s == 0 ? "Q" : "F" + (s + 1)));
            }
            if (!Main.dedServ)
            {
                ClassButtonTexture = LoadTexture("Interface/ClassBtn");
                LevelArrowTexture = GetTexture("Interface/LevelArrow");
                BorderTexture = GetTexture("Interface/Border");
                //for(int i = 1; i <= 6; i++)
                //    AddClassUnlocked(i, this.Name);
                MagicCircle = LoadTexture("Content/MagicCircle");
                CastBar = LoadTexture("Content/CastBar");
                Longsword = new SkillSprite("Content/Effects/longsword");
                CerberusSprite = new SkillSprite("Content/Effects/cerberus");
                Electricity = new SkillSprite("Content/Effects/Electricity");
                if (ModLoader.GetMod("NExperience") != null)
                    NExperienceLoaded = true;
                LoadUnlockedClasses();
            }
        }

        public override void ModifyInterfaceLayers(List<Terraria.UI.GameInterfaceLayer> layers)
        {
            int InterfaceArrowPos = 0;
            for (int l = 0; l < layers.Count; l++)
            {
                if (layers[l].Name.Contains("Resource"))
                    InterfaceArrowPos = l;
            }
            Terraria.UI.LegacyGameInterfaceLayer levelinfo = new Terraria.UI.LegacyGameInterfaceLayer("NClasses: Class Level Info", DrawLevelInfo, Terraria.UI.InterfaceScaleType.UI);
            layers.Insert(InterfaceArrowPos, levelinfo);
        }

        public static bool DrawClassInfoInterface()
        {
            ClassSelectionInterface.UpdateAndDraw();
            return true;
        }

        public static void DrawTextBorder(Vector2 StartPosition, int Width, Color color)
        {
            StartPosition.X -= 6;
            StartPosition.Y -= 4;
            Width += 12;
            for (int x = 0; x < 3; x++)
            {
                int DrawX = (int)StartPosition.X, DrawY = (int)StartPosition.Y, DrawWidth = 8, DrawHeight = 24;
                if (x == 2)
                {
                    DrawX += Width - DrawWidth;
                }
                else if (x == 1)
                {
                    DrawX += DrawWidth;
                    DrawWidth = Width - 16;
                }
                Main.spriteBatch.Draw(BorderTexture, new Rectangle(DrawX, DrawY, DrawWidth, DrawHeight), new Rectangle(x * 8, 0, 8, 24), color);
            }
        }

        public static int GetClassMaxExp(int ClassRank, int ClassLevel)
        {
            int ResultExp = 30;
            int ExpBoostOne = 5, ExpBoostTwo = 3;
            switch (ClassRank)
            {
                case 1:
                    ResultExp = GetClassMaxExp(0, 10);
                    ExpBoostOne = 7;
                    ExpBoostTwo = 5;
                    break;
                case 2:
                    ResultExp = GetClassMaxExp(0, 10);
                    ClassLevel += 20;
                    ExpBoostOne = 9;
                    ExpBoostTwo = 7;
                    break;
                case 3:
                    ResultExp = GetClassMaxExp(0, 10);;
                    ClassLevel += 40;
                    ExpBoostOne = 12;
                    ExpBoostTwo = 9;
                    break;
                case 4:
                    ResultExp = GetClassMaxExp(0, 10);;
                    ClassLevel += 60;
                    ExpBoostOne = 15;
                    ExpBoostTwo = 12;
                    break;
                case 5:
                    ResultExp = GetClassMaxExp(0, 10);;
                    ClassLevel += 80;
                    ExpBoostOne = 18;
                    ExpBoostTwo = 15;
                    break;
            }
            ResultExp += ExpBoostTwo * (ClassLevel - 1) + ExpBoostOne * (ClassLevel - 1);
            return ResultExp;
        }

        public static bool IsArrow(int projtype)
        {
            return (projtype == 1 || projtype == 2 || projtype == 4 || projtype == 5 || projtype == 41 || projtype == 91 || projtype == 103 || projtype == 172 || projtype == 225 || projtype == 278 || projtype == 282 || projtype == 474 || projtype == 639);
        }

        public static bool IsBullet(int projtype)
        {
            return (projtype == 14 || projtype == 36 || projtype == 14 || projtype == 89 || projtype == 104 || projtype == 207 || projtype == 242 || projtype == 279 || projtype == 283 || projtype == 284 || projtype == 285 || projtype == 286 || projtype == 287 || projtype == 638);
        }

        public static bool IsRocket(int projtype)
        {
            return (projtype == Terraria.ID.ProjectileID.RocketI || projtype == Terraria.ID.ProjectileID.RocketII || projtype == Terraria.ID.ProjectileID.RocketIII || projtype == Terraria.ID.ProjectileID.RocketIV);
        }

        public static bool IsDart(int projtype)
        {
            return (projtype == Terraria.ID.ProjectileID.Seed || projtype == Terraria.ID.ProjectileID.PoisonDart || projtype == Terraria.ID.ProjectileID.CursedDart || projtype == Terraria.ID.ProjectileID.IchorDart || projtype == Terraria.ID.ProjectileID.CrystalDart);
        }

        public static bool IsUndead(int MobID)
        {
            switch (MobID)
            {
                case 31:
                case 294:
                case 295:
                case 296:
                case 39:
                case 34:
                case 32:
                case 161:
                case 431:
                case 52:
                case 481:
                case 21:
                case 201:
                case 202:
                case 203:
                case 322:
                case 323:
                case 324:
                case 635:
                case 254:
                case 255:
                case 45:
                case 44:
                case 167:
                case 3:
                case 132:
                case 186:
                case 187:
                case 188:
                case 189:
                case 200:
                case 319:
                case 320:
                case 321:
                case 331:
                case 332:
                case 430:
                case 432:
                case 433:
                case 434:
                case 435:
                case 436:
                case 68:
                case 77:
                case 197:
                case 78:
                case 79:
                case 80:
                case 273:
                case 274:
                case 275:
                case 276:
                case 287:
                case 285:
                case 286:
                case 524:
                case 525:
                case 526:
                case 527:
                case 288:
                case 289:
                case 277:
                case 278:
                case 279:
                case 280:
                case 283:
                case 284:
                case 290:
                case 140:
                case 281:
                case 282:
                case 172:
                case 269:
                case 270:
                case 271:
                case 272:
                case 110:
                case 293:
                case 291:
                case 292:
                case 82:
                case 536:
                case 53:
                case 223:
                case 251:
                case 162:
                case 158:
                case 159:
                case 253:
                case 166:
                case 467:
                case 305:
                case 306:
                case 307:
                case 308:
                case 309:
                case 310:
                case 311:
                case 312:
                case 313:
                case 314:
                case 315:
                case 330:
                case 329:
                case 338:
                case 339:
                case 340:
                case 35:
                case 36:
                case 325:
                case 327:
                case 566:
                case 567:
                case 453:
                    return true;
            }
            return false;
        }

        public override object Call(params object[] args)
        {
            if (args.Length == 1)
            {
                if (args[0] == "hidehud")
                {
                    ClassSelectionInterface.ShowHud = false;
                }
                else if (args[0] == "showhud")
                {
                    ClassSelectionInterface.ShowHud = true;
                }
            }
            return base.Call(args);
        }

        public static bool DrawLevelInfo()
        {
            ClassData cd = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().GetLastClass;
            Vector2 Offset = new Vector2(Main.screenWidth * ExpBarOffset.X, Main.screenHeight * ExpBarOffset.Y);
            Vector2 DrawPosition = new Vector2(48f, Main.screenHeight - 50 + 10) + Offset;
            string ClassName = cd.Name;
            bool MaxedClass = cd.Level >= cd.MaxLevel;
            if (MaxedClass)
            {
                ClassName += " Master";
            }
            Vector2 TextDimension = Utils.DrawBorderString(Main.spriteBatch, ClassName, DrawPosition, (MaxedClass ? Color.Yellow : Color.White) * Main.cursorAlpha);
            DrawPosition.Y -= 10;
            bool MouseOver = false;
            if (!MaxedClass)
            {
                DrawPosition.X = 4;
                DrawPosition.Y += 25;
                float LevelDist = Utils.DrawBorderString(Main.spriteBatch, cd.Level.ToString(), DrawPosition, Color.White * Main.cursorAlpha).X;
                DrawPosition.X += 150 + LevelDist + 4;
                Utils.DrawBorderString(Main.spriteBatch, (cd.Level + 1).ToString(), DrawPosition, Color.White * Main.cursorAlpha);
                DrawPosition.X -= 150 + 2;
                DrawPosition.Y += 5;
                Main.spriteBatch.Draw(LevelArrowTexture, DrawPosition, new Rectangle(0, 0, 150, 12), Color.White);
                DrawPosition.X += 2;
                DrawPosition.Y += 2;
                MouseOver = Main.mouseX >= DrawPosition.X && Main.mouseX < DrawPosition.X + 150 &&
                    Main.mouseY >= DrawPosition.Y && Main.mouseY < DrawPosition.Y + 12;
                if (cd.Exp > 0)
                {
                    int BarFill = (int)(144 * ((float)cd.Exp / cd.MaxExp));
                    if (BarFill > 144)
                        BarFill = 144;
                    Main.spriteBatch.Draw(LevelArrowTexture, DrawPosition, new Rectangle(152, 2, BarFill, 8), Color.Green * Main.cursorAlpha);
                }
            }
            if (MouseOver)
            {
                string Text = "Exp [" + cd.Exp + "/" + cd.MaxExp + "]";
                Vector2 TextPosition = new Vector2(Main.mouseX + 18, Main.mouseY + 18);
                if (TextPosition.Y >= Main.screenHeight - 22)
                    TextPosition.Y = Main.screenHeight - 22;
                Utils.DrawBorderString(Main.spriteBatch, Text, TextPosition, Color.White * Main.cursorAlpha);
            }
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
                    DrawTextBorder(SkillInfoStartPosition, 128 - 16, Color.Yellow);
                    if (Main.playerInventory || SkillChangeSlot > -1)
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
                DrawTextBorder(SkillInfoStartPosition, 128 - 16, Color.Red);
                if (Main.playerInventory || SkillChangeSlot > -1)
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
                    Vector2 SkillStartPosition = new Vector2(Main.screenWidth * 0.5f - (128* playerMod.Classes.Count * 0.5f) + 128 * c, Main.screenHeight - 75);
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
            SkillInfoStartPosition.X += 128;
            DrawClassInfoInterface();
            return true;
        }
	}
}