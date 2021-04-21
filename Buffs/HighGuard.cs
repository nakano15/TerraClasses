using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace TerraClasses.Buffs
{
    public class HighGuard : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("High Guard");
            Description.SetDefault("Your character has taken defensive stance.");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            int SkillLevel = PlayerMod.GetPlayerSkillBuffLevel(player, player.buffType[buffIndex]);
            player.statDefense += 2 * SkillLevel;
            if (SkillLevel > 5)
                player.statDefense += (int)(player.statDefense * 0.05f);
            player.meleeCrit -= (int)(player.meleeCrit * 0.1f);
            player.rangedCrit -= (int)(player.rangedCrit * 0.1f);
            player.magicCrit -= (int)(player.magicCrit * 0.1f);
            player.meleeSpeed -= 0.2f;
            player.meleeDamage += 0.2f;
            player.moveSpeed -= 0.15f;

            player.buffTime[buffIndex] = 5;
        }
    }
}
