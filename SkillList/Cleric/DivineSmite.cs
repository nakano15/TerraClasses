using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace TerraClasses.SkillList.Cleric
{
    public class DivineSmite : SkillBase
    {
        public DivineSmite()
        {
            Name = "Divine Smite";
            Description = "Drops a star on your foes upon attacking them.\n" +
                " Star causes 26% + 4% per level Magic Damage.\n" +
                " There is a cooldown of 3 seconds between each star drop.\n" +
                " Star count increases for every 5 levels.";
            MaxLevel = 10;
        }

        public override SkillData GetSkillData => new DivineSmiteData();

        public override void Update(Player player, SkillData rawdata)
        {
            DivineSmiteData data = (DivineSmiteData)rawdata;
            if (data.StarCooldown > 0)
                data.StarCooldown--;
        }

        public override void OnHitNPC(Player player, SkillData data, Item item, NPC target, int damage, float knockback, bool crit)
        {
            TryDroppingStar(player, data, target);
        }

        public override void OnHitNPCWithProj(Player player, SkillData data, Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            TryDroppingStar(player, data, target);
        }

        private void TryDroppingStar(Player caster, SkillData rawdata, NPC target)
        {
            if (target.life <= 0)
                return;
            DivineSmiteData data = (DivineSmiteData)rawdata;
            if(data.StarCooldown <= 0)
            {
                List<sbyte> StarPositions = new List<sbyte>();
                for(sbyte s = -2; s <= 2; s++)
                {
                    StarPositions.Add(s);
                }
                byte MaxStars = (byte)(1 + rawdata.Level / 5);
                if (MaxStars > 5) MaxStars = 5;
                for (int i = 0; i < MaxStars; i++)
                {
                    sbyte StarInclination = 0;
                    {
                        int PickedOne = Main.rand.Next(StarPositions.Count);
                        StarInclination = StarPositions[PickedOne];
                        StarPositions.RemoveAt(PickedOne);
                    }
                    Vector2 StarSpawnPosition = new Vector2(target.position.X + 100f * StarInclination, target.position.Y - 600) + target.velocity;
                    Vector2 ShotDirection = (target.Center - StarSpawnPosition);
                    ShotDirection.Normalize();
                    ShotDirection *= 28;
                    int Damage = rawdata.GetMagicDamage(0, 0.26f + 0.04f * data.Level, caster);
                    Projectile.NewProjectile(StarSpawnPosition, ShotDirection, 92, Damage, 0, caster.whoAmI);
                }
                data.StarCooldown = (int)(3 * 60);
            }
        }

        public class DivineSmiteData : SkillData
        {
            public int StarCooldown = 0;
        }
    }
}
