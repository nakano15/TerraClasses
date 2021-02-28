using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace TerraClasses.SkillList.Mage
{
    public class MeteorBarrage : SkillBase
    {
        public MeteorBarrage()
        {
            Name = "Meteor Barrage";
            Description = "Invokes a meteor barrage at the targetted position.\n" +
                " Each meteor causes 140% + 12% * Level of magic damage.";
            MaxLevel = 10;
            Cooldown = GetCooldown(25, 1);
            skillType = Enum.SkillTypes.Active;
        }

        public override void Update(Terraria.Player player, SkillData data)
        {
            const byte SpawnXVar = 0, SpawnYVar = 1;
            if (data.Time == 0)
            {
                data.SetFloat(SpawnXVar, Main.screenPosition.X + Main.mouseX);
                data.SetFloat(SpawnYVar, Main.screenPosition.Y + Main.mouseY);
            }
            if (data.Time % 10 == 0)
            {
                const float ShotSpeed = 20;
                float SpawnX = data.GetFloat(SpawnXVar), SpawnY = data.GetFloat(SpawnYVar);
                float CenterX = SpawnX, CenterY = SpawnY;
                SpawnY -= 1000;
                SpawnX += Main.rand.Next(-300, 301);
                Vector2 ShotDir = new Vector2(CenterX - SpawnX, CenterY - SpawnY);
                ShotDir.Normalize();
                Projectile.NewProjectile(new Vector2(SpawnX, SpawnY), ShotDir * ShotSpeed, Terraria.ID.ProjectileID.Meteor1 + Main.rand.Next(3), data.GetMagicDamage(0, 1.4f + 0.12f * data.Level, player), 4f, player.whoAmI);
            }
            if (data.Time / 10 >= data.Level)
                data.EndUse();
        }
    }
}
