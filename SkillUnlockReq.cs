using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses
{
    public class ClassSkillInfo
    {
        public int SkillID = 0;
        public string SkillMod = "";
        public int FreeLevel = 0;
        /// <summary>
        /// Not to be mistaken by Skill ID. It's the skill position on class skill tree.
        /// </summary>
        public int[] SkillReqPosition = new int[] { -1, -1, -1 };
        public int[] SkillLevel = new int[] { 1, 1, 1 };

        public SkillBase GetSkillBase
        {
            get
            {
                return MainMod.GetSkill(SkillID, SkillMod);
            }
        }
    }
}
