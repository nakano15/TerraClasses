using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace TerraClasses.SkillList.Cleric
{
    public class IncreaseAgility : SkillBase
    {
        public IncreaseAgility()
        {
            Name = "Increase Agility";
            Description = "Increases agility of allies in range.\n" +
                " Speed Bonus increases by 3% + 2% every 2 levels.\n" +
                " Duration of the buff increases by 30 seconds per level.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Active;
        }

        public override void Update(Player player, SkillData data)
        {
            TargetTranslator.Translator[] Targets = data.GetPossibleTargets(true, true, player.Center, 400);
            foreach(TargetTranslator.Translator target in Targets)
            {
                if(target.Target is Player)
                {
                    data.ApplySkillBuff((Player)target.Target, Terraria.ModLoader.ModContent.BuffType<Buffs.AgilityUp>(), 30 * 60 * data.Level);
                    for(int i = 0; i < 5; i++)
                    {
                        Dust.NewDust(target.Position, target.Width, target.Height, 175);
                    }
                }
            }
            data.EndUse(false);
        }
    }
}
