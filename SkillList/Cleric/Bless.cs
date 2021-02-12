using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerraClasses.SkillList.Cleric
{ 
    public class Bless : SkillBase
    {
        public Bless()
        {
            Name = "Bless";
            Description = "Increases the allies Damage by 4% per level.\nAllies defense increases by 2% per level.";
            skillType = Enum.SkillTypes.Active;
            MaxLevel = 10;
            Cooldown = 1* 3600 + 30 * 60;
        }

        public override void Update(Player player, SkillData data)
        {
            if (data.Time == 1)
            {
                float distance = 136f;
                for (int p = 0; p < 255; p++)
                {
                    if (Main.player[p].active && !Main.player[p].dead && (Main.player[p].Center - player.Center).Length() < distance)
                    {
                        data.ApplySkillBuff(Main.player[p], ModContent.BuffType<Buffs.Bless>(), 5 * 3600 + 30 * 60 * data.Level);
                        data.ApplyPlayerInteraction(Main.player[p]);
                    }
                }
            }
            if (data.Time % 3 == 0)
            {
                float HealEffectX = (float)Math.Sin(data.Time * 3) * 5;
                foreach (Player p in data.GetPlayersInteractedWith())
                {
                    Vector2 EffectPos = new Vector2(player.Center.X + HealEffectX, player.position.Y - 8);
                    Dust d = Dust.NewDustPerfect(EffectPos - Main.screenPosition, Terraria.ID.DustID.Platinum);
                    d.noGravity = false;
                    d.noLight = false;
                    d.scale = Main.rand.Next(13, 18) * 0.1f;
                }
            }
            if (data.Time >= 90)
            {
                data.EndUse();
            }
        }
    }
}
