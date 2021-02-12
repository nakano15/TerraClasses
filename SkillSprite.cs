using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace TerraClasses
{
    public class SkillSprite
    {
        public Texture2D Texture;
        public string SpriteDir = "";
        public bool LoadFailed = false;

        public SkillSprite(string Dir)
        {
            SpriteDir = Dir;
        }

        public Texture2D GetTexture
        {
            get
            {
                if (LoadFailed)
                    return null;
                if (Texture == null)
                {
                    if (MainMod.TextureExists(SpriteDir))
                        Texture = MainMod.LoadTexture(SpriteDir);
                    else
                        LoadFailed = true;
                }
                return Texture;
            }
        }
    }
}
