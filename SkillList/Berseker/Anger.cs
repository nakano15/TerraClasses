using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.SkillList.Berseker
{
    public class Anger : SkillBase
    {
        const byte DamageStackVar = 0;

        public Anger()
        {
            Name = "Anger";
            Description = "Increases your damage as you get hurt.\n" +
                " Damage stack is reduced overtime.\n" +
                " Every 2 Levels reduces the time It takes for the damage stack to deplete.\n" +
                " Level increases the damage bonus based on max health.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Passive;
        }

        public override void Update(Terraria.Player player, SkillData data)
        {
            if (data.GetFloat(DamageStackVar) > 0)
            {
                data.ChangeFloat(DamageStackVar, 1f - (0.1f * (data.Level * 0.5f)));
            }
        }

        public override void UpdateStatus(Terraria.Player player, SkillData data)
        {
            float DamageStack = data.GetFloat(DamageStackVar) / player.statLifeMax2;
            if(DamageStack > 0)
                player.meleeDamage += DamageStack;
        }

        public override void OnHitByNPC(Terraria.Player player, SkillData data, Terraria.NPC npc, int damage, bool crit)
        {
            data.ChangeFloat(DamageStackVar, damage);
        }

        public override void OnHitByProjectile(Terraria.Player player, SkillData data, Terraria.Projectile proj, int damage, bool crit)
        {
            data.ChangeFloat(DamageStackVar, damage);
        }
    }
}
