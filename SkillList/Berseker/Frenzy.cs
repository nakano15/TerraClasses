using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;

namespace TerraClasses.SkillList.Berseker
{
    public class Frenzy : SkillBase
    {
        public Frenzy()
        {
            Name = "Frenzy";
            Description = "Enter a frenzied state, attacking full power anything in your way.\nAll Damages increases by 150%.\nDefense drops to 0.\nAttack speed greatly increased.\nLasts 20 + 1 seconds per level.\nWhen skill is at level 6+:\n +5 Seconds duration.\n Causes a explosion around your character,\n  inflicting 60% melee damage to all nearby foes.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Active;
            Cooldown = GetCooldown(0, 5);
        }

        public override void UpdateStatus(Terraria.Player player, SkillData data)
        {
            player.meleeDamage += (player.meleeDamage - 1) * 0.5f;
            player.rangedDamage += (player.rangedDamage - 1) * 0.5f;
            player.magicDamage += (player.magicDamage - 1) * 0.5f;
            player.minionDamage += (player.minionDamage - 1) * 0.5f;
            player.statDefense = 0;
            player.meleeSpeed += 1f;
        }

        public override void Update(Terraria.Player player, SkillData data)
        {
            if (data.Time == 0 && data.Level >= 6)
            {
                //Plays explosion effect.if (this.type == 29)
                Vector2 position = player.Center;
                Main.PlaySound(Terraria.ID.SoundID.Item14, position);
                int width = 200, height = 200;
                position.X -= width / 2;
                position.Y -= height / 2;
                for (int index1 = 0; index1 < 50; ++index1)
                {
                    int index2 = Dust.NewDust(position, width, height, 31, 0.0f, 0.0f, 100, new Color(), 2f);
                    Main.dust[index2].velocity *= 1.4f;
                }
                for (int index1 = 0; index1 < 80; ++index1)
                {
                    int index2 = Dust.NewDust(position, width, height, 6, 0.0f, 0.0f, 100, new Color(), 3f);
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 5f;
                    int index3 = Dust.NewDust(position, width, height, 6, 0.0f, 0.0f, 100, new Color(), 2f);
                    Main.dust[index3].velocity *= 3f;
                }
                for (int index1 = 0; index1 < 2; ++index1)
                {
                    int index2 = Gore.NewGore(new Vector2((float)((double)position.X + (double)(width / 2) - 24.0), (float)((double)position.Y + (double)(height / 2) - 24.0)), new Vector2(), Main.rand.Next(61, 64), 1f);
                    Main.gore[index2].scale = 1.5f;
                    Main.gore[index2].velocity.X += 1.5f;
                    Main.gore[index2].velocity.Y += 1.5f;
                    int index3 = Gore.NewGore(new Vector2((float)((double)position.X + (double)(width / 2) - 24.0), (float)((double)position.Y + (double)(height / 2) - 24.0)), new Vector2(), Main.rand.Next(61, 64), 1f);
                    Main.gore[index3].scale = 1.5f;
                    Main.gore[index3].velocity.X -= 1.5f;
                    Main.gore[index3].velocity.Y += 1.5f;
                    int index4 = Gore.NewGore(new Vector2((float)((double)position.X + (double)(width / 2) - 24.0), (float)((double)position.Y + (double)(height / 2) - 24.0)), new Vector2(), Main.rand.Next(61, 64), 1f);
                    Main.gore[index4].scale = 1.5f;
                    Main.gore[index4].velocity.X += 1.5f;
                    Main.gore[index4].velocity.Y -= 1.5f;
                    int index5 = Gore.NewGore(new Vector2((float)((double)position.X + (double)(width / 2) - 24.0), (float)((double)position.Y + (double)(height / 2) - 24.0)), new Vector2(), Main.rand.Next(61, 64), 1f);
                    Main.gore[index5].scale = 1.5f;
                    Main.gore[index5].velocity.X -= 1.5f;
                    Main.gore[index5].velocity.Y -= 1.5f;
                }
                Rectangle rect = new Rectangle((int)position.X, (int)position.Y, width, height);
                int Damage = data.GetMeleeDamage(0, 0.6f, player);
                for (int i = 0; i < 200; i++)
                {
                    if (Main.npc[i].active && !Main.npc[i].friendly && !Main.npc[i].dontTakeDamage && Main.npc[i].getRect().Intersects(rect))
                    {
                        int direction = Main.npc[i].Center.X - player.Center.X < 0 ? -1 : 1;
                        Main.npc[i].StrikeNPC(Damage, 2f, direction, true);
                    }
                }
            }
            int MaxDuration = 20 + data.Level;
            if (data.Level > 5)
                MaxDuration += 5;
            if (data.Time >= MaxDuration * 60)
            {
                data.EndUse();
            }
        }
    }
}
