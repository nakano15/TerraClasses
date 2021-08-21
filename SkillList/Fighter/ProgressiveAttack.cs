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

        public override SkillData GetSkillData => new ProgressiveAttackData();

        public override void Update(Player player, SkillData rawdata)
        {
            ProgressiveAttackData data = (ProgressiveAttackData)rawdata;
            if (player.inventory[player.selectedItem].melee)
            {
                if(player.itemAnimation > data.LastItemAttackTime)
                {
                    data.AttackLevel++;
                    data.TimeOut = 300;
                    if (data.AttackLevel > 3)
                        data.AttackLevel -= 3;
                    //CombatText.NewText(new Microsoft.Xna.Framework.Rectangle((int)player.position.X, (int)player.Center.Y, player.width, 1), Microsoft.Xna.Framework.Color.Red, "Swing " + data.AttackLevel, false, true);
                }
            }
            data.LastItemAttackTime = player.itemAnimation;
        }

        public override void UpdateStatus(Terraria.Player player, SkillData rawdata)
        {
            ProgressiveAttackData data = (ProgressiveAttackData)rawdata;
            if (data.AttackLevel > 0)
            {
                if (data.TimeOut == 0)
                {
                    if (data.AttackLevel > 0)
                    {
                        data.AttackLevel = 0;
                        //CombatText.NewText(new Microsoft.Xna.Framework.Rectangle((int)player.Center.X, (int)player.Center.Y, 1, 1), Microsoft.Xna.Framework.Color.Red, "Combo Break", false, true);
                    }
                }
                else
                {
                    data.TimeOut--;
                    switch (data.AttackLevel)
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
                }
            }
        }

        public class ProgressiveAttackData: SkillData
        {
            public byte AttackLevel = 0;
            public int TimeOut = 0, LastItemAttackTime = 0;
        }
    }
}
