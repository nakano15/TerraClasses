using System;
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
        private Texture2D _SkillBackgroundTextureStyle;
        public Texture2D SkillBackgroundTextureStyle { get { if(_SkillBackgroundTextureStyle == null || _SkillBackgroundTextureStyle.IsDisposed) _SkillBackgroundTextureStyle = MainMod.SkillBackgroundTexture; return _SkillBackgroundTextureStyle; } }
        public byte BackgroundTextureX = 0, BackgroundTextureY = 0;
        private Texture2D _SkillTexture = null;

        public SkillBase()
        {
        }

        public void SetSkillBackgroundTexture(Texture2D texture)
        {
            _SkillBackgroundTextureStyle = texture;
        }


        public void DrawSkillIcon(Vector2 Position, float OriginX = 0, float OriginY = 0, bool DrawBackground = true, bool DrawForeground = true, bool DrawEffect = true, bool DrawBorder = true, SkillData sd = null, ClassBase cb = null)
        {
            Vector2 DrawPosition = Position;
            DrawPosition.X -= OriginX * 32;
            DrawPosition.Y -= OriginY * 32;
            Color SkillColor = Color.White;
            if (cb != null) SkillColor = cb.SkillColor;
            if (DrawBackground)
            {
                Main.spriteBatch.Draw(MainMod.SkillSlotTexture, DrawPosition, Color.White);
                Texture2D SkillBackground = SkillBackgroundTextureStyle;
                Main.spriteBatch.Draw(SkillBackground, DrawPosition, new Rectangle(BackgroundTextureX, BackgroundTextureY, 32, 32), SkillColor);
            }
            if (DrawForeground)
            {
                Texture2D SkillIcon = GetIconTexture();
                Main.spriteBatch.Draw(SkillIcon, DrawPosition, Color.White);
            }
            if (DrawEffect && sd != null)
            {
                if (sd.Cooldown > 0)
                {
                    float Percentage = (float)sd.Cooldown / Cooldown;
                    DrawPosition.Y += (1f - Percentage) * 32;
                    Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle((int)DrawPosition.X, (int)DrawPosition.Y, 32, (int)(32 * Percentage)), null, Color.Red * 0.5f);
                    float CooldownTime = sd.Cooldown * (1f / 60);
                    if (CooldownTime > 5)
                        CooldownTime = (int)CooldownTime;
                    else
                        CooldownTime = (float)Math.Round(CooldownTime, 1);
                    Vector2 TextPosition = new Vector2(Position.X + 16, Position.Y + 32);
                    string mes = "";
                    if(CooldownTime >= 60)
                    {
                        mes = (int)(CooldownTime * (1f / 60)) + "m";
                    }
                    else
                    {
                        mes = CooldownTime + "s";
                    }
                    Utils.DrawBorderString(Main.spriteBatch, mes, TextPosition, Color.Wheat, 0.8f, 0.5f, 1);
                }
            }
            if (DrawBorder)
            {
                Color color = Color.White;
                switch (skillType)
                {
                    case Enum.SkillTypes.Active:
                        color = Color.Green;
                        break;
                    case Enum.SkillTypes.Attack:
                        color = Color.Red;
                        break;
                    case Enum.SkillTypes.Passive:
                        color = Color.Goldenrod;
                        break;
                }
                MainMod.DrawSkillIconBorder(Position, color);
            }
        }

        public Texture2D GetIconTexture()
        {
            if(_SkillTexture == null || _SkillTexture.IsDisposed)
            {
                string TextureDirectory = this.GetType().Namespace.Replace(".", "/").Replace("TerraClasses/", "") + "/" + this.GetType().Name;
                if (MainMod.mod.TextureExists(TextureDirectory))
                {
                    _SkillTexture = MainMod.mod.GetTexture(TextureDirectory);
                }
                else
                {
                    _SkillTexture = MainMod.SkillEmptyTexture;
                    //Main.NewText("Texture does not exists at: " + TextureDirectory);
                }
            }
            return _SkillTexture;
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

        public virtual int SkillProjectileDamagePlayer(Projectile proj, SkillData data, PlayerMod Owner, PlayerMod Target, int Damage, bool Critical)
        {
            return Damage;
        }

        public virtual int SkillProjectileDamageNpc(Projectile proj, SkillData data, PlayerMod Owner, NPC Target, int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            return damage;
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
