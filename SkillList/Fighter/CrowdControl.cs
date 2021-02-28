using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace TerraClasses.SkillList.Fighter
{
    public class CrowdControl : SkillBase
    {
        public CrowdControl()
        {
            Name = "Crowd Control";
            Description = "Increases your character defense by 2 for each hostile monster nearby.\n" +
                " Level increases the radius of checking.\n" +
                " Level 10 increases defense bonus by 3.";
            skillType = Enum.SkillTypes.Passive;
            MaxLevel = 10;
        }

        public override void UpdateStatus(Terraria.Player player, SkillData data)
        {
            int Defbonus = (data.Level >= 10 ? 3 : 2);
            float CheckRadius = 86f + 32f * (data.Level - 1);
            for (int x = 0; x < 255; x++)
            {
                if (player.hostile && x != player.whoAmI && Main.player[x].active && !Main.player[x].dead && Main.player[x].hostile && (Main.player[x].team == 0 || Main.player[x].team != player.team) && Main.player[x].Distance(player.Center) < CheckRadius)
                {
                    player.statDefense += Defbonus;
                }
                if (x < 200 && Main.npc[x].active && !Main.npc[x].friendly && !Main.npc[x].townNPC && Main.npc[x].Distance(player.Center) < CheckRadius)
                {
                    player.statDefense += Defbonus;
                }
            }
        }
    }
}
