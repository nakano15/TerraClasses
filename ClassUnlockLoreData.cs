﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;

namespace TerraClasses
{
    public class ClassUnlockLoreData //Where will this be stored?
    {
        public int ClassID = 0;
        public string ClassModID = "";
        public List<ClassLoreStepProgress> LoreInternalProgress = new List<ClassLoreStepProgress>();
        public ClassUnlockLoreBase GetBase { get { return MainMod.GetClass(ClassID, ClassModID).GetLoreBase; } }
        public bool IsComplete = false;
        public int LastLoreStep = -1;
        public bool LoreAlreadyUnlocked { get { return MainMod.HasClassUnlocked(ClassID, ClassModID); } }

        public ClassUnlockLoreData(int ClassID, string ClassModID)
        {
            this.ClassID = ClassID;
            this.ClassModID = ClassModID;
            int Objectives = GetBase.Objectives.Count;
            while (LoreInternalProgress.Count < Objectives)
            {
                LoreInternalProgress.Add(new ClassLoreStepProgress());
            }
        }

        public void Update(Player player)
        {
            if (LoreAlreadyUnlocked)
                return;
            if (MainMod.DebugMode)
            {
                IsComplete = true;
                return;
            }
            ClassUnlockLoreBase ClassBase = GetBase;
            IsComplete = true;
            int HighestCompletedObjective = 0;
            for (int i = 0; i < ClassBase.Objectives.Count; i++)
            {
                if (!LoreInternalProgress[i].Complete)
                {
                    switch (ClassBase.Objectives[i].objectivetype)
                    {
                        case ClassUnlockLoreBase.ObjectiveType.Hunt:
                            {
                                ClassUnlockLoreBase.HuntMonsterObjective hmo = (ClassUnlockLoreBase.HuntMonsterObjective)ClassBase.Objectives[i];
                                if (LoreInternalProgress[i].StepValue >= hmo.Stack)
                                    LoreInternalProgress[i].Complete = true;
                            }
                            break;
                        case ClassUnlockLoreBase.ObjectiveType.GatherItem:
                            {
                                ClassUnlockLoreBase.GatherItemObjective gio = (ClassUnlockLoreBase.GatherItemObjective)ClassBase.Objectives[i];
                                if (LoreInternalProgress[i].StepValue >= gio.Stack)
                                    LoreInternalProgress[i].Complete = true;
                            }
                            break;
                        case ClassUnlockLoreBase.ObjectiveType.TalkToNpc:
                            {
                                ClassUnlockLoreBase.TalkToNpcObjective tno = (ClassUnlockLoreBase.TalkToNpcObjective)ClassBase.Objectives[i];
                                if (LoreInternalProgress[i].StepValue >= 1)
                                    LoreInternalProgress[i].Complete = true;
                                else
                                {
                                    if(player.talkNPC > -1 && Main.npc[player.talkNPC].type == tno.NpcID)
                                    {
                                        LoreInternalProgress[i].StepValue = 1;
                                        Main.npcChatText = tno.MessageText;
                                    }
                                }
                            }
                            break;
                    }
                    if (!LoreInternalProgress[i].Complete)
                    {
                        IsComplete = false;
                        HighestCompletedObjective = i;
                        break;
                    }
                }
            }
            if (HighestCompletedObjective > LastLoreStep)
            {
                if (LastLoreStep > -1)
                {
                    if (LastLoreStep == ClassBase.Objectives.Count - 1)
                    {
                        Main.NewText("You unveiled [" + MainMod.GetClass(ClassID, ClassModID).Name + "] class knowledge.", Microsoft.Xna.Framework.Color.Green);
                    }
                    else
                    {
                        Main.NewText("Part of [" + MainMod.GetClass(ClassID, ClassModID).Name + "] class knowledge unlocked.", Microsoft.Xna.Framework.Color.Orange);
                    }
                }
                LastLoreStep = HighestCompletedObjective;
            }
        }

        public void Save(Terraria.ModLoader.IO.TagCompound tag)
        {
            tag.Add("ModVersion", MainMod.ModVersion);
            tag.Add("ObjectivesProgressCount", LoreInternalProgress.Count);
            for (int i = 0; i < LoreInternalProgress.Count; i++)
            {
                ClassLoreStepProgress clsp = LoreInternalProgress[i];
                tag.Add("ObjectivesProgressStep" + i, LoreInternalProgress[i].StepValue);
            }
        }

        public void Load(Terraria.ModLoader.IO.TagCompound tag)
        {
            int ModVersion = tag.GetInt("ModVersion");
            int LoreCount = tag.GetInt("ObjectivesProgressCount");
            for (int i = 0; i < LoreCount; i++)
            {
                float Progress = tag.GetFloat("ObjectivesProgressStep" + i);
                ClassLoreStepProgress clsp = new ClassLoreStepProgress();
                clsp.StepValue = Progress;
                if (i < LoreInternalProgress.Count)
                {
                    LoreInternalProgress[i] = clsp;
                }
                else
                {
                    LoreInternalProgress.Add(clsp);
                }
            }
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(LoreInternalProgress.Count);
            for (int i = 0; i < LoreInternalProgress.Count; i++)
            {
                ClassLoreStepProgress clsp = LoreInternalProgress[i];
                writer.Write(LoreInternalProgress[i].StepValue);
            }
        }

