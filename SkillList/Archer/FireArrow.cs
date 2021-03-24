using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.SkillList.Archer
{
    public class FireArrow : SkillBase
    {
        public FireArrow()
        {
            Name = "Fire Arrow";
            Description = "For 45 seconds, Wooden Arrows shoot are turned into Fire Arrows.";
            skillType = Enum.SkillTypes.Active;
            Cooldown = GetTime(15);
        }

        public override void Update(Terraria.Player player, SkillData data)
        {
            if (data.Time >= 45 * 60)
                data.EndUse();
        }

        public override bool BeforeShooting(Terraria.Player player, SkillData data, Terraria.Item weapon, ref int type, ref int damage, ref float knockback, ref Microsoft.Xna.Framework.Vector2 Position, ref float SpeedX, ref float SpeedY)
        {
            if (type == Terraria.ID.ProjectileID.WoodenArrowFriendly)
                type = Terraria.ID.ProjectileID.FireArrow;
            return true;
        }
    }
}
