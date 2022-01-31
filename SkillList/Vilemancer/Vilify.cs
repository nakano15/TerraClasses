using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace TerraClasses.SkillList.Vilemancer
{
    class Vilify : SkillBase
    {
        public Vilify()
        {
            Name = "Vilify";
            Description = "Spray vile powder around you, banishing hallow \n" +
                "and corrupting critters.\n" +
                " Power causes 66% + 2.7% per level magic damage.";
            MaxLevel = 10;
            Cooldown = 40;
            skillType = Enum.SkillTypes.Active;
        }

        public override void Update(Player player, SkillData data)
        {
            if(data.Time >= 5)
            {
                int Damage = data.GetMagicDamage(0, 0.66f + 0.027f * data.Level, player);
                for (int i = 0; i < 4; i++)
                {
                    float Rotation = Microsoft.Xna.Framework.MathHelper.ToRadians(Main.rand.NextFloat() * 360);
                    Microsoft.Xna.Framework.Vector2 Direction = new Microsoft.Xna.Framework.Vector2((float)Math.Sin(Rotation), (float)Math.Cos(Rotation));//new Microsoft.Xna.Framework.Vector2(1f, 0);
                    /*Direction.Y = Direction.X * Main.rand.NextFloat();
                    Direction.X -= Direction.Y;
                    if (Main.rand.NextFloat() < 0.5f)
                        Direction.X *= -1;
                    if (Main.rand.NextFloat() < 0.5f)
                        Direction.Y *= -1;*/
                    Projectile.NewProjectile(player.Center, Direction * 6, Terraria.ID.ProjectileID.VilePowder, Damage, 1f, player.whoAmI);
                }
                if(data.Step >= data.Level + 10)
                {
                    data.EndUse();
                }
                else
                {
                    data.ChangeStep();
                }
            }
        }
    }
}
