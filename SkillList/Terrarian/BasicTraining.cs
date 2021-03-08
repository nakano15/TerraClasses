using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.SkillList.Terrarian
{
    public class BasicTraining : SkillBase
    {
        public BasicTraining()
        {
            Name = "Basic Training";
            Description = "Unlocks features as you level It up.\n" +
                " Level 3 unlocks active skills.\n" +
                " Level 6 unlocks combat skills.\n" +
                " Level 10 unlocks class changes.";
            MaxLevel = 10;
        }

        public override void UpdateStatus(Terraria.Player player, SkillData data)
        {

        }
    }
}
