using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.ClassList
{
    public class Knight : ClassBase
    {
        public Knight()
        {
            Name = "Knight";
            Description = "";
            AddSkill(49);
            AddSkill(50);
            AddSkill(51);
        }

        public override ClassUnlockLoreBase CreateLoreBase()
        {
            ClassUnlockLoreBase Lore = new ClassUnlockLoreBase();
            return Lore;
        }
    }
}
