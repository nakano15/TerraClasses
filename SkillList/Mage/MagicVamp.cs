using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.SkillList.Mage
{
    public class MagicVamp : SkillBase
    {
        public MagicVamp()
        {
            Name = "Magic Vamp";
            Description = "Occassionally decreases enemies magic defense and strenghtens players magic weapon damage \nby a percentage when attacking with weapons that deal magic damage only.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Attack;
        }
    }
}
