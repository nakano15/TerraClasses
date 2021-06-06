using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace TerraClasses.SkillList.Cerberus
{
    public class Frost_Breath : SkillBase
    {
        public Frost_Breath()
        {
            Name = "Frost Breath";
            Description = "Launches Frost Bolts to the mouse direction.\n" +
                " Each bolt inflicts 95% + 9% per level of Magic Damage.\n" +
                " Launches 3 + 1 for every 2 level bolts in a row.\n" +
                " Level reduces the delay between each bolt.\n" +
                " At higher levels, there is a chance of launching multiple bolts at once.";
            skillType = Enum.SkillTypes.Active;
            MaxLevel = 10;
            Cooldown = GetTime(32);
            UnallowOtherSkillUsage = true;
        }

        public override void UpdateStatus(Player player, SkillData data)
        {
            player.delayUseItem = true;
        }

        public override void Update(Player player, SkillData data)
        {
            const int ProjectileID = Terraria.ID.ProjectileID.FrostBoltStaff;
            float ShotTime = 10 - ((data.Level * 0.5f) - (data.Level + 1));
            if (ShotTime < 5)
                ShotTime = 5;
            if (data.Time > 1 && data.LastTime <= 1)
            {
                CerberusFormData cfb = (CerberusFormData)PlayerMod.GetPlayerSkillData(player, 22);
                if(cfb != null)
                {
                    cfb.HeadFrame[1] = (byte)(ShotTime * 0.5f);
                }
            }
            if (data.Time > 3 && data.LastTime <= 3)
            {
                for (int i = 0; i < 1 + Main.rand.Next((int)(data.Level * 0.3)); i++)
                {
                    Vector2 ShotSpawnPos = CerberusForm.GetMouthPosition(player, true);
                    Vector2 ShotDirection = (new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition - ShotSpawnPos);
                    int Accuracy = 50 - data.Level;
                    if (Accuracy < 0)
                        Accuracy = 0;
                    ShotDirection.X += Main.rand.Next(-Accuracy, Accuracy + 1);
                    ShotDirection.Y += Main.rand.Next(-Accuracy, Accuracy + 1);
                    ShotDirection.Normalize();
                    ShotDirection *= 16;
                    int Damage = data.GetMagicDamage(0, 0.95f + 0.09f * data.Level, player);
                    Projectile.NewProjectile(ShotSpawnPos, ShotDirection, ProjectileID, Damage, 7f, player.whoAmI);
                }
            }
            if(data.Time <= 5)
            {
                player.direction = (Main.mouseX + Main.screenPosition.X < player.Center.X) ? -1 : 1;
            }
            if (data.Time >= ShotTime)
            {
                if (data.Step >= (data.Level + 5) * 0.5f)
                    data.EndUse();
                else
                    data.ChangeStep();
            }
        }
    }
}
