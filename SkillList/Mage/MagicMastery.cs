using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.SkillList.Mage
{
    public class MagicMastery : SkillBase
    {
        public MagicMastery()
        {
            Name = "Magic Mastery";
            Description = "Increases magic damage by 2% per level.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Passive;
        }

        public override void UpdateStatus(Terraria.Player player, SkillData data)
        {
            player.magicDamage += 0.02f * data.Level;
        }
    }
}
