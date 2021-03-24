using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace TerraClasses.SkillList.Knight
{
    public class HeavySlam : SkillBase
    {
        public HeavySlam()
        {
            Name = "Heavy Slam";
            Description = "Use the most of your strength to hit your foes.\n" +
                " Need a Melee Weapon.\n" +
                " Causes 180% + 16% per level of Melee Damage.\n" +
                " After level 5, inflicts Broken Armor on anyone affected by the attack.";
            skillType = Enum.SkillTypes.Active;
            MaxLevel = 10;
        }

        public override void UpdateItemUse(Player player, SkillData data, bool JustUsed)
        {
            if (player.itemAnimation > 0 && player.inventory[player.selectedItem].melee)
            {
                if (player.itemAnimation == 1)
                    data.EndUse();
            }
        }

        public override void ModifyHitNPC(Player player, SkillData data, Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            if (item.melee)
            {
                damage = (int)(damage * (1.8f + 0.16f * data.Level));
                if (data.Level > 5)
                    target.AddBuff(Terraria.ID.BuffID.BrokenArmor, 15 * 60);
            }
        }

        public override void ModifyHitNPCWithProj(Player player, SkillData data, Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (proj.melee)
            {
                damage = (int)(damage * (1.8f + 0.16f * data.Level));
                if (data.Level > 5)
                    target.AddBuff(Terraria.ID.BuffID.BrokenArmor, 15 * 60);
            }
        }
    }
}
