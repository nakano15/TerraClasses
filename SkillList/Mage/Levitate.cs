using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.SkillList.Mage
{
    public class Levitate : SkillBase
    {
        public Levitate()
        {
            Name = "Levitate"; //(Could be a spell you hold that drains mana or a spell that last for 10 seconds with a 15 second cooldown)
            Description = "Allows the player to float like if they were on a magic carpet.Has a blue visual effect under the characters feet.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Active;
        }
    }
}
