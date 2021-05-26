using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace TerraClasses
{
    public class ClassData
    {
        public ClassBase GetClass { get { return MainMod.GetClass(ClassID, ClassModID); } }
        public int ClassID = 0;
        public string ClassModID = "";
        public byte ClassRank = 0;
        public int Level = 1;
        public int Exp = 0;
        public int SkillPoint = 1;
        public List<SkillData> Skills = new List<SkillData>();
        public string Name { get { return GetClass.Name; } }
        public string Description { get { return GetClass.Description; } }
        public int MaxLevel { get { return GetClass.MaxLevel; } }
        public int MaxExp { get { return MainMod.GetClassMaxExp(ClassRank, Level); } }//GetClass.GetLevelExp(Level); } }

        public void ChangeClass(int ClassID, string ClassModID = "")
        {
            if (ClassModID == "")
                ClassModID = "TerraClasses";
            this.ClassID = ClassID;
            this.ClassModID = ClassModID;
            Skills.Clear();
            foreach (ClassSkillInfo skill in GetClass.SkillList)
            {
                SkillData sd = MainMod.GetSkill(skill.SkillID, skill.SkillMod).GetSkillData;
                sd.ID = skill.SkillID;
                sd.ModID = skill.SkillMod;
                sd.Level = skill.FreeLevel;
                if (sd.Level > sd.MaxLevel)
                    sd.Level = sd.MaxLevel;
                Skills.Add(sd);
            }
        }

        public void Save(Terraria.ModLoader.IO.TagCompound tag, int ClassIndex)
        {
            string ClassText = "Class_" + ClassIndex;
            tag.Add(ClassText + "_ID", ClassID);
            tag.Add(ClassText + "_ModID", ClassModID);
            tag.Add(ClassText + "_Level", Level);
            tag.Add(ClassText + "_Exp", Exp);
            tag.Add(ClassText + "_SkillCount", Skills.Count);
            for (int s = 0; s < Skills.Count; s++)
            {
                string SkillText = ClassText + "_Skill_" + s;
                tag.Add(SkillText + "_ID", Skills[s].ID);
                tag.Add(SkillText + "_ModID", Skills[s].ModID);
                tag.Add(SkillText + "_Level", Skills[s].RealLevel);
                tag.Add(SkillText + "_Cooldown", Skills[s].Cooldown);
            }
        }

        public void Load(Terraria.ModLoader.IO.TagCompound tag, int ClassIndex, int ModVersion)
        {
            string ClassText = "Class_" + ClassIndex;
            int ClassID = tag.GetInt(ClassText + "_ID");
            string ClassModID = tag.GetString(ClassText + "_ModID");
            ChangeClass(ClassID, ClassModID);
            Level = tag.GetInt(ClassText + "_Level");
            Exp = tag.GetInt(ClassText + "_Exp");
            int SkillCount = tag.GetInt(ClassText + "_SkillCount");
            for (int s = 0; s < SkillCount; s++)
            {
                string SkillText = ClassText + "_Skill_" + s;
                int ID = tag.GetInt(SkillText + "_ID");
                string ModID = tag.GetString(SkillText + "_ModID");
                int SkillLevel = tag.GetInt(SkillText + "_Level");
                int Cooldown = tag.GetInt(SkillText + "_Cooldown");
                foreach (SkillData sd in Skills)
                {
                    if (sd.ID == ID && sd.ModID == ModID)
                    {
                        sd.Level = SkillLevel;
                        sd.Cooldown = Cooldown;
                        break;
                    }
                }
            }
        }

        public void ResetClassSkills()
        {
            ClassBase cb = GetClass;
            for (int cs = 0; cs < cb.SkillList.Count; cs++)
            {
                ResetSkill(cb.SkillList[cs]);
            }
        }

        public void ResetSkill(int SkillID, string SkillModID = "")
        {
            if (SkillModID == "") SkillModID = MainMod.mod.Name;
            ClassBase cb = GetClass;
            ClassSkillInfo csi = null;
            for(int cs = 0; cs < cb.SkillList.Count; cs++)
            {
                if(cb.SkillList[cs].SkillID == SkillID && cb.SkillList[cs].SkillMod == SkillModID)
                {
                    csi = cb.SkillList[cs];
                    break;
                }
            }
            if (csi == null) return;
            ResetSkill(csi);
        }

        public void ResetSkill(ClassSkillInfo csi)
        {
            for (int s = 0; s < Skills.Count; s++)
            {
                if (Skills[s].ID == csi.SkillID && Skills[s].ModID == csi.SkillMod)
                {
                    int LevelsToRemove = Skills[s].Level - csi.FreeLevel;
                    if (LevelsToRemove > 0)
                    {
                        Skills[s].Level -= LevelsToRemove;
                        SkillPoint += LevelsToRemove;
                        return;
                    }
                }
            }
        }

        public void Update(Player player, int ClassPosition)
        {
            this.ClassRank = (byte)ClassPosition;
            int SP = Level;
            for (int s = 0; s < Skills.Count; s++)
            {
                SkillData sd = Skills[s];
                if (sd.Level > 0)
                {
                    //sd.GetBase.Update(player, sd);
                    if (sd.SkillTypes == Enum.SkillTypes.Active)
                    {
                        if (sd.Active)
                        {
                            player.GetModPlayer<PlayerMod>().SkillBeingUsed.SkillPosition = s;
                            player.GetModPlayer<PlayerMod>().SkillBeingUsed.ClassPosition = ClassPosition;
                        }
                    }
                    if (sd.SkillTypes == Enum.SkillTypes.Attack)
                    {
                        SkillSlot CombatSlot = player.GetModPlayer<PlayerMod>().CombatSkill;
                        sd.Active = (CombatSlot.ClassPosition == ClassPosition && CombatSlot.SkillPosition == s);
                    }
                    sd.UpdateSkill(player);
                }
                SP -= sd.RealLevel;
            }
            bool LeveledUp = false;
            while (Level < MaxLevel && Exp >= MaxExp)
            {
                Exp -= MaxExp;
                Level++;
                LeveledUp = true;
            }
            if (LeveledUp)
            {
                if (Level >= MaxLevel)
                {
                    Exp = 0;
                    Main.NewText(Name + " mastered!");
                }
                else
                {
                    Main.NewText(Name + " level increased!");
                }
            }
            SkillPoint = SP;
        }

        public void SpendPointOnSkill(int SkillPosition)
        {
            if (SkillPoint > 0 && SkillPosition >= 0 && SkillPosition < Skills.Count && Skills[SkillPosition].Level < Skills[SkillPosition].MaxLevel)
            {
                SkillData sd = Skills[SkillPosition];
                foreach (ClassSkillInfo csi in GetClass.SkillList)
                {
                    if (csi.SkillID == sd.ID && csi.SkillMod == sd.ModID)
                    {
                        bool RequirementMet = true;
                        for (int s = 0; s < 3; s++)
                        {
                            if (csi.SkillReqPosition[s] > -1)
                            {
                                if (Skills[csi.SkillReqPosition[s]].Level < csi.SkillLevel[s])
                                {
                                    RequirementMet = false;
                                    break;
                                }
                            }
                        }
                        if (RequirementMet)
                        {
                            sd.Level++;
                            SkillPoint--;
                        }
                        else
                        {
                            Main.NewText("Skill Requirement not reached yet.");
                        }
                    }
                }
            }
        }
    }
}
