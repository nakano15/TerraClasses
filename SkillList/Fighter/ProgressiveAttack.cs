using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace TerraClasses.SkillList.Fighter
{
    public class ProgressiveAttack : SkillBase
    {
        const byte IAttackCycleVar = 0, IAttackTimeout = 1;

        public ProgressiveAttack()
        {
            Name = "Progressive Attack";
            Description = "Progressivelly changes the attack damage and speed based on swings.\n" +
                " First swing inflicts 1% extra damage per level.\n" +
                " Second swing inflicts 20%+1% per level more damage.\n" +
                " Third swing inflicts 40% + 1% per level more damage, attack is 30% slower and then resets the cycle.\n" +
                " Level increases melee damage and attack speed.";
            skillType = Enum.SkillTypes.Attack;
            MaxLevel = 10;
        }

        public override void UpdateItemUse(Terraria.Player player, SkillData data, bool JustUsed)
        {
            if (JustUsed && player.inventory[player.selectedItem].melee)
            {
                int Level = data.GetInteger(IAttackCycleVar) + 1;
                if (Level > 3)
                    Level -= 3;
                data.SetInteger(IAttackCycleVar, Level);
                data.SetInteger(IAttackTimeout, 300);
                CombatText.NewText(new Microsoft.Xna.Framework.Rectangle((int)player.position.X, (int)player.Center.Y, player.width, 1), Microsoft.Xna.Framework.Color.Red, "Swing "+Level,false, true);
            }
        }

        public override void UpdateStatus(Terraria.Player player, SkillData data)
        {
            int Level = data.GetInteger(IAttackCycleVar);
            if (Level > 0)
            {
                int Timeout = data.GetInteger(IAttackTimeout);
                if (Timeout == 0)
                {
                    if (Level > 0)
                    {
                        data.SetInteger(IAttackCycleVar, 0);
                        Level = 0;
                        CombatText.NewText(new Microsoft.Xna.Framework.Rectangle((int)player.Center.X, (int)player.Center.Y, 1, 1), Microsoft.Xna.Framework.Color.Red, "Combo Break", false, true);
                    }
                }
                else
                {
                    data.SetInteger(IAttackTimeout, Timeout - 1);
                    switch (Level)
                    {
                        case 1:
                            player.meleeDamage += 0.01f * data.Level;
                            break;
                        case 2:
                            player.meleeDamage += 0.2f + 0.01f * data.Level;
                            player.meleeSpeed -= player.meleeSpeed * 0.01f * data.Level;
                            break;
                        case 3:
                            player.meleeDamage += 0.4f + 0.01f * data.Level;
                            player.meleeSpeed += 0.3f - player.meleeSpeed * 0.01f * data.Level;
                            break;
                    }
                    data.SetInteger(IAttackCycleVar, Level);
                }
            }
        }
    }
}
