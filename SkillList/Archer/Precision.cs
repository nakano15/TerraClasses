using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerraClasses.SkillList.Archer
{
    public class Precision : SkillBase
    {
        public Precision()
        {
            Name = "Precision";
            Description = "The lower your move speed, the higher precision you get.\n" +
                " Precision increases your arrow damage by 6% per level based on the precision percentage.\n" +
                " Moving too fast reduces the percentage.\n" +
                " Past level 5, each level gives 3% Critical Rate";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Passive;
        }

        public override void UpdateStatus(Player player, SkillData rawdata)
        {
            PrecisionData data = (PrecisionData)rawdata;
            float Boost = data.ChargePercent * 0.01f;
            player.rangedDamage += Boost * 0.06f * data.Level;
            if (data.Level > 5)
                player.rangedCrit += (int)((data.Level - 5) * 3 * Boost);
        }

        public override void Update(Player player, SkillData rawdata)
        {
            PrecisionData data = (PrecisionData)rawdata;
            float ChargeValue = ((float)Math.Abs(player.velocity.X) / 1.5f - 1f) * -0.25f;
            ChargeValue *= 0.5f;
            float Boost = data.ChargePercent + ChargeValue;
            if (Boost > 100) Boost = 100;
            if (Boost < 0) Boost = 0;
            data.ChargePercent = Boost;
        }

        public override void Draw(Player player, SkillData rawdata, PlayerDrawInfo pdi)
        {
            if (Main.gameMenu) return;
            PrecisionData data = (PrecisionData)rawdata;
            Vector2 DrawPosition = new Vector2(Main.screenWidth * 0.25f, Main.screenHeight * 0.75f);
            Utils.DrawBorderString(Main.spriteBatch, "Precision: " + (int)data.ChargePercent + "%", DrawPosition, Color.Blue, 0.75f, 0.5f, 0.5f);
        }

        public override SkillData GetSkillData => new PrecisionData();

        public class PrecisionData : SkillData
        {
            public float ChargePercent = 0;
        }
    }
}