        public void Load(BinaryReader reader, int ModVersion)
        {
            int LoreCount = reader.ReadInt32();
            for (int i = 0; i < LoreCount; i++)
            {
                float Progress = reader.ReadSingle();
                ClassLoreStepProgress clsp = new ClassLoreStepProgress();
                clsp.StepValue = Progress;
                if (i < LoreInternalProgress.Count)
                {
                    LoreInternalProgress[i] = clsp;
                }
                else
                {
                    LoreInternalProgress.Add(clsp);
                }
            }
        }

        public string GetText()
        {
            string Lore = "";
            ClassUnlockLoreBase ClassBase = GetBase;
            bool LoreUnlocked = LoreAlreadyUnlocked;
            for (int i = 0; i < ClassBase.Objectives.Count; i++)
            {
                if (!LoreInternalProgress[i].Complete && !LoreUnlocked)
                {
                    if (Lore == "")
                    {
                        Lore = "[Lore is hidden until you prove your worth]\n";
                    }
                    else
                    {
                        Lore += "...'\n";
                    }
                    switch (ClassBase.Objectives[i].objectivetype)
                    {
                        case ClassUnlockLoreBase.ObjectiveType.Hunt:
                            {
                                ClassUnlockLoreBase.HuntMonsterObjective hmo = (ClassUnlockLoreBase.HuntMonsterObjective)ClassBase.Objectives[i];
                                Lore += "Hunt " + LoreInternalProgress[i].StepValue + "/" + hmo.Stack + " " + hmo.GetMobName + ".";
                            }
                            break;
                        case ClassUnlockLoreBase.ObjectiveType.GatherItem:
                            {
                                ClassUnlockLoreBase.GatherItemObjective gio = (ClassUnlockLoreBase.GatherItemObjective)ClassBase.Objectives[i];
                                Lore += "Collect " + (gio.Stack - LoreInternalProgress[i].StepValue) + " " + gio.GetItemName + ".\nUse the book to place items inside It.";
                            }
                            break;
                        case ClassUnlockLoreBase.ObjectiveType.TalkToNpc:
                            {
                                ClassUnlockLoreBase.TalkToNpcObjective tno = (ClassUnlockLoreBase.TalkToNpcObjective)ClassBase.Objectives[i];
                                Lore += "Speak with " + tno.NpcName + ".";
                            }
                            break;
                    }
                    break;
                }
                else
                {
                    if (Lore == "")
                        Lore += "'";
                    Lore += ClassBase.Objectives[i].LorePiece;
                }
            }
            if (LoreAlreadyUnlocked)
            {
                Lore += "'\nYou have already unlocked this class.";
            }
            else if (IsComplete)
                Lore += "'\nUse the book to unlock the class knowledge.";
            return Lore;
        }

        public void OnBookUsed(Player player)
        {
            if (player.whoAmI != Main.myPlayer || LoreAlreadyUnlocked)
                return;
            ClassUnlockLoreBase ClassBase = GetBase;
            for (int o = 0; o < ClassBase.Objectives.Count; o++)
            {
                if (!LoreInternalProgress[o].Complete)
                {
                    switch (ClassBase.Objectives[o].objectivetype)
                    {
                        case ClassUnlockLoreBase.ObjectiveType.GatherItem:
                            {
                                ClassUnlockLoreBase.GatherItemObjective gio = (ClassUnlockLoreBase.GatherItemObjective)ClassBase.Objectives[o];
                                for (int i = 0; i < 58; i++)
                                {
                                    if (player.inventory[i].type == gio.ItemID)
                                    {
                                        int StackToReduce = gio.Stack - (int)LoreInternalProgress[o].StepValue;
                                        if (player.inventory[i].stack < StackToReduce)
                                        {
                                            StackToReduce = player.inventory[i].stack;
                                        }
                                        LoreInternalProgress[o].StepValue += StackToReduce;
                                        player.inventory[i].stack -= StackToReduce;
                                    }
                                }
                            }
                            break;
                    }
                    break;
                }
            }
        }
                
        public void OnMobKill(int MobID, int MobNetID)
        {
            if (LoreAlreadyUnlocked)
                return;
            ClassUnlockLoreBase ClassBase = GetBase;
            for (int i = 0; i < ClassBase.Objectives.Count; i++)
            {
                if (!LoreInternalProgress[i].Complete)
                {
                    switch (ClassBase.Objectives[i].objectivetype)
                    {
                        case ClassUnlockLoreBase.ObjectiveType.Hunt:
                            {
                                ClassUnlockLoreBase.HuntMonsterObjective hmo = (ClassUnlockLoreBase.HuntMonsterObjective)ClassBase.Objectives[i];
                                if (IsRequiredMonster(MobID, hmo.MobID))
                                {
                                    LoreInternalProgress[i].StepValue++;
                                }
                            }
                            break;
                    }
                    break;
                }
            }
        }

