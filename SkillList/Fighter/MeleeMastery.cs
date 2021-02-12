using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.SkillList.Fighter
{
    public class MeleeMastery : SkillBase
    {
        public MeleeMastery()
        {
            Name = "Melee Mastery";
            Description = "Increases melee damage by 2% per level.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Passive;
        }

        public override void UpdateStatus(Terraria.Player player, SkillData data)
        {
            player.meleeDamage += 0.02f * data.Level;
        }
    }
}
