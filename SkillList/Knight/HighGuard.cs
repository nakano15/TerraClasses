using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace TerraClasses.SkillList.Knight
{
    public class HighGuard : SkillBase
    {
        public HighGuard()
        {
            Name = "High Guard";
            Description = "Upon activating, will either apply or remove High Guard buff.\n" +
                " When High Guard is active, It will give you:\n" +
                "  2 per level of Defense.\n" +
                " After Level 5, +5% Defense bonus.\n" +
                " Reduce all your critical rates by 10%.\n" +
                " Melee attacks will be 20% slower, but also stronger.\n" +
                " Movement Speed drops by 15%.";
            Cooldown = 60;
            CastTime = 20;
            skillType = Enum.SkillTypes.Active;
            MaxLevel = 10;
        }

        public override void Update(Player player, SkillData data)
        {
            int BuffID = ModContent.BuffType<Buffs.HighGuard>();
            if (player.HasBuff(BuffID))
            {
                player.DelBuff(BuffID);
                CombatText.NewText(player.getRect(), Microsoft.Xna.Framework.Color.White, "High Guard Deactivated.");
            }
            else
            {
                data.ApplySkillBuff(player, BuffID, 5);
                CombatText.NewText(player.getRect(), Microsoft.Xna.Framework.Color.White, "High Guard Activated.");
            }
            data.EndUse(false);
        }
    }
}