        public static bool IsRequiredMonster(int NpcType, int ReqMobID)
        {
            bool IsQuestMob = false;
            if (NpcType == NPCID.EaterofWorldsHead || NpcType == NPCID.EaterofWorldsBody || NpcType == NPCID.EaterofWorldsTail)
            {
                byte BodyPartCount = 0;
                for (int n = 0; n < 200; n++)
                {
                    if (Main.npc[n].active && Main.npc[n].type == NPCID.EaterofWorldsBody)
                    {
                        BodyPartCount++;
                    }
                }
                IsQuestMob = BodyPartCount < 2;
            }
            else if (NpcType == ReqMobID)
                IsQuestMob = true;
            else
            {
                switch (ReqMobID)
                {
                    case NPCID.Zombie: //Add event monsters to the list.
                        IsQuestMob = NpcType == 430 || NpcType == 132 || NpcType == 186 || NpcType == 432 || NpcType == 187 || NpcType == 433 || NpcType == 188 || NpcType == 434 || NpcType == 189 || NpcType == 435 ||
                            NpcType == 200 || NpcType == 436 || NpcType == 319 || NpcType == 320 || NpcType == 321 || NpcType == 331 || NpcType == 332 || NpcType == 223 || NpcType == 52 || NpcType == 53 || NpcType == 536 ||
                            NpcType == Terraria.ID.NPCID.ZombieEskimo || NpcType == NPCID.ArmedZombieEskimo || NpcType == 255 || NpcType == 254 || NpcType == Terraria.ID.NPCID.BloodZombie;
                        break;
                    case NPCID.ZombieEskimo:
                        IsQuestMob = NpcType == NPCID.ArmedZombieEskimo;
                        break;
                    case NPCID.DemonEye:
                        IsQuestMob = NpcType == 190 || NpcType == 191 || NpcType == 192 || NpcType == 193 || NpcType == 194 || NpcType == 317 || NpcType == 318;
                        break;
                    case NPCID.BloodCrawler:
                        IsQuestMob = NpcType == NPCID.BloodCrawlerWall;
                        break;
                    case NPCID.Demon:
                        IsQuestMob = NpcType == NPCID.VoodooDemon;
                        break;
                    case NPCID.JungleCreeper:
                        IsQuestMob = NpcType == NPCID.JungleCreeperWall;
                        break;
                    case NPCID.Hornet:
                        IsQuestMob = NpcType == NPCID.HornetFatty || NpcType == NPCID.HornetHoney || NpcType == NPCID.HornetLeafy || NpcType == NPCID.HornetSpikey || NpcType == NPCID.HornetStingy;
                        break;
                    case NPCID.AngryBones:
                        IsQuestMob = NpcType == 294 || NpcType == 295 || NpcType == 296;
                        break;
                    case NPCID.BlueArmoredBones:
                        IsQuestMob = NpcType == NPCID.BlueArmoredBonesMace || NpcType == NPCID.BlueArmoredBonesNoPants || NpcType == NPCID.BlueArmoredBonesSword;
                        break;
                    case NPCID.RustyArmoredBonesAxe:
                        IsQuestMob = NpcType == NPCID.RustyArmoredBonesFlail || NpcType == NPCID.RustyArmoredBonesSword || NpcType == NPCID.RustyArmoredBonesSwordNoArmor;
                        break;
                    case NPCID.HellArmoredBones:
                        IsQuestMob = NpcType == NPCID.HellArmoredBonesMace || NpcType == NPCID.HellArmoredBonesSpikeShield || NpcType == NPCID.HellArmoredBonesSword;
                        break;
                    case NPCID.BlueSlime:
                        IsQuestMob = NpcType == NPCID.SlimeRibbonGreen || NpcType == NPCID.SlimeRibbonRed || NpcType == NPCID.SlimeRibbonWhite || NpcType == NPCID.SlimeRibbonYellow || NpcType == 302 ||
                            NpcType == NPCID.SandSlime || NpcType == NPCID.IceSlime || NpcType == NPCID.SpikedIceSlime || NpcType == NPCID.SlimedZombie || NpcType == NPCID.ArmedZombieSlimed ||
                            NpcType == NPCID.LavaSlime || NpcType == NPCID.RainbowSlime || NpcType == NPCID.KingSlime || NpcType == NPCID.IlluminantSlime || NpcType == NPCID.DungeonSlime ||
                            NpcType == NPCID.MotherSlime || NpcType == NPCID.Slimeling || NpcType == NPCID.SlimeMasked || NpcType == NPCID.SlimeSpiked || NpcType == NPCID.SpikedJungleSlime ||
                            NpcType == NPCID.UmbrellaSlime; //302 is Bunny Slime
                        break;
                }
            }
            return IsQuestMob;
        }

        public class ClassLoreStepProgress
        {
            public float StepValue = 0;
            public bool Complete = false;
        }
    }
}
