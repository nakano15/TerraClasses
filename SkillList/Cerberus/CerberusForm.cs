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
            Description = "Unlocks Cerberus forms. Demons are friendly to you.";
            skillType = Enum.SkillTypes.Passive;
            MaxLevel = 1;
        }

        private void DarkenColor(ref Color color)
        {
            color.R = (byte)(color.R * 0.3f);
            color.G = (byte)(color.G * 0.3f);
            color.B = (byte)(color.B * 0.3f);
        }

        public override void Update(Player player, SkillData data)
        {
            player.npcTypeNoAggro[Terraria.ID.NPCID.Demon] = player.npcTypeNoAggro[Terraria.ID.NPCID.VoodooDemon] = player.npcTypeNoAggro[Terraria.ID.NPCID.RedDevil] = true;
            for (byte i = 0; i < 3; i++)
            {
                if (data.GetInteger(i) > 0)
                {
                    data.ChangeInteger(i, -1);
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

        public override void Draw(Player player, SkillData data, Terraria.ModLoader.PlayerDrawInfo pdi)
        {
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
                        if (data.GetInteger(i) > 0)
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
                    HeadPosition = player.head;
                }
                else if ((!pdi.drawAltHair && Main.playerDrawData[t].texture == Main.playerHairTexture[pdi.drawPlayer.hair]) || Main.playerDrawData[t].texture == Main.playerHairAltTexture[pdi.drawPlayer.hair] || pdi.drawPlayer.head > 0 && Main.playerDrawData[t].texture == Main.armorHeadTexture[pdi.drawPlayer.head])
                {
                    /*Terraria.DataStructures.DrawData ndd;
                    Rectangle rect;
                    rect = new Rectangle(dd.sourceRect.Value.X, dd.sourceRect.Value.Y, dd.sourceRect.Value.Width, dd.sourceRect.Value.Height);
                    rect.X += 3 * 40;
                    Color c = dd.color;
                    DarkenColor(ref c);
                    ndd = new Terraria.DataStructures.DrawData(CerberusTexture, dd.position, rect, c, dd.rotation, dd.origin, dd.scale, dd.effect, 0);
                    Main.playerDrawData.Insert(t + 1, ndd);
                    t++;*/
                    Terraria.DataStructures.DrawData ndd = Main.playerDrawData[t];
                    Main.playerDrawData.RemoveAt(t);
                    //if (HeadPosition > -1)
                    //    Main.playerDrawData.Insert(HeadPosition, ndd);
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
}
