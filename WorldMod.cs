using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace TerraClasses
{
    public class WorldMod : ModWorld
    {
        public override Terraria.ModLoader.IO.TagCompound Save()
        {
            MainMod.SaveLoreDatas();
            return base.Save();
        }

        public override void Load(Terraria.ModLoader.IO.TagCompound tag)
        {
            base.Load(tag);
        }

        public override void Initialize()
        {
            MainMod.LoadLoreDatas();
        }
    }
}
