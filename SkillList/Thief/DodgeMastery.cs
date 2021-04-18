using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace TerraClasses.SkillList.Thief
{
    public class DodgeMastery : SkillBase
    {
        public DodgeMastery()
        {
            Name = "Dodge Mastery";
            Description = "Increases the chance of dodging attacks.\n" +
                " Dodge rate increases by 2% per level.";
            MaxLevel = 10;
        }

        public override void ModifyHitByNPC(Player player, SkillData data, NPC npc, ref int damage, ref bool crit)
        {
            if(player.immuneTime == 0 && Main.rand.NextDouble() < 0.02f * data.Level)
            {
                player.immuneTime = (int)(5 + data.Level * 0.5f) * 60;
                CombatText.NewText(player.getRect(), Microsoft.Xna.Framework.Color.Gray, "Dodged");
                damage = 1;
                crit = false;
            }
        }

        public override void ModifyHitByProjectile(Player player, SkillData data, Projectile proj, ref int damage, ref bool crit)
        {
            if (player.immuneTime == 0 && Main.rand.NextDouble() < 0.02f * data.Level)
            {
                player.immuneTime = (int)(5 + data.Level * 0.5f) * 60;
                CombatText.NewText(player.getRect(), Microsoft.Xna.Framework.Color.Gray, "Dodged");
                damage = 1;
                crit = false;
            }
        }
    }
}
