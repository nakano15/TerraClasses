using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace TerraClasses.SkillList.Cleric
{
    public class Heal : SkillBase
    {
        public Heal()
        {
            Name = "Heal";
            Description = "Restores self and nearby allies health.\nHealth restored is equal to 15% of your max health, plus\n40% + 8% per level of magic damage.\nHealing distance is of 120+8 per level pixels.";
            skillType = Enum.SkillTypes.Active;
            MaxLevel = 10;
            Cooldown = 45 * 60;
        }

        public override void Update(Player player, SkillData data)
        {
            if (data.Time == 1)
            {
                int HealingPower = data.GetMagicDamage(0, 0.4f + 0.08f * data.Level, player) + (int)(player.statLifeMax2 * 0.15f);
                float MaxDistance = 120 + 8 * data.Level;
                for (int p = 0; p < 255; p++)
                {
                    if (p == player.whoAmI || (Main.player[p].active && !Main.player[p].dead && player.hostile == Main.player[p].hostile && player.team == Main.player[p].team && Main.player[p].Distance(player.Center) < MaxDistance))
                    {
                        data.ApplyPlayerInteraction(Main.player[p]);
                        player.statLife += HealingPower;
                        player.HealEffect(HealingPower);
                        data.ApplyPlayerInteraction(Main.player[p]);
                    }
                }
                data.EndUse();
            }
            if (data.Time % 3 == 0)
            {
                float HealEffectX = (float)Math.Sin(data.Time * 3) * 5;
                float EffectPositionY = 42 - (42 * ((float)data.Time / 90));
                for (int x = -1; x < 2; x += 2)
                {
                    foreach (Player p in data.GetPlayersInteractedWith())
                    {
                        Vector2 EffectPos = new Vector2(player.Center.X + HealEffectX * x, player.position.Y + EffectPositionY);
                        Dust d = Dust.NewDustPerfect(EffectPos - Main.screenPosition, Terraria.ID.DustID.GreenBlood);
                        d.noGravity = false;
                        d.noLight = false;
                        d.scale = Main.rand.Next(13, 18) * 0.1f;
                    }
                }
            }
            if (data.Time >= 90)
                data.EndUse();
        }
    }
}
