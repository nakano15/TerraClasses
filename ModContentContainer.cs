using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses
{
    public class ModContentContainer
    {
        private string ModID;
        private static Dictionary<int, ClassBase> ClassList = new Dictionary<int, ClassBase>();
        private static Dictionary<int, SkillBase> SkillList = new Dictionary<int, SkillBase>();

        public ModContentContainer(string ModID)
        {
            this.ModID = ModID;
        }

        public ClassBase GetClassBase(int ID)
        {
            if (!ClassList.ContainsKey(ID))
            {
                ClassBase cb = new ClassBase();
                if (ModID == MainMod.mod.Name)
                {
                    cb = ClassDB.GetClass(ID);
                }
                else
                {
                    cb = GetModClassBase(ID);
                }
                ClassList.Add(ID, cb);
            }
            return ClassList[ID];
        }

        public SkillBase GetSkillBase(int ID)
        {
            if (!SkillList.ContainsKey(ID))
            {
                SkillBase sb = new SkillBase();
                if (ModID == MainMod.mod.Name)
                {
                    sb = SkillDB.GetSkill(ID);
                }
                else
                {
                    sb = GetModSkillBase(ID);
                }
                SkillList.Add(ID, sb);
            }
            return SkillList[ID];
        }

        public virtual ClassBase GetModClassBase(int ID)
        {
            ClassBase cb = new ClassBase();

            return cb;
        }

        public virtual SkillBase GetModSkillBase(int ID)
        {
            SkillBase sb = new SkillBase();

            return sb;
        }
    }
}
