using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace TerraClasses.SkillList.Mage
{
    public class ManaOverload : SkillBase
    {
        public ManaOverload()
        {
            Name = "Mana Overload";
            Description = "Gain 5 bonus mana per 1% of current magic stat if your max mana isn't 400.\nIf your max mana is 400, you gain mana cost reduction based on magic damage.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Passive;
        }

        public override void UpdateStatus(Player player, SkillData data)
        {
            if(player.statManaMax2 != 400)
            {
                player.statManaMax2 += (int)((player.magicDamage - 1f) * 5);
            }
            else
            {
                player.manaCost -= (player.magicDamage - 1f) * 0.05f;
            }
        }
    }
}
