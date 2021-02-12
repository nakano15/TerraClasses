using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.ClassList
{
    public class Terrarian : ClassBase
    {
        public Terrarian()
        {
            Name = "Terrarian";
            Description = "The initial class a Terrarian can have.\nYou can choose a path after reaching level 10 with it.";
            AddSkill(0);
        }

        public override int GetLevelExp(int Level)
        {
            const int a = 5, b = 2, c = 20;
            return a * Level * Level + b * Level + c;
        }
    }
}
