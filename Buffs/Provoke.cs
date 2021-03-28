using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace TerraClasses.Buffs
{
    public class Provoke : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Provoke");
            Description.SetDefault("Your character will attract more attention of nearby enemies.");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
            Main.persistentBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            int SkillLevel = PlayerMod.GetPlayerSkillBuffLevel(player, buffIndex);
            player.aggro += 40 * SkillLevel;
            if (SkillLevel > 5)
                player.statDefense += (int)(player.statDefense * 0.05f);
        }
    }
}
