using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace TerraClasses.SkillList.Summoner
{
    public class HauntingPresence : SkillBase
    {
        private static List<int> SpawnedHauntProjPos = new List<int>();

        public HauntingPresence()
        {
            Name = "Haunting Presence";
            Description = "Whenever your character has foes nearby,\n" +
                "your minions will invoke Lost Souls too attack them.\n" +
                " Lost Soul inflicts 35% minion damage, ignoring defense.\n" +
                "  Defense ignore does not works on monsters with thousands defense\n" +
                " Skill level and summon count increases how frequently they are shot.\n" +
                "  Summon count only impacts on this up to 10.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Passive;
        }

        public override void Update(Terraria.Player player, SkillData rawdata)
        {
            HauntingPresenceData data = (HauntingPresenceData)rawdata;
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
                Vector2 NearestHostilePosition = Vector2.Zero;
                float NearestHostileDistance = 500f;
                for (int n = 0; n < 200; n++)
                {
                    float Distance = 0;
                    if (Main.npc[n].active && !Main.npc[n].friendly && Main.npc[n].CanBeChasedBy() && (Distance = Main.npc[n].Distance(player.Center)) < NearestHostileDistance && 
                        Collision.CanHit(Main.npc[n].position, Main.npc[n].width, Main.npc[n].height, player.position, player.width, player.height))
                    {
                        HostileNearby = true;
                        NearestHostileDistance = Distance;
                        NearestHostilePosition = Main.npc[n].Center;
                    }
                }
                if (HostileNearby)
                {
                    int SummonIndex = data.LastShooterSummon;
                    int LastSummonPos = -1;
                    int SummonPosStack = 0;
                    for (int proj = 0; proj < 1000; proj++)
                    {
                        if (Main.projectile[proj].active && Main.projectile[proj].owner == player.whoAmI && Main.projectile[proj].minion &&
                            Main.projectile[proj].type != Terraria.ID.ProjectileID.StardustGuardian)
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
                        Vector2 LaunchDirection = (NearestHostilePosition - Main.projectile[LastSummonPos].Center);
                        LaunchDirection.Normalize();
                        int ProjPos = Projectile.NewProjectile(Main.projectile[LastSummonPos].Center, LaunchDirection * 8, 297, data.GetSummonDamage(0, 0.35f, player), 0.7f, player.whoAmI);
                        if (ProjPos > -1)
                        {
                            if (!SpawnedHauntProjPos.Contains(ProjPos))
                            {
                                SpawnedHauntProjPos.Add(ProjPos);
                            }
                        }
                    };
                    SummonIndex++;
                    if (SummonIndex > player.maxMinions)
                        SummonIndex = 0;
                    data.LastShooterSummon = SummonIndex;
                    data.ChangeStep();
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

        public override SkillData GetSkillData => new HauntingPresenceData();

        public class HauntingPresenceData : SkillData
        {
            public int LastShooterSummon = 0;
        }
    }
}
