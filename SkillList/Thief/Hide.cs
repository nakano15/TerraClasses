using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace TerraClasses.SkillList.Thief
{
    public class Hide : SkillBase
    {
        public Hide()
        {
            Name = "Hide";
            Description = "Hides yourself on the ground, avoiding attacks and attracting attention.\n" +
                " Moving or jumping cancels this effect.";
            MaxLevel = 1;
            skillType = Enum.SkillTypes.Active;
            Cooldown = GetTime(45);
        }

        public override void Update(Player player, SkillData data)
        {
            player.immuneTime = 5;
            player.immuneAlpha = 255;
            player.aggro -= 900;
            if (player.controlRight || player.controlLeft || player.controlJump)
                data.EndUse(false);
        }

        public override void Draw(Player player, SkillData data, PlayerDrawInfo pdi)
        {
            Microsoft.Xna.Framework.Vector2 Position = player.position;
            Position.X += player.width * 0.5f - 10;
            Position.Y += player.height - 4;
            Terraria.DataStructures.DrawData dd = new Terraria.DataStructures.DrawData(MainMod.HideEffect.GetTexture, Position - Main.screenPosition, 
                null, Microsoft.Xna.Framework.Color.White);
            Main.playerDrawData.Add(dd);
        }
    }
}
