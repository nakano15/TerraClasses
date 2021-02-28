using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace TerraClasses.SkillList.Knight
{
    public class Cavalry : SkillBase
    {
        public Cavalry()
        {
            Name = "Cavalry";
            Description = "Makes yourself stronger when doing mounted combat.\n" +
                " Melee Damage increases based on speed. Damage increases with level.\n" +
                " Ranged attacks increased by 1% per level.\n" +
                " Defense increases by 2 per level.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Passive;
        }

        public override void UpdateStatus(Player player, SkillData data)
        {
            if (player.mount.Active)
            {
                player.meleeDamage += Math.Abs(player.velocity.X) / 5f * (0.1f + 0.003f * data.Level);
                player.rangedDamage += 0.01f * data.Level;
                player.statDefense += 2 * data.Level;
            }
        }
    }
}
