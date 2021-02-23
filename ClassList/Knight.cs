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
        }

        public override ClassUnlockLoreBase CreateLoreBase()
        {
            return base.CreateLoreBase();
        }
    }
}
