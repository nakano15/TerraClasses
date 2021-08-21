using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.ClassList
{
    public class Hoplite : ClassBase
    {
        public Hoplite()
        {
            Name = "Hoplite";
            Description = "You are not entertained.";
            MaxLevel = 50;
            AddSkill(80);
            AddSkill(81);
            AddSkill(82);
        }
    }
}
