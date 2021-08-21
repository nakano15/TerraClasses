using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace TerraClasses.SkillList.Hoplite
{
    class Survival : SkillBase
    {
        public Survival()
        {
            Name = "Survival";
            Description = "Critical Hits from Melee attacks restores your health.\n" +
                "  Restores health based on 15% of Critical Damage.\n" +
                "  Health Restored by critical hits increases by 2% per 3 levels.\n" +
                "  At every 5 levels, Melee Critical Hit rate increases by 3.\n" +
                "  At level 10, defense increases by 5.\n" +
                "  Vampire Knives will not trigger this effect.";
            MaxLevel = 10;
        }

        public override void UpdateStatus(Player player, SkillData data)
        {
            for (int i = 5; i <= data.Level; i += 5)
            {
                player.meleeCrit += 3;
            }
            if (data.Level >= 10)
                player.statDefense += 5;
        }

        public override void OnHitNPC(Player player, SkillData data, Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (item.melee && crit)
            {
                DoLifeRestore(player, data, damage);
            }
        }

        public override void OnHitNPCWithProj(Player player, SkillData data, Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (proj.melee && proj.type != Terraria.ID.ProjectileID.VampireKnife && crit)
            {
                DoLifeRestore(player, data, damage);
            }
        }

        public void DoLifeRestore(Player player, SkillData sd, int Damage)
        {
            float RestorePercentage = 0.15f;
            for(int i = 3; i <= sd.Level; i += 3)
            {
                RestorePercentage += 0.02f;
            }
            int HealthRestored = (int)((Damage * 2) * RestorePercentage);
        }
    }
}
