using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.ClassList
{
    public class Arachnomancer : ClassBase
    {
        public Arachnomancer()
        {
            Name = "Arachnomancer";
            Description = "Summon spiders to aid you in combat.";
            MaxLevel = 50;
            AddSkill(57);
            AddSkill(59);
        }
    }
}
