using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.SkillList.Archer
{
    public class RangedMastery : SkillBase
    {
        public RangedMastery()
        {
            Name = "Ranged Mastery";
            Description = "Increases Ranged Damage by 2% per level.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Passive;
        }

        public override void UpdateStatus(Terraria.Player player, SkillData data)
        {
            player.rangedDamage += 0.02f * data.Level;
        }
    }
}
