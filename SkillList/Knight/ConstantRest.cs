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

        public override SkillData GetSkillData => new ConstantRestData();

        public override void Update(Player player, SkillData rawdata)
        {
            ConstantRestData data = (ConstantRestData)rawdata;
            if (player.velocity.X != 0)
            {
                data.HealthRegenTime = 0;
            }
            else
            {
                data.HealthRegenTime++;
                if(data.HealthRegenTime >= 3 * 60 - 9 * data.Level)
                {
                    if (player.statLife < player.statLifeMax2)
                    {
                        int HealthToRestore = 5 + data.Level / 5;
                        if (player.statLifeMax2 - player.statLife < HealthToRestore)
                            HealthToRestore = player.statLifeMax2 - player.statLife;
                        player.statLife += HealthToRestore;
                        player.HealEffect(HealthToRestore);
                    }
                    data.HealthRegenTime = 0;
                }
            }
        }

        public class ConstantRestData : SkillData
        {
            public int HealthRegenTime = 0;
        }
    }
}
