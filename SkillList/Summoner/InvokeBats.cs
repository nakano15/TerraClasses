using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace TerraClasses.SkillList.Summoner
{
    public class InvokeBats : SkillBase
    {
        public InvokeBats()
        {
            Name = "Invoke Bats";
            Description = "Summon bats to attack your foes, as you attack.\n" +
                " Summon Damage increases by 60% + 2% per level.\n" +
                " Your character may recover some health when a bat hits a monster.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Attack;
        }

        public override SkillData GetSkillData => new InvokeBatsSkillData();

        public override void Update(Terraria.Player player, SkillData rawdata)
        {
            InvokeBatsSkillData data = (InvokeBatsSkillData)rawdata;
            const float ShotDirectionVariation = 0.2f;
            if (data.Time == 0)
            {
                data.ShotDirectionY = -ShotDirectionVariation * 2;
            }
            if (data.Time >= 21 && player.itemAnimation > 0 && player.HeldItem.damage > 0)
            {
                data.ChangeStep();
                int Damage = data.GetSummonDamage(0, 0.6f + 0.02f * data.Level, player);
                Vector2 ShotDirection = new Vector2(player.direction * -2f, data.ShotDirectionY);
                int ProjPos = Projectile.NewProjectile(player.Center, ShotDirection, 316, Damage, 0f, player.whoAmI);
                if (!data.BatPositions.Contains(ProjPos))
                {
                    data.BatPositions.Add(ProjPos);
                }
                data.ShotDirectionY += ShotDirectionVariation;
                if (data.ShotDirectionY >= ShotDirectionVariation * 2)
                    data.ShotDirectionY *= -1;
            }
        }

        public override void OnHitNPCWithProj(Player player, SkillData rawdata, Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            InvokeBatsSkillData data = (InvokeBatsSkillData)rawdata;
            if (proj.type == 316 && data.BatPositions.Contains(proj.whoAmI) && Main.rand.Next(100) < data.Level)
            {
                data.BatPositions.Remove(proj.whoAmI);
                int HealthChange = 1;
                if (player.statLifeMax2 > player.statLifeMax)
                {
                    HealthChange = (int)(player.statLifeMax2 / player.statLifeMax);
                }
                player.statLife += HealthChange;
                CombatText.NewText(player.getRect(), CombatText.HealLife, HealthChange, false, true);
            }
        }

        public class InvokeBatsSkillData : SkillData
        {
            public List<int> BatPositions = new List<int>();
            public float ShotDirectionY = 0;
        }
    }
}
