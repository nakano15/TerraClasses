using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace TerraClasses.SkillList.Cerberus
{
    public class PawStrike : SkillBase
    {
        const int AttackDuration = 30;
        public PawStrike()
        {
            Name = "Paw Strike";
            Description = "Hit your foes with a giant paw.\n" +
                " Inflicts 120% + 44% per level of Melee Damage.\n" +
                " Paw size increases with level.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Active;
            Cooldown = GetTime(32);
        }

        public float GetPawSize(SkillData data)
        {
            return 1f + data.Level * 0.12f;
        }

        public override void Update(Player player, SkillData data)
        {
            float AttackPercentage = data.Time * (1f / AttackDuration);
            AttackPercentage *= AttackPercentage;
            if (AttackPercentage > 1)
            {
                AttackPercentage = 1f;
                data.EndUse();
            }
            Rectangle HitBox = new Rectangle((int)(player.position.X + player.width * 0.5f), (int)(player.position.Y + player.height * 0.5f), 0, 0);
            float PawSize = GetPawSize(data);
            if (AttackPercentage < 0.33f)
            {
                HitBox.Width = (int)(40 * 1.2f * 2 * PawSize);
                HitBox.Height = (int)(40 * 1.2f * 2 * PawSize);
                if (player.direction > 0)
                    HitBox.X -= HitBox.Width;
                HitBox.X -= (int)(8 * player.direction * PawSize);
                HitBox.Y += (int)(8 * PawSize);
            }
            else if (AttackPercentage < 0.66f)
            {
                HitBox.Width = (int)(40 * 1.2f * 2 * PawSize);
                HitBox.Height = (int)(40 * 1.2f * 2 * PawSize);
                if (player.direction < 0)
                    HitBox.X -= HitBox.Width;
                HitBox.X -= (int)(8 * player.direction * PawSize);
                HitBox.Y += (int)(8 * PawSize);
            }
            else if (AttackPercentage < 0.66f)
            {
                HitBox.Width = (int)(40 * 1.2f * 2 * PawSize);
                HitBox.Height = (int)(80 * 1.2f * 2 * PawSize);
                if (player.direction < 0)
                    HitBox.X -= HitBox.Width;
                HitBox.X -= (int)(8 * player.direction * PawSize);
                HitBox.Y += (int)(8 * PawSize);
            }
            float DamagePercentage = 1.20f + 0.44f * data.Level;
            //int Damage = data.GetMeleeDamage(0, 1.20f + 0.44f * data.Level, player);
            for(int i = 0; i < 200; i++)
            {
                if(Main.npc[i].active && !Main.npc[i].friendly && !Main.npc[i].dontTakeDamage && Main.npc[i].getRect().Intersects(HitBox))
                {
                    data.HurtNpc(player, Main.npc[i], DamageTypes.Melee, DamagePercentage, player.direction, 3.6f, 30);
                }
            }
        }

        public override void Draw(Player player, SkillData data, PlayerDrawInfo pdi)
        {
            float PawSize = GetPawSize(data);
            Vector2 Origin = new Vector2(8, 10);
            if (player.direction < 0)
                Origin.X = 80 - Origin.X;
            float AttackPercentage = data.Time * (1f / AttackDuration);
            AttackPercentage *= AttackPercentage;
            if (AttackPercentage > 1)
                AttackPercentage = 1f;
            float Rotation = (-2.356194f + (float)Math.PI * AttackPercentage) * player.direction;
            Terraria.DataStructures.DrawData dd = new Terraria.DataStructures.DrawData(MainMod.CerberusPawSprite.GetTexture, player.Center - Main.screenPosition, null, Color.White, Rotation,
                Origin, PawSize, pdi.spriteEffects, 0);
            Main.playerDrawData.Insert(0, dd);
        }
    }
}
