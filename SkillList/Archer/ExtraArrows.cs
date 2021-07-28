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

        public override SkillData GetSkillData => new ExtraArrowsData();

        public override bool BeforeShooting(Player player, SkillData rawdata, Item weapon, ref int type, ref int damage, ref float knockback, ref Vector2 Position, ref float SpeedX, ref float SpeedY)
        {
            ExtraArrowsData data = (ExtraArrowsData)rawdata;
            if (weapon.useAmmo == Terraria.ID.AmmoID.Arrow)
            {
                data.AttackCount ++;
                if (data.AttackCount > 3)
                {
                    data.AttackCount = 1;
                }
                for (int i = 0; i < data.AttackCount; i++)
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

        public class ExtraArrowsData : SkillData
        {
            public byte AttackCount = 0;
        }
    }
}
