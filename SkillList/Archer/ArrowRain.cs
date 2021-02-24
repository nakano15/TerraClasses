using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace TerraClasses.SkillList.Archer
{
    public class ArrowRain : SkillBase
    {
        public ArrowRain()
        {
            Name = "Arrow Rain";
            Description = "Shoot arrows to the sky, making them rain around.\nUpward shoot causes 120% damage, falling shoots causes 70%.\nAttack damage of 80% + 3% per level.\nArrows falls for 7 seconds.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Active;
            Cooldown = GetCooldown(22);
        }

        public int GetDamage(Player player, SkillData data, bool UpwardShot)
        {
            int HighestDamage = 0;
            for (int i = 0; i < 10; i++)
            {
                int ItemDamage = player.inventory[i].damage;
                if(!player.inventory[i].ranged)
                    ItemDamage = (int)(ItemDamage * 0.75f);
                if (ItemDamage > HighestDamage)
                    HighestDamage = ItemDamage;
            }
            return (int)(HighestDamage * player.rangedDamage * (UpwardShot ? 1.2f : 0.7f) * (0.8f + data.Level * 0.03f));
        }

        public override void Update(Terraria.Player player, SkillData data)
        {
            if (data.Time == 0 && !player.inventory.Take(10).Any(x => x.useAmmo == Terraria.ID.AmmoID.Arrow))
            {
                CombatText.NewText(player.getRect(), Color.Red, "You don't have a bow type weapon.");
                data.EndUse(true);
                return;
            }
            const byte RainPosXVar = 0;
            switch (data.Step)
            {
                case 0:
                    {
                        if (data.LastTime == 0)
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                if (player.inventory[i].useAmmo == Terraria.ID.AmmoID.Arrow)
                                {
                                    FakeWeaponUsage(player, i, player.Center - new Vector2(0, -100));
                                    break;
                                }
                            }
                            Vector2 ShotPos = player.Center;
                            for (int x = 1; x <= 6; x++)
                            {
                                float SpeedX = x * player.direction * 2, SpeedY = -18 - x * 2;
                                int proj = Projectile.NewProjectile(player.Center, new Vector2(SpeedX, SpeedY), Terraria.ID.ProjectileID.WoodenArrowFriendly, GetDamage(player, data, true), 1.2f, player.whoAmI);
                                Main.projectile[proj].noDropItem = true;
                            }
                            data.SetFloat(RainPosXVar, ShotPos.X);
                        }
                        if (data.Time >= 60 * 3)
                            data.ChangeStep();
                    }
                    break;
                case 1:
                    {
                        if (data.Time % 3 == 0)
                        {
                            Vector2 ProjPosition = player.Center;
                            ProjPosition.X = data.GetFloat(RainPosXVar);
                            ProjPosition.Y -= Main.screenHeight;
                            ProjPosition.X -= Main.rand.Next(-Main.screenHeight, Main.screenHeight);
                            int proj = Projectile.NewProjectile(ProjPosition, new Vector2(0, 10 + Main.rand.Next(7, 13)), Terraria.ID.ProjectileID.WoodenArrowFriendly, GetDamage(player, data, false), 0.7f, player.whoAmI);
                            Main.projectile[proj].noDropItem = true;
                        }
                        if (data.Time >= 7 * 60)
                        {
                            data.EndUse();
                        }
                    }
                    break;
            }
        }
    }
}
