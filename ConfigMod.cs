using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;

namespace TerraClasses
{
    [Label("Personal Configuration")]
    public class ConfigMod : ModConfig
    {
        public override ConfigScope Mode
        {
            get { return ConfigScope.ClientSide; }
        }

        [Label("Level Exp Bar Offset")]
        [Tooltip("This allows you to change the position of the exp bar in pixels. Useful if the interface is overlapping another mod interface. Negative numbers moves the exp bar to left(x) or top(y), while positive numbers moves It right(x) or bottom(y).")]
        [Range(-1f, 1f)]
        [Increment(0.01f)]
        public Vector2 ExpBarOffset { get { return MainMod.ExpBarOffset; } set { MainMod.ExpBarOffset = value; } }
    }

    [Label("Debugging Settings")]
    public class DebugConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Label("Enable Debug Mode")]
        [Tooltip("Turns on debug mode, giving access to all classes and skills. Can only be turned on in the main menu.")]
        public bool DebugMode
        {
            get
            {
                return MainMod.DebugMode;
            }
            set
            {
                if (Main.gameMenu)
                    MainMod.DebugMode = value;
            }
        }
    }
}
