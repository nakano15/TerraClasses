using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.SkillList.Sharpshooter
{
    public class Flintlock : SkillBase
    {
        const int FlintlockFrame = 9;

        public Flintlock()
        {
            Name = "Flintlock";
            Description = "Takes on a stance to better use guns.\n" +
                " Ranged damage increases by 8% per level.\n" +
                " Ranged critical rate increased by 8%.\n" +
                " Knockback increased by 15%.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Passive;
        }

        public bool FlintlockTriggered(Terraria.Player player)
        {
            return player.velocity.X == 0 && player.velocity.Y == 0 && player.inventory[player.selectedItem].ammo == Terraria.ID.AmmoID.Bullet;
        }

        public override void UpdateStatus(Terraria.Player player, SkillData data)
        {
            if (FlintlockTriggered(player))
            {
                player.rangedDamage += 0.08f * data.Level;
                player.rangedCrit += 8;
            }
        }

        public override bool BeforeShooting(Terraria.Player player, SkillData data, Terraria.Item weapon, ref int type, ref int damage, ref float knockback, ref Microsoft.Xna.Framework.Vector2 Position, ref float SpeedX, ref float SpeedY)
        {
            if (FlintlockTriggered(player))
            {
                knockback *= 1.15f;
            }
            return true;
        }

        public override void UpdateAnimation(Terraria.Player player, SkillData data)
        {
            if (FlintlockTriggered(player) && player.itemAnimation > 0)
            {
                player.legFrame.Y = player.legFrame.Height * FlintlockFrame;
            }
        }
    }
}
