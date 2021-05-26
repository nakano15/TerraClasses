using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace TerraClasses.SkillList.Fighter
{
    public class PiercingShot : SkillBase
    {
        public PiercingShot()
        {
            Name = "Piercing Shot";
            Description = "Firearm shots will now inflict extra damage on foes based on their defense.\n" +
                " Defense piercing increases by 5% + 2% per level.\n" +
                " Past level 5, knockback is increased by 20%.";
            MaxLevel = 10;
        }

        public override void ModifyHitNPCWithProj(Player player, SkillData data, Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            switch (proj.type)
            {
                case Terraria.ID.ProjectileID.Bullet:
                case Terraria.ID.ProjectileID.BulletHighVelocity:
                case Terraria.ID.ProjectileID.ChlorophyteBullet:
                case Terraria.ID.ProjectileID.CrystalBullet:
                case Terraria.ID.ProjectileID.CursedBullet:
                case Terraria.ID.ProjectileID.ExplosiveBullet:
                case Terraria.ID.ProjectileID.GoldenBullet:
                case Terraria.ID.ProjectileID.IchorBullet:
                case Terraria.ID.ProjectileID.MoonlordBullet:
                case Terraria.ID.ProjectileID.NanoBullet:
                case Terraria.ID.ProjectileID.PartyBullet:
                case Terraria.ID.ProjectileID.SniperBullet:
                case Terraria.ID.ProjectileID.VenomBullet:
                    float DefensePierce = 0.05f + 0.02f * data.Level;
                    damage += (int)(target.defense * DefensePierce);
                    if(data.Level > 5)
                    {
                        knockback *= 1.2f;
                    }
                    break;
            }
        }
    }
}
