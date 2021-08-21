using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace TerraClasses.SkillList.Hoplite
{
    class AdaptedCombat : SkillBase
    {
        public AdaptedCombat()
        {
            Name = "Adapted Physical Combat";
            Description = "If either Melee or Ranged is lower than the other,\n" +
                " they will get a boost based on diference.\n" +
                "  The boost will be of 2% per level.\n" +
                "  Gains a 10% boost after level 5.\n" +
                "  Gets 3% Melee and Ranged Critical Rate bonus at level 10.";
            MaxLevel = 10;
        }

        public override void UpdateStatus(Player player, SkillData data)
        {
            float Bonus = 0.02f * data.Level;
            if (data.Level > 5)
                Bonus += 0.1f;
            if(player.meleeDamage < player.rangedDamage)
            {
                float Difference = player.rangedDamage - player.meleeDamage;
                player.meleeDamage += Difference * Bonus;
            }
            else
            {
                float Difference = player.meleeDamage - player.rangedDamage;
                player.rangedDamage += Difference * Bonus;
            }
            if(data.Level >= 10)
            {
                player.meleeCrit += 3;
                player.rangedCrit += 3;
            }
        }
    }
}
