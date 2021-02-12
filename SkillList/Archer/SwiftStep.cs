using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace TerraClasses.SkillList.Archer
{
    public class SwiftStep : SkillBase
    {
        public SwiftStep()
        {
            Name = "Swift Step";
            Description = "Boosts your movement speed by 20% for 30 seconds. ";
            MaxLevel = 10;
            Cooldown = GetCooldown(0, 1);
            skillType = Enum.SkillTypes.Active;
        }

        public override void UpdateStatus(Player player, SkillData data)
        {
            player.moveSpeed += 0.2f;
            if (data.Time >= 60 * 30)
            {
                data.EndUse();
                return;
            }
        }
    }
}
