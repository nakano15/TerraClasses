using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.SkillList.Terrarian
{
    public class BasicTraining : SkillBase
    {
        public BasicTraining()
        {
            Name = "Basic Training";
            Description = "Unlocks features as you level It up.\nLevel 3 unlocks active skills.\nLevel 6 unlocks combat skills.\nLevel 10 unlocks class changes.";
            MaxLevel = 10;
        }

        public override void UpdateStatus(Terraria.Player player, SkillData data)
        {
            player.GetModPlayer<PlayerMod>().BasicTrainingLevel = (byte)data.Level;
        }
    }
}
