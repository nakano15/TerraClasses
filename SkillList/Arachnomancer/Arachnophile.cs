using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace TerraClasses.SkillList.Arachnomancer
{
    public class Arachnophile : SkillBase
    {
        public Arachnophile()
        {
            Name = "Arachnophilia";
            Description = "Makes spiders friendly towards you.\n" +
                " Blood Crawler and Sand Poachers are not affected from this.\n" +
                " Spiders web spit restores 5% of your character health when hit by it, and gives Regeneration buff.";
        }

        public override void UpdateStatus(Player player, SkillData data)
        {
            player.npcTypeNoAggro[Terraria.ID.NPCID.WallCreeper] = player.npcTypeNoAggro[Terraria.ID.NPCID.WallCreeperWall] = true;
            player.npcTypeNoAggro[Terraria.ID.NPCID.JungleCreeper] = player.npcTypeNoAggro[Terraria.ID.NPCID.JungleCreeperWall] = true;
            player.npcTypeNoAggro[Terraria.ID.NPCID.BlackRecluse] = player.npcTypeNoAggro[Terraria.ID.NPCID.BlackRecluseWall] = true;
            if (player.HasBuff(Terraria.ID.BuffID.Webbed))
            {
                player.DelBuff(player.FindBuffIndex(Terraria.ID.BuffID.Webbed));
                player.AddBuff(Terraria.ID.BuffID.Regeneration, 5 * 60);
            }
        }

        public override void ModifyHitByProjectile(Player player, SkillData data, Projectile proj, ref int damage, ref bool crit)
        {
            if(proj.type == Terraria.ID.ProjectileID.WebSpit)
            {
                damage = 1;
                int HealthRegen = (int)(player.statLifeMax2 * 0.05f);
                player.statLife += HealthRegen;
                if (player.statLife > player.statLifeMax2)
                    player.statLife = player.statLifeMax2;
                player.HealEffect(HealthRegen);
            }
        }
    }
}
