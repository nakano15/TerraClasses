using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.SkillList.Mage
{
    public class ManaControl : SkillBase
    {
        public ManaControl()
        {
            Name = "Mana Control";
            Description = "Reduces the mana cost by 1% per level.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Passive;
        }

        public override void UpdateStatus(Terraria.Player player, SkillData data)
        {
            player.manaCost -= 0.01f * data.Level;
        }
    }
}
