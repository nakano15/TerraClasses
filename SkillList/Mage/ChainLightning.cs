using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.SkillList.Mage
{
    public class ChainLightning : SkillBase
    {
        public ChainLightning()
        {
            Name = "Chain Lightning";
            Description = "Releases magic bolts that target one enemey after another dealing magic damage. (At max rank can chain up to 8 enemies)";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Active;
        }
    }
}
