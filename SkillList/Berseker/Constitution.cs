using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.SkillList.Berseker
{
    public class Constitution : SkillBase
    {
        public Constitution()
        {
            Name = "Constitution";
            Description = "Increases your maximum health by 4% per level.\n" +
                " If skill is level 6 or above, starts reducing Defense by 2% per level.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Passive;
        }

        public override void UpdateStatus(Terraria.Player player, SkillData data)
        {
            player.statLifeMax2 += (int)(player.statLifeMax2 * 0.04f * data.Level);
            if (data.Level >= 6)
            {
                player.statDefense -= (int)(player.statDefense * ((data.Level - 5) * 0.02f));
            }
        }
    }
}
