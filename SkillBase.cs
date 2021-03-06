﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TerraClasses
{
    public class SkillBase
    {
        public string Name = "Unknown", WikiPage = null;
        public string Description = "The skill creator forgot to setup this...";
        public int MaxLevel = 1;
        public int ManaCost = 0;
        public int Cooldown = 0;
        public int CastTime = 0;
        public bool UnnallowAttacksWhileCasting = false, UnallowOtherSkillUsage = false;
        public Enum.SkillTypes skillType = Enum.SkillTypes.Passive;
        public PositionToTakeOnCastEnum PositionToTake = PositionToTakeOnCastEnum.Mouse;
        public virtual SkillData GetSkillData { get { return new SkillData(); } }
        private byte[,] SkillIconMap = new byte[16, 16];
        private Color SkillIconColor = Color.White;

        public SkillBase()
        {
            SkillIconMap = new byte[,]{
                {9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9},
                {9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9},
                {9,9,5,5,5,5,5,5,5,5,5,5,5,5,9,9},
                {9,9,5,5,5,5,5,5,5,5,5,5,5,5,9,9},
                {9,9,5,5,5,5,5,5,5,5,5,5,5,5,9,9},
                {9,9,5,5,5,4,4,4,4,4,4,5,5,5,9,9},
                {9,9,5,5,5,4,2,2,2,2,4,5,5,5,9,9},
                {9,9,5,5,5,4,2,1,1,2,4,5,5,5,9,9},
                {9,9,5,5,5,4,2,1,1,2,4,5,5,5,9,9},
                {9,9,5,5,5,4,2,2,2,2,4,5,5,5,9,9},
                {9,9,5,5,5,4,4,4,4,4,4,5,5,5,9,9},
                {9,9,5,5,5,5,5,5,5,5,5,5,5,5,9,9},
                {9,9,5,5,5,5,5,5,5,5,5,5,5,5,9,9},
                {9,9,5,5,5,5,5,5,5,5,5,5,5,5,9,9},
                {9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9},
                {9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9}
            };
        }

        public void ChangeSkillIconColor(Color NewColor)
        {
            this.SkillIconColor = NewColor;
        }

        public void SetSkillIcon(byte[,] SkillByteMap, Color IconColor)
        {
            this.SkillIconMap = SkillByteMap;
            this.SkillIconColor = IconColor;
        }

        public void GetSkillIconInfos (out byte[,] ByteMap, out Color color)
        {
            ByteMap = SkillIconMap;
            color = SkillIconColor;
        }

        public virtual float GetEffectRange(SkillData sd)
        {
            return 0;
        }

        public int GetTime(int Seconds, int Minutes = 0, int Hours = 0)
        {
            return Seconds * 60 + Minutes * 3600 + Hours * 216000;
        }

        public Texture2D GetProjectileTexture(int ID)
        {
            if (!Main.projectileLoaded[ID])
                Main.instance.LoadProjectile(ID);
            return Main.projectileTexture[ID];
        }

        public Texture2D GetNPCTexture(int ID)
        {
            if (!Main.NPCLoaded[ID])
                Main.instance.LoadNPC(ID);
            return Main.npcTexture[ID];
        }

        public Texture2D GetItemTexture(int ID)
        {
            return Main.itemTexture[ID];
        }

        public Texture2D GetGoreTexture(int ID)
        {
            if (!Main.goreLoaded[ID])
                Main.instance.LoadGore(ID);
            return Main.goreTexture[ID];
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

        public static Vector2 GetMousePositionInTheWorld { get { return new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition; } }

        public void FakeWeaponUsage(Player player, int WeaponPosition, float ItemRotation, int Duration = 8)
        {
            player.GetModPlayer<PlayerMod>().FakeWeaponUsage(WeaponPosition, ItemRotation, Duration);
        }

        public void FakeWeaponUsage(Player player, int WeaponPosition, Vector2 AimDirection, int Duration = 8)
        {
            player.GetModPlayer<PlayerMod>().FakeWeaponUsage(WeaponPosition, AimDirection, Duration);
        }

        public enum PositionToTakeOnCastEnum : byte
        {
            Mouse,
            Player
        }
    }
}
