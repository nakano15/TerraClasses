using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using TerraClasses.TargetTranslator;

namespace TerraClasses.SkillList.Knight
{
    public class Charge : SkillBase
    {
        public Charge()
        {
            Name = "Charge";
            Description = "Charge at your foes, launching them backwards.\n" +
                " Your character is invulnerable while charging.\n" +
                " Charging inflicts 33% + 2.5% per level of melee damage.\n" +
                " Your melee damage also increases by the number above, when you attack." +
                " Damage increases by 20% when mounted.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Active;
            PositionToTake = PositionToTakeOnCastEnum.Player;
            Cooldown = GetCooldown(40);
        }

        public float GetDamageValue(Player player, SkillData data)
        {
            float Damage = 0.33f + 0.025f * data.Level;
            if (player.mount.Active)
                Damage += 0.2f;
            return Damage;
        }

        public override void UpdateStatus(Player player, SkillData data)
        {
            player.meleeDamage += GetDamageValue(player, data);
        }

        public override void Update(Player player, SkillData data)
        {
            player.immuneTime = 60;
            float DamageToInflict = GetDamageValue(player, data);
            if(player.direction > 0)
            {
                player.controlRight = true;
                player.controlLeft = false;
            }
            else
            {
                player.controlRight = false;
                player.controlLeft = true;
            }
            Translator[] Targets = data.GetPossibleTargets(false, false, player.Center, 200);
            Microsoft.Xna.Framework.Rectangle rect = player.getRect();
            int Damage = data.GetMeleeDamage(0, DamageToInflict, player);
            foreach(Translator Target in Targets)
            {
                if (Target.GetRectangle.Intersects(rect))
                {
                    data.HurtTarget(Target, Damage, player.direction, 12f);
                }
            }
            if (data.Time >= 5 * 60)
            {
                data.EndUse(false);
            }
        }
    }
}
