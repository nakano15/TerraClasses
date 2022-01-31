using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace TerraClasses.SkillList.Vilemancer
{
    public class CorruptionAffinity : SkillBase
    {
        public CorruptionAffinity()
        {
            Name = "Corruption Affinity";
            Description = "You grow stronger in the corruption.\n" +
                " Corrupt creatures stops being aggressive towards you as level increases.\n" +
                " While in the Corruption: \n"+
                "  Max Health increases by 0.5% per level.\n" +
                "  Damage Increases by 2% per level.\n" +
                "  Defense Increases by 5.\n" +
                "  Health Regeneration increases by 2.";
            MaxLevel = 10;
        }

        public override void UpdateStatus(Player player, SkillData data)
        {
            if (player.ZoneCorrupt)
            {
                player.statLifeMax2 += (int)(player.statLifeMax2 * 0.005f * data.Level);
                player.meleeDamage += 0.02f * data.Level;
                player.statDefense += 5;
                player.lifeRegen += 2;
            }
            player.npcTypeNoAggro[Terraria.ID.NPCID.EaterofSouls] = true;
            player.npcTypeNoAggro[Terraria.ID.NPCID.CorruptBunny] = true;
            player.npcTypeNoAggro[Terraria.ID.NPCID.CorruptGoldfish] = true;
            player.npcTypeNoAggro[Terraria.ID.NPCID.CorruptPenguin] = true;
            if (data.Level >= 3)
                player.npcTypeNoAggro[Terraria.ID.NPCID.DevourerHead] = player.npcTypeNoAggro[Terraria.ID.NPCID.DevourerBody] = player.npcTypeNoAggro[Terraria.ID.NPCID.DevourerTail] = true;
            if (data.Level >= 6)
                player.npcTypeNoAggro[Terraria.ID.NPCID.Corruptor] = true;
            if (data.Level >= 8)
                player.npcTypeNoAggro[Terraria.ID.NPCID.SeekerBody] = player.npcTypeNoAggro[Terraria.ID.NPCID.SeekerHead] = player.npcTypeNoAggro[Terraria.ID.NPCID.SeekerTail] = true;
            if (data.Level >= 10)
                player.npcTypeNoAggro[Terraria.ID.NPCID.Clinger] = true;
        }
    }
}
