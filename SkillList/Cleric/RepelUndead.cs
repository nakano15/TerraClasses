using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace TerraClasses.SkillList.Cleric
{
    public class RepelUndead : SkillBase
    {
        public RepelUndead()
        {
            Name = "Repel Undead";
            Description = "Confuses nearby undead monsters.\n" +
                " Skill is active for 20 seconds.\n" +
                " For every 2 levels, increases damage overtime on nearby undeads by 2.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Active;
            Cooldown = GetTime(30, 2);
            CastTime = 30;
            PositionToTake = PositionToTakeOnCastEnum.Player;
        }

        public override float GetEffectRange(SkillData sd)
        {
            return 70 + 5 * sd.Level;
        }

        public bool IsUndead(int MobID)
        {
            return MainMod.IsUndead(MobID);
        }

        public override void Update(Terraria.Player player, SkillData data)
        {
            float Range = GetEffectRange(data);
            Vector2 PlayerCenter = player.Center;
            int DamageOvertimeInflicted = 0;
            bool InflictDamage = data.Time % 6 == 0;
            if (InflictDamage)
                DamageOvertimeInflicted = (data.Level / 2) * 2;
            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].active && !Main.npc[i].dontTakeDamage && Main.npc[i].Distance(PlayerCenter) < Range && IsUndead(Main.npc[i].type))
                {
                    Main.npc[i].AddBuff(31, 5 * 60);
                    if (InflictDamage)
                    {
                        CombatText.NewText(Main.npc[i].getRect(), CombatText.DamagedHostile, DamageOvertimeInflicted, false, true);
                        Main.npc[i].life -= DamageOvertimeInflicted;
                        if (Main.npc[i].life <= 0)
                            Main.npc[i].checkDead();
                    }
                }
            }
            if (data.Time >= 20 * 60)
                data.EndUse();
        }
    }
}
