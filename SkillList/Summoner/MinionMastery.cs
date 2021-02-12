using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.SkillList.Summoner
{
    public class MinionMastery : SkillBase
    {
        public MinionMastery()
        {
            Name = "Minion Mastery";
            Description = "Increases Summon Damage by 2% per level.\nAt level 6 and above, increases maximum number of summons by 1.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Passive;
        }

        public override void UpdateStatus(Terraria.Player player, SkillData data)
        {
            player.minionDamage += 0.02f * data.Level;
            if (data.Level > 5)
                player.maxMinions++;
        }
    }
}
