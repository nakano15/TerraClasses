using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace TerraClasses.SkillList.Cerberus
{
    public class CerberusForm : SkillBase
    {
        public CerberusForm()
        {
            Name = "Cerberus Form";
            Description = "Changes yourself to a Cerberus.\n" +
                " Demons are friendly towards you, except Fire Imps.\n" +
                " Immunity to Lava.\n" +
                " Lava blocks doesn't burn you.\n" +
                " On Fire! debuff now heals you.";
            skillType = Enum.SkillTypes.Passive;
            MaxLevel = 1;
        }

        public override SkillData GetSkillData => new CerberusFormData();

        public static Vector2 GetMouthPosition(Player player, bool XPositionToo = false)
        {
            Vector2 ShotSpawnPosition = player.Center;
            ShotSpawnPosition.Y -= 8 * player.gravDir;
            if (XPositionToo)
            {
                ShotSpawnPosition.X += 12 * player.direction;
            }
            return ShotSpawnPosition;
        }

        private void DarkenColor(ref Color color)
        {
            color.R = (byte)(color.R * 0.3f);
            color.G = (byte)(color.G * 0.3f);
            color.B = (byte)(color.B * 0.3f);
        }

        public override void UpdateStatus(Player player, SkillData data)
        {
            player.fireWalk = true;
        }

        public override void Update(Player player, SkillData data)
        {
            player.npcTypeNoAggro[Terraria.ID.NPCID.Demon] = player.npcTypeNoAggro[Terraria.ID.NPCID.VoodooDemon] = player.npcTypeNoAggro[Terraria.ID.NPCID.RedDevil] = true;
            CerberusFormData cfb = (CerberusFormData)data;
            for (byte i = 0; i < 3; i++)
            {
                if (cfb.HeadFrame[i] > 0)
                {
                    cfb.HeadFrame[i]--;
                }
            }
            if (player.HasBuff(Terraria.ID.BuffID.OnFire))
            {
                player.lifeRegen += 8;
                cfb.FireHealingTimer++;
                if(cfb.FireHealingTimer >= 10)
                {
                    cfb.FireHealingTimer -= 10;
                    if(player.statLife < player.statLifeMax2)
                    {
                        int HealthRegen = player.statLifeMax2 / player.statLifeMax;
                        if (HealthRegen < 1)
                            HealthRegen = 1;
                        player.statLifeMax2 += HealthRegen;
                        CombatText.NewText(player.getRect(), CombatText.HealLife, HealthRegen, false, true);
                    }
                }
                else
                {
                    cfb.FireHealingTimer = 0;
                }
            }
        }

        private Rectangle RevampRect(Rectangle Original)
        {
            if (true || Original.Y < 0)
            {
                Rectangle rect = new Rectangle(Original.X, Original.Y, Original.Width, Original.Height);
                if (rect.Y == 1064) rect.Y = 0;
                //rect.Y += 336;
                return rect;
            }
            return Original;
        }

        public override void Draw(Player player, SkillData rawdata, Terraria.ModLoader.PlayerDrawInfo pdi)
        {
            CerberusFormData data = (CerberusFormData)rawdata;
            if (player.velocity.X != 0 && player.velocity.Y == 0)
            {
                for (int i = 0; i < 1; i++)
                {
                    Vector2 bottom = player.Bottom;
                    bottom.X -= 4 - Main.rand.Next(8);
                    bottom.Y--;
                    int dust = Dust.NewDust(bottom, 8, 2, Terraria.ID.DustID.Fire);
                    Main.dust[dust].noLight = true;
                }
            }
            Texture2D CerberusTexture = MainMod.CerberusSprite.GetTexture;
            int HeadPosition = -1;
            for (int t = 0; t < Main.playerDrawData.Count; t++)
            {
                Terraria.DataStructures.DrawData dd = Main.playerDrawData[t];
                if (Main.playerDrawData[t].texture == Main.playerTextures[player.skinVariant, Terraria.ID.PlayerTextureID.Hands] ||
                    Main.playerDrawData[t].texture == Main.playerTextures[player.skinVariant, Terraria.ID.PlayerTextureID.ArmHand] ||
                    Main.playerDrawData[t].texture == Main.playerTextures[player.skinVariant, Terraria.ID.PlayerTextureID.ArmSkin] ||
                    Main.playerDrawData[t].texture == Main.playerTextures[player.skinVariant, Terraria.ID.PlayerTextureID.LegSkin] ||
                    Main.playerDrawData[t].texture == Main.playerTextures[player.skinVariant, Terraria.ID.PlayerTextureID.TorsoSkin])
                {
                    Color c = dd.color;
                    DarkenColor(ref c);
                    Terraria.DataStructures.DrawData ndd = new Terraria.DataStructures.DrawData(dd.texture, dd.position, dd.sourceRect, c, dd.rotation, dd.origin, dd.scale, dd.effect, 0);
                    Main.playerDrawData[t] = ndd;
                }
                if (Main.playerDrawData[t].texture == Main.playerTextures[player.skinVariant, Terraria.ID.PlayerTextureID.Head])
                {
                    Terraria.DataStructures.DrawData ndd;
                    Rectangle rect;
                    for (byte i = 0; i < 3; i++)
                    {
                        rect = RevampRect(dd.sourceRect.Value);
                        switch (i)
                        {
                            case 0:
                                rect.X += 2 * 40;
                                break;
                            case 1:
                                rect.X += 3 * 40;
                                break;
                            case 2:
                                rect.X += 1 * 40;
                                break;
                        }
                        if (data.HeadFrame[i] > 0)
                        {
                            rect.Y = 19 * rect.Height;
                        }
                        Color c = dd.color;
                        DarkenColor(ref c);
                        ndd = new Terraria.DataStructures.DrawData(CerberusTexture, dd.position, rect, c, dd.rotation, dd.origin, dd.scale, dd.effect, 0);
                        switch (i)
                        {
                            case 0:
                                Main.playerDrawData[t] = ndd;
                                break;
                            case 1:
                                Main.playerDrawData.Insert(t + 1, ndd);
                                break;
                            case 2:
                                Main.playerDrawData.Insert(t - 1, ndd);
                                break;
                        }
                        HeadPosition = t + 1;
                    }
                }
                else if (player.head > 0 && Main.playerDrawData[t].texture == Main.armorHeadTexture[player.head])
                {
                    Main.playerDrawData.RemoveAt(t);
                    //HeadPosition = player.head;
                }
                else if ((!pdi.drawAltHair && Main.playerDrawData[t].texture == Main.playerHairTexture[pdi.drawPlayer.hair]) || Main.playerDrawData[t].texture == Main.playerHairAltTexture[pdi.drawPlayer.hair] || pdi.drawPlayer.head > 0 && Main.playerDrawData[t].texture == Main.armorHeadTexture[pdi.drawPlayer.head])
                {
                    Terraria.DataStructures.DrawData ndd = Main.playerDrawData[t];
                    Main.playerDrawData.RemoveAt(t);
                }
                else if (Main.playerDrawData[t].texture == Main.playerTextures[player.skinVariant, Terraria.ID.PlayerTextureID.Eyes])
                {
                    Rectangle rect = RevampRect(new Rectangle(dd.sourceRect.Value.X + 4 * 40, dd.sourceRect.Value.Y, dd.sourceRect.Value.Width, dd.sourceRect.Value.Height));
                    Terraria.DataStructures.DrawData ndd = new Terraria.DataStructures.DrawData(CerberusTexture, dd.position, rect, dd.color, dd.rotation, dd.origin, dd.scale, dd.effect, 0);
                    Main.playerDrawData[t] = ndd;
                }
                else if (Main.playerDrawData[t].texture == Main.playerTextures[player.skinVariant, Terraria.ID.PlayerTextureID.EyeWhites])
                {
                    Rectangle rect = RevampRect(new Rectangle(dd.sourceRect.Value.X + 5 * 40, dd.sourceRect.Value.Y, dd.sourceRect.Value.Width, dd.sourceRect.Value.Height));
                    Terraria.DataStructures.DrawData ndd = new Terraria.DataStructures.DrawData(CerberusTexture, dd.position, rect, dd.color, dd.rotation, dd.origin, dd.scale, dd.effect, 0);
                    Main.playerDrawData[t] = ndd;
                }
                else if (Main.playerDrawData[t].texture == Main.playerTextures[player.skinVariant, Terraria.ID.PlayerTextureID.LegSkin])
                {
                    Rectangle rect = RevampRect(new Rectangle(dd.sourceRect.Value.X, dd.sourceRect.Value.Y, dd.sourceRect.Value.Width, dd.sourceRect.Value.Height));
                    Color c = dd.color;
                    DarkenColor(ref c);
                    Terraria.DataStructures.DrawData ndd = new Terraria.DataStructures.DrawData(CerberusTexture, dd.position, rect, c, dd.rotation, dd.origin, dd.scale, dd.effect, 0);
                    Main.playerDrawData.Insert(t, ndd);
                    t++;
                }
            }
        }
    }

    public class CerberusFormData : SkillData
    {
        public byte[] HeadFrame = new byte[] { 0, 0, 0 };
        public byte FireHealingTimer = 0;
    }
}
