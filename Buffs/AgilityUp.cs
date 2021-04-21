using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace TerraClasses.Buffs
{
    public class AgilityUp : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Agility Up");
            Description.SetDefault("Increases your Movement Speed.");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            int SkillLevel = PlayerMod.GetPlayerSkillBuffLevel(player, player.buffType[buffIndex]);
            player.moveSpeed += 0.03f + SkillLevel * 0.02f;
            player.meleeSpeed += 0.03f + SkillLevel * 0.02f;
        }
    }
}
