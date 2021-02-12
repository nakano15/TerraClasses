﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace TerraClasses.SkillList.Summoner
{
    public class InvokeBats : SkillBase
    {
        private const byte ShotDirectionVar = 0;
        private static List<int> BatPositions = new List<int>();

        public InvokeBats()
        {
            Name = "Invoke Bats";
            Description = "Summon bats to attack your foes, as you attack.\nSummon Damage increases by 60% + 2% per level.\nYour character may recover some health when a bat hits a monster.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Attack;
        }

        public override void Update(Terraria.Player player, SkillData data)
        {
            const float ShotDirectionVariation = 0.2f;
            if (data.Time == 0)
            {
                data.SetFloat(ShotDirectionVar, -ShotDirectionVariation * 2);
            }
            if (data.Time >= 21 && player.itemAnimation > 0 && player.HeldItem.damage > 0)
            {
                data.Time = 0;
                float ShotDirectionY = data.GetFloat(ShotDirectionVar);
                int Damage = data.GetSummonDamage(0, 0.6f + 0.02f * data.Level, player);
                Vector2 ShotDirection = new Vector2(player.direction * -2f, ShotDirectionY);
                int ProjPos = Projectile.NewProjectile(player.Center, ShotDirection, 316, Damage, 0f, player.whoAmI);
                if (!BatPositions.Contains(ProjPos))
                {
                    BatPositions.Add(ProjPos);
                }
                ShotDirectionY += ShotDirectionVariation;
                if (ShotDirectionY >= ShotDirectionVariation * 2)
                    ShotDirectionY *= -1;
                data.SetFloat(ShotDirectionVar, ShotDirectionY);
            }
        }

        public override void OnHitNPCWithProj(Player player, SkillData data, Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (proj.type == 316 && BatPositions.Contains(proj.whoAmI) && Main.rand.Next(100) < data.Level)
            {
                BatPositions.Remove(proj.whoAmI);
                int HealthChange = 1;
                if (player.statLifeMax2 > player.statLifeMax)
                {
                    HealthChange = (int)(player.statLifeMax2 / player.statLifeMax);
                }
                player.statLife += HealthChange;
                CombatText.NewText(player.getRect(), CombatText.HealLife, HealthChange, false, true);
            }
        }
    }
}
