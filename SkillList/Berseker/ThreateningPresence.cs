using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace TerraClasses.SkillList.Berseker
{
    public class ThreateningPresence : SkillBase
    {
        public ThreateningPresence()
        {
            Name = "Threatening Presence";
            Description = "Killing enemies makes weak monsters nearby run away.\n" +
                "Skill level makes tougher monsters flee from you, aswell as increases range.\n" +
                "Critical Hits increases factor.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Passive;
        }

        /*public override void Update(Player player, SkillData data)
        {
            if (!player.inventory[player.selectedItem].melee)
                return;
            float Factor = player.inventory[player.selectedItem].damage * 0.66f + 5 + data.Level * 2;

        }*/

        public override void OnHitNPC(Player player, SkillData data, Item item, NPC target, int damage, float knockback, bool crit)
        {
            if(target.life <= 0 && item.melee)
            {
                AffectNearbyTargets(player, item, data, target, crit);
            }
        }

        public override void OnHitNPCWithProj(Player player, SkillData data, Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (target.life <= 0 && proj.melee)
            {
                AffectNearbyTargets(player, player.inventory[player.selectedItem], data, target, crit);
            }
        }

        private void AffectNearbyTargets(Player player, Item item, SkillData data, NPC target, bool crit = false)
        {
            float Factor = item.damage * 0.66f + 3 + data.Level * 2;
            float Range = 200f + data.Level * 10f;

            if (crit)
                Factor += 15;
            for (int n = 0; n < 200; n++)
            {
                if (n != target.whoAmI && Main.npc[n].active && !Main.npc[n].buffImmune[Terraria.ID.BuffID.Confused] && (Main.npc[n].Center - player.Center).Length() < Range + (Math.Max(Main.npc[n].width, Main.npc[n].height) + player.height) * 0.5f && Main.npc[n].defense < Factor)
                {
                    target.AddBuff(Terraria.ID.BuffID.Confused, 20 * 60);
                }
            }
        }
    }
}
