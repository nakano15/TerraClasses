using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.SkillList.Mage
{
    public class MagicArrows : SkillBase
    {
        public MagicArrows()
        {
            Name = "Magic Arrows";
            Description = "Makes Wooden Arrows be turned into Jester Arrows.";
            skillType = Enum.SkillTypes.Attack;
        }

        public override bool BeforeShooting(Terraria.Player player, SkillData data, Terraria.Item weapon, ref int type, ref int damage, ref float knockback, ref Microsoft.Xna.Framework.Vector2 Position, ref float SpeedX, ref float SpeedY)
        {
            if (type == Terraria.ID.ProjectileID.WoodenArrowFriendly)
                type = Terraria.ID.ProjectileID.JestersArrow;
            return true;
        }
    }
}
