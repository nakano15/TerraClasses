using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TerraClasses.SkillList.Vilemancer
{
    class Vilemancy : SkillBase
    {
        public Vilemancy()
        {
            Name = "Vilemancy";
            Description = "Empowers your vile powers.\n" +
                " Eater of Souls created from this gets stronger.";
            MaxLevel = 10;
        }

        public override SkillData GetSkillData => new VilemancyData();

        public override void Update(Player player, SkillData rawdata)
        {
            VilemancyData data = (VilemancyData)rawdata;
            foreach(EaterOfSouls eater in data.Eaters)
            {

            }
        }

        public class VilemancyData : SkillData
        {
            public List<EaterOfSouls> Eaters = new List<EaterOfSouls>();
        }

        public class EaterOfSouls
        {
            public Vector2 Position = Vector2.Zero, Velocity = Vector2.Zero;
            public float LifeTime = 0;
        }
    }
}
