using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace TerraClasses.SkillList.Fighter
{
    public class TrustyLongsword : SkillBase
    {
        const int TextureDimension = 50;
        const int MaxDuration = 45;
        Vector2 GetOrigin { get { return new Vector2(50 - 2, 50 - 2); } }
        public TrustyLongsword()
        {
            Name = "Side Sword";
            Description = "Swings a longsword, inflicting damage to nearby enemies.\n" +
                " Inflicts 140% + 20% per level of melee damage.\n" +
                " Level increases sword size by 15%.";
            Cooldown = GetTime(28);
            skillType = Enum.SkillTypes.Active;
            MaxLevel = 10;
        }

        public float RotationFromSkillTime(int Time)
        {
            return 3.141592653589793f * ((float)Time / MaxDuration);
        }

        public override void Update(Terraria.Player player, SkillData data)
        {
            if (data.Time >= MaxDuration)
                data.EndUse();
            else
            {
                int AttackSize = (int)(48 * (1f + 0.1f * data.Level));
                Rectangle rect = new Rectangle((int)player.Center.X, (int)player.Center.Y, AttackSize, AttackSize);
                if (data.Time <= MaxDuration * 0.333f)
                {
                    if(player.direction > 0)rect.X -= rect.Width;
                    rect.Y -= rect.Height;
                }
                else if (data.Time <= MaxDuration * 0.667f)
                {
                    if (player.direction < 0) rect.X -= rect.Width;
                    rect.Y -= rect.Height;
                }
                else
                {
                    if (player.direction < 0) rect.X -= rect.Width;
                }
                rect.X += player.direction * 4;
                float DamageMult = 1.4f + 0.2f * data.Level;
                //int Damage = data.GetMeleeDamage(0, 1.4f + 0.2f * data.Level, player);
                for (int x = 0; x < 255; x++)
                {
                    if (player.hostile && x != player.whoAmI && Main.player[x].active && !Main.player[x].dead && Main.player[x].hostile && (Main.player[x].team == 0 || Main.player[x].team != player.team) && Main.player[x].getRect().Intersects(rect))
                    {
                        data.HurtPlayer(player, Main.player[x], DamageTypes.Melee, DamageMult, player.direction, 60, false);
                        //Hurt player
                    }
                    if (x < 200 && Main.npc[x].active && !Main.npc[x].friendly && !Main.npc[x].townNPC && !Main.npc[x].dontTakeDamage && Main.npc[x].getRect().Intersects(rect))
                    {
                        data.HurtNpc(player, Main.npc[x], DamageTypes.Melee, DamageMult, player.direction, player.inventory[player.selectedItem].knockBack, 60, false);
                        //Hurt npc
                    }
                }
            }
        }

        public override void Draw(Terraria.Player player, SkillData data, Terraria.ModLoader.PlayerDrawInfo pdi)
        {
            float Rotation = RotationFromSkillTime(data.Time) * player.direction;
            Vector2 Origin = GetOrigin;
            if (player.direction < 0)
                Origin.X = 50 - Origin.X;
            Vector2 SkillPosition = player.Center;
            SkillPosition.X += 4f * player.direction;
            //SkillPosition.Y -= 8f;
            Terraria.DataStructures.DrawData dd = new Terraria.DataStructures.DrawData(MainMod.Longsword.GetTexture, SkillPosition - Main.screenPosition, null, pdi.eyeWhiteColor,Rotation, Origin, 1f + data.Level * 0.15f, (player.direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None), 0);
            Main.playerDrawData.Insert(0, dd);
        }
    }
}
