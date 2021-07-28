using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.SkillList.Berseker
{
    public class Anger : SkillBase
    {
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

        public override SkillData GetSkillData => new AngerData();

        public override void Update(Terraria.Player player, SkillData rawdata)
        {
            AngerData data = (AngerData)rawdata;
            if (data.DamageStack > 0)
            {
                data.DamageStack -= 1f - (0.1f * (data.Level * 0.5f));
                if (data.DamageStack < 0)
                    data.DamageStack = 0;
            }
        }

        public override void UpdateStatus(Terraria.Player player, SkillData rawdata)
        {
            AngerData data = (AngerData)rawdata;
            float DamageStack = data.DamageStack / player.statLifeMax2;
            if(DamageStack > 0)
                player.meleeDamage += DamageStack;
        }

        public override void OnHitByNPC(Terraria.Player player, SkillData data, Terraria.NPC npc, int damage, bool crit)
        {
            ChangeDamageValue(damage, data);
        }

        public override void OnHitByProjectile(Terraria.Player player, SkillData data, Terraria.Projectile proj, int damage, bool crit)
        {
            ChangeDamageValue(damage, data);
        }

        public void ChangeDamageValue(float Damage, SkillData sd)
        {
            AngerData data = (AngerData)sd;
            data.DamageStack += Damage;
        }

        public class AngerData : SkillData
        {
            public float DamageStack = 0;
        }
    }
}
