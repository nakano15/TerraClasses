using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;

namespace TerraClasses.SkillList.Archer
{
    public class ExtraArrows : SkillBase
    {
        const byte IAttackCountVar = 0;
        public ExtraArrows()
        {
            Name = "Extra Arrows";
            Description = "Shoots extra arrows rhythimically.\n" +
                " One, Two, Three, One, Two, Three.\n" +
                " ArrowDamage is of 60 + 8% per level.\n" +
                " Second Arrow inflicts 80% of damage, third 60%.\n" +
                " Only shoots arrows when using any arrow using weapons.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Attack;
        }

        public override bool BeforeShooting(Player player, SkillData data, Item weapon, ref int type, ref int damage, ref float knockback, ref Vector2 Position, ref float SpeedX, ref float SpeedY)
        {
            if (weapon.useAmmo == Terraria.ID.AmmoID.Arrow)
            {
                data.ChangeInteger(IAttackCountVar, 1);
                int Count = data.GetInteger(IAttackCountVar);
                if (Count > 3)
                {
                    data.SetInteger(IAttackCountVar, 1);
                    Count = 1;
                }
                for (int i = 0; i < Count; i++)
                {
                    float ThisSpeedX = SpeedX, ThisSpeedY = SpeedY;
                    int Damage = (int)(damage * (0.6f + 0.08f * data.Level));
                    if (i == 1)
                    {
                        ThisSpeedY -= 0.4f;
                        Damage = (int)(Damage * 0.8f);
                    }
                    if (i == 2)
                    {
                        ThisSpeedY += 0.4f;
                        Damage = (int)(Damage * 0.6f);
                    }
                    int proj = Projectile.NewProjectile(Position.X, Position.Y, ThisSpeedX, ThisSpeedY, type, Damage, 0.6f, player.whoAmI);
                    Main.projectile[proj].noDropItem = true;
                }
            }
            return true;
        }
    }
}
