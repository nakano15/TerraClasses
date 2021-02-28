using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace TerraClasses.SkillList.Fighter
{
    public class DoubleArrows : SkillBase
    {
        public DoubleArrows()
        {
            Name = "Double Arrows";
            Description = "Shots two arrows at once.\n" +
                " Arrows causes 60% + 2% per level of their damages.";
            skillType = Enum.SkillTypes.Attack;
            MaxLevel = 10;
        }

        public override bool BeforeShooting(Terraria.Player player, SkillData data, Terraria.Item weapon, ref int type, ref int damage, ref float knockback, ref Microsoft.Xna.Framework.Vector2 Position, ref float SpeedX, ref float SpeedY)
        {
            damage = (int)(damage * (0.6f + 0.02f * data.Level));
            if (weapon.ranged && MainMod.IsArrow(type))
            {
                Projectile.NewProjectile(Position.X, Position.Y, SpeedX, SpeedY - 0.4f, type, damage, knockback, player.whoAmI);
            }
            return true;
        }
    }
}
