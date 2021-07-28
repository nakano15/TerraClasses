using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace TerraClasses.SkillList.Cerberus
{
    public class ClawSlash : SkillBase
    {
        public ClawSlash()
        {
            Name = "Claw Slash";
            Description = "Cuts foes nearby by ripping them with a claw.\n" +
                " Auto triggers when attacking.\n" +
                " Causes 66% + 1.2% per level of melee damage.\n" +
                " Claws are affected by melee speed accessories.";
            skillType = Enum.SkillTypes.Attack;
            MaxLevel = 10;
        }

        public override SkillData GetSkillData => new ClawSlashData();

        public int GetAttackDuration(Player player, SkillData data)
        {
            return (int)((40 - Math.Min(data.Level * 0.5f, 10)) * player.meleeSpeed);
        }

        public override void Update(Player player, SkillData rawdata)
        {
            ClawSlashData data = (ClawSlashData)rawdata;
            if (data.AttackDuration > 0)
            {
                Rectangle Hitbox = new Rectangle((int)data.ClawSpawnPosition.X - 32, (int)data.ClawSpawnPosition.Y - 32, 64, 64);
                int Damage = data.GetMeleeDamage(0, 0.66f + 0.012f * data.Level, player);
                for (int i = 0; i < 200; i++)
                {
                    if (Main.npc[i].active && !Main.npc[i].friendly && !Main.npc[i].dontTakeDamage && Main.npc[i].getRect().Intersects(Hitbox))
                    {
                        data.HurtNpc(Main.npc[i], Damage, player.direction, 1.3f);
                    }
                }
                data.AttackDuration--;
            }
        }

        public override void UpdateItemUse(Player player, SkillData rawdata, bool JustUsed)
        {
            ClawSlashData data = (ClawSlashData)rawdata;
            if (data.AttackDuration == 0)
            {
                if (player.controlUseItem)
                {
                    Vector2 ClawPosition = player.Center;
                    Vector2 MousePosition = new Vector2(Main.mouseX + Main.screenPosition.X, Main.mouseY + Main.screenPosition.Y) - ClawPosition;
                    MousePosition.Normalize();
                    ClawPosition += MousePosition * 64;
                    data.ClawSpawnPosition = ClawPosition;
                    data.AttackDuration = data.MaxAttackDuration = GetAttackDuration(player, data);
                    data.MarkType = (byte)Main.rand.Next(2);
                    data.Rotation = (float)Math.PI * (Main.rand.NextFloat() * 0.4f - 0.2f);
                }
            }
        }

        public override void Draw(Player player, SkillData rawdata, PlayerDrawInfo pdi)
        {
            ClawSlashData data = (ClawSlashData)rawdata;
            if(data.AttackDuration > 0)
            {
                Vector2 Origin = new Vector2(32, 32);
                Rectangle rect = new Rectangle(64 * data.MarkType, 0, 64, 64);
                Color color = Color.White;
                if(data.AttackDuration < data.MaxAttackDuration * 0.5f)
                {
                    float Progress = (data.AttackDuration / (data.MaxAttackDuration * 0.5f));
                    //rect.Height = (int)(rect.Height * Progress);
                    //rect.Y = 64 - rect.Height;
                    color *= Progress;
                    //color.R = color.G = color.B = (byte)(255 * Progress);
                }
                else
                {
                    rect.Height = (int)(rect.Height * (1f - (data.AttackDuration - data.MaxAttackDuration * 0.5f) / (data.MaxAttackDuration * 0.5f)));
                    //rect.Y = 64 - rect.Height;
                }
                Terraria.DataStructures.DrawData dd = new Terraria.DataStructures.DrawData(MainMod.CerberusClawMark.GetTexture, data.ClawSpawnPosition - Main.screenPosition,
                    rect, color, data.Rotation, Origin, 1f, pdi.spriteEffects, 0);
                Main.playerDrawData.Insert(0, dd);
            }
        }

        public class ClawSlashData : SkillData
        {
            public Vector2 ClawSpawnPosition = Vector2.Zero;
            public int AttackDuration = 0, MaxAttackDuration = 0;
            public byte MarkType = 0;
            public float Rotation = 0f;
        }
    }
}
