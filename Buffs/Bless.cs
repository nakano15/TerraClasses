using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace TerraClasses.Buffs
{
    public class Bless : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Bless");
            Description.SetDefault("Increases Damages and Defense.");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            int SkillLevel = PlayerMod.GetPlayerSkillBuffLevel(player, player.buffType[buffIndex]);
            float DamBonus = 0.04f * SkillLevel, DefBonus = 0.02f * SkillLevel;
            player.meleeDamage += player.meleeDamage * DamBonus;
            player.rangedDamage += player.rangedDamage * DamBonus;
            player.magicDamage += player.magicDamage * DamBonus;
            player.minionDamage += player.minionDamage * DamBonus;
            player.statDefense += (int)(player.statDefense * DefBonus);
        }
    }
}
