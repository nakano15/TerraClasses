using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.SkillList.Sharpshooter
{
    public class GunKnowledge : SkillBase
    {
        public GunKnowledge()
        {
            Name = "Gun Knowledge";
            Description = "Gives you knowledge on how to use guns.\n" +
                " Weapons using bullets will grant you 1% extra ranged damage.\n" +
                " Gains +6% Ranged Critical Rate when skill is level 6 or above.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Passive;
        }

        public override void UpdateStatus(Terraria.Player player, SkillData data)
        {
            if (player.inventory[player.selectedItem].ammo == Terraria.ID.AmmoID.Bullet)
            {
                player.rangedDamage += 0.01f * data.Level;
                if (data.Level > 5)
                    player.rangedCrit += 6;
            }
        }
    }
}
