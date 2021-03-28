using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace TerraClasses
{
    public class ItemSkillLevelBonus
    {
        public const int MaxSkillLevelBonus = 5;
        public int SkillID = 0;
        public string SkillModID = "";
        public int SkillLevelBonus = 0;

        public void CreateRandomSkillBonus()
        {
            SkillID = Main.rand.Next(1, MainMod.TotalSkills);
            SkillModID = MainMod.mod.Name;
            SkillLevelBonus = Main.rand.Next(1, MaxSkillLevelBonus + 1);
        }

        public void CreateRandomBonusBasedOnClass(int ClassID, string ClassModID = "")
        {
            ClassModID = MainMod.mod.Name;
            ClassBase cb = MainMod.GetClass(ClassID, ClassModID);
            if (cb.SkillList.Count == 0)
                return;
            ClassSkillInfo csi = cb.SkillList[Main.rand.Next(cb.SkillList.Count)];
            SkillID = csi.SkillID;
            SkillModID = csi.SkillMod;
            SkillLevelBonus = Main.rand.Next(1, MaxSkillLevelBonus + 1);
        }

        public string GetBonusInfo()
        {
            string Text = MainMod.GetSkill(SkillID, SkillModID).Name + " Lv +" + SkillLevelBonus;
            return "[" + Text + "]";
        }
    }
}
