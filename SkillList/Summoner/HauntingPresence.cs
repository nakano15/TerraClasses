﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace TerraClasses.SkillList.Summoner
{
    public class HauntingPresence : SkillBase
    {
        private static List<int> SpawnedHauntProjPos = new List<int>();

        public HauntingPresence()
        {
            Name = "Haunting Presence";
            Description = "Whenever your character has foes nearby,\nyour minions will invoke Lost Souls too attack them.\nLost Soul inflicts 35% minion damage, ignoring defense.\n Defense ignore does not works on monsters with thousands defense\nSkill level and summon count increases how frequently they are shot.\n Summon count only impacts on this up to 10.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Passive;
        }

        public override void Update(Terraria.Player player, SkillData data)
        {
            int Frequence = 120 - data.Level * 4;
            if (player.numMinions > 9)
            {
                Frequence -= 40;
            }
            else
                Frequence -= player.numMinions * 4;
            if (data.Time >= Frequence)
            {
                bool HostileNearby = false;
                for (int n = 0; n < 200; n++)
                {
                    if (Main.npc[n].active && !Main.npc[n].friendly && !Main.npc[n].dontTakeDamage && Main.npc[n].Distance(player.Center) < 450f)
                    {
                        HostileNearby = true;
                        break;
                    }
                }
                if (HostileNearby)
                {
                    int SummonIndex = data.GetInteger(0);
                    int LastSummonPos = -1;
                    int SummonPosStack = 0;
                    for (int proj = 0; proj < 1000; proj++)
                    {
                        if (Main.projectile[proj].active && Main.projectile[proj].owner == player.whoAmI && Main.projectile[proj].minion)
                        {
                            LastSummonPos = proj;
                            if (SummonPosStack == SummonIndex)
                            {
                                break;
                            }
                            SummonPosStack++;
                        }
                    }
                    if (LastSummonPos > -1)
                    {
                        int ProjPos = Projectile.NewProjectile(Main.projectile[LastSummonPos].Center, Microsoft.Xna.Framework.Vector2.Zero, 297, data.GetSummonDamage(0, 0.35f, player), 0.7f, player.whoAmI);
                        if (ProjPos > -1)
                        {
                            if (!SpawnedHauntProjPos.Contains(ProjPos))
                            {
                                SpawnedHauntProjPos.Add(ProjPos);
                            }
                        }
                    }
                    SummonIndex++;
                    if (SummonIndex > player.maxMinions)
                        SummonIndex = 0;
                    data.SetInteger(0, SummonIndex);
                    data.Time = 0;
                }
            }
        }

        public override void ModifyHitNPCWithProj(Player player, SkillData data, Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (SpawnedHauntProjPos.Contains(proj.whoAmI))
            {
                SpawnedHauntProjPos.Remove(proj.whoAmI);
                if(target.defense < 9000)
                    damage += target.defense / 2;
            }
        }
    }
}
