using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace TerraClasses.SkillList.Knight
{
    public class ConstantRest : SkillBase
    {
        private const byte HealthRegenTimerVar = 0;

        public ConstantRest()
        {
            Name = "Constant Rest";
            Description = "You get an extra health regeneration per second you're stopped.\n" +
                " Restores 5 health every time It triggers.\n" +
                " Health restored increases by 1 every 5 levels.\n" +
                " Health is restored every 3 seconds - 0.15 seconds per level.";
            MaxLevel = 10;
        }

        public override void Update(Player player, SkillData data)
        {
            int HealthRegenValue = data.GetInteger(HealthRegenTimerVar);
            if (player.velocity.X != 0)
            {
                if (HealthRegenValue > 0)
                {
                    data.SetInteger(HealthRegenTimerVar, 0);
                }
            }
            else
            {
                HealthRegenValue++;
                if(HealthRegenValue >= 3 * 60 - 9 * data.Level)
                {
                    if (player.statLife < player.statLifeMax2)
                    {
                        int HealthToRestore = 5 + data.Level / 5;
                        if (player.statLifeMax2 - player.statLife < HealthToRestore)
                            HealthToRestore = player.statLifeMax2 - player.statLife;
                        player.statLife += HealthToRestore;
                        player.HealEffect(HealthToRestore);
                    }
                    data.SetInteger(HealthRegenTimerVar, 0);
                }
                else
                {
                    data.SetInteger(HealthRegenTimerVar, HealthRegenValue);
                }
            }
        }
    }
}
