using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace TerraClasses
{
    public class ClassBase
    {
        public string Name = "Unknown", WikiPage = null;
        public string Description = "The class creator forgot to setup this...";
        public List<ClassSkillInfo> SkillList = new List<ClassSkillInfo>();
        public int MaxLevel = 10;
        public ClassTypes ClassType = ClassTypes.Normal;
        private ClassUnlockLoreBase LoreBase;

        public ClassUnlockLoreBase GetLoreBase
        {
            get
            {
                if (LoreBase == null)
                    LoreBase = CreateLoreBase();
                return LoreBase;
            }
        }

        public virtual ClassUnlockLoreBase CreateLoreBase()
        {
            return new ClassUnlockLoreBase();
        }

        public int AddSkill(int ID, Terraria.ModLoader.Mod mod, int FreeLevel = 0)
        {
            return AddSkill(ID, mod.Name,FreeLevel);
        }

        public int AddSkill(int ID, string ModID = "", int FreeLevel = 0)
        {
            if (ModID == "")
                ModID = "TerraClasses";
            ClassSkillInfo csi = new ClassSkillInfo()
            {
                SkillID = ID,
                SkillMod = ModID,
                FreeLevel = FreeLevel
            };
            int SkillPos = SkillList.Count;
            SkillList.Add(csi);
            return SkillPos;
        }

        public void AddRequirement(int SkillPosition, int SkillLevel)
        {
            if (SkillList.Count == 0)
                return;
            ClassSkillInfo csi = SkillList[SkillList.Count - 1];
            for (int i = 0; i < 3; i++)
            {
                if (csi.SkillReqPosition[i] == -1)
                {
                    csi.SkillReqPosition[i] = SkillPosition;
                    csi.SkillLevel[i] = SkillLevel;
                    return;
                }
            }
        }

        public virtual int GetLevelExp(int Level)
        {
            return Level * Level * 10;
        }

        public enum ClassTypes
        {
            Normal,
            Aspect //Changes player visual, can only have one.
        }
    }
}
