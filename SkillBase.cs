using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace TerraClasses
{
    public class SkillBase
    {
        public string Name = "Unknown";
        public string Description = "The skill creator forgot to setup this...";
        public int MaxLevel = 1;
        public int ManaCost = 0;
        public int Cooldown = 0;
        public bool UnnallowAttacksWhileCasting = false;
        public Enum.SkillTypes skillType = Enum.SkillTypes.Passive;

        public int GetCooldown(int Seconds, int Minutes = 0, int Hours = 0)
        {
            return Seconds * 60 + Minutes * 3600 + Hours * 216000;
        }

        public virtual void Update(Player player, SkillData data)
        {

        }

        public virtual void UpdateAnimation(Player player, SkillData data)
        {

        }

        public virtual void UpdateStatus(Player player, SkillData data)
        {

        }

        public virtual void UpdateItemUse(Player player, SkillData data, bool JustUsed)
        {

        }

        public virtual bool BeforeShooting(Player player, SkillData data, Item weapon, ref int type, ref int damage, ref float knockback, ref Microsoft.Xna.Framework.Vector2 Position, ref float SpeedX, ref float SpeedY)
        {
            return true;
        }

        public virtual void OnHitNPC(Player player, SkillData data, Item item, NPC target, int damage, float knockback, bool crit)
        {

        }
        public virtual void OnHitNPCWithProj(Player player, SkillData data, Projectile proj, NPC target, int damage, float knockback, bool crit)
        {

        }

        public virtual void OnHitByNPC(Player player, SkillData data, NPC npc, int damage, bool crit)
        {

        }

        public virtual void OnHitByProjectile(Player player, SkillData data, Projectile proj, int damage, bool crit)
        {

        }

        public virtual void ModifyHitNPC(Player player, SkillData data, Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {

        }

        public virtual void ModifyHitNPCWithProj(Player player, SkillData data, Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {

        }

        public virtual void ModifyHitByNPC(Player player, SkillData data, NPC npc, ref int damage, ref bool crit)
        {

        }

        public virtual void ModifyHitByProjectile(Player player, SkillData data, Projectile proj, ref int damage, ref bool crit)
        {

        }

        public virtual void Draw(Player player, SkillData data, Terraria.ModLoader.PlayerDrawInfo pdi)
        {

        }

        public Vector2 GetDirection(Vector2 ShotSpawn, Vector2 ShotDestination)
        {
            Vector2 ShotDirection = ShotDestination - ShotSpawn;
            ShotDirection.Normalize();
            return ShotDirection;
        }

        public float GetRotation(Vector2 ShotSpawn, Vector2 ShotDestination)
        {
            Vector2 Result = GetDirection(ShotSpawn, ShotDestination);
            return (float)Math.Atan2(Result.Y, Result.X);
        }
    }
}
