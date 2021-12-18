using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace TerraClasses.SkillList.Thief
{
    public class DoubleStrike : SkillBase
    {
        public DoubleStrike()
        {
            Name = "Double Strike";
            Description = "Attacking with a melee weapon can inflict a second hit on targets in melee range.\n" +
                " Adds 20% + 6% per level chance of striking once again with melee attack.\n" +
                " Second attack inflicts 40% + 4% per level of melee damage.";
            skillType = Enum.SkillTypes.Passive;
            MaxLevel = 10;
        }

        public override SkillData GetSkillData => new DoubleStrikeData();

        public override void Update(Player player, SkillData rawdata)
        {
            DoubleStrikeData data = (DoubleStrikeData)rawdata;
            if (player.inventory[player.selectedItem].melee)
            {
                if (player.itemAnimation > 0)
                {
                    if(data.LastAttackAnimationTime < player.itemAnimation)
                    {
                        data.Trigger = Main.rand.NextFloat() < 0.2f + 0.06f * data.Level;
                    }
                    else
                    {
                        if(data.Trigger && player.itemAnimation == (int)player.itemAnimationMax * 0.5f)
                        {
                            Item item = player.inventory[player.selectedItem];
                            Rectangle HitBox = new Rectangle((int)player.Center.X, (int)player.Center.Y, (int)(item.width * item.scale * 1.2f), (int)(item.height * item.scale));
                            if (player.direction < 0)
                                HitBox.X -= HitBox.Width;
                            if(player.gravDir > 0)
                            {
                                HitBox.Y -= (int)(HitBox.Height * 0.2f);
                            }
                            else
                            {
                                HitBox.Y += (int)(HitBox.Height * 0.2f);
                            }
                            TargetTranslator.Translator[] Targets = data.GetPossibleTargets(false, HitBox, false);
                            int ExtraDamage = data.GetMeleeDamage(0, 0.4f + 0.04f * data.Level, player);
                            bool Critical = Main.rand.Next(100) < player.meleeCrit + item.crit;
                            if (player.whoAmI == Main.myPlayer)
                            {
                                foreach (TargetTranslator.Translator target in Targets)
                                {
                                    target.Hurt(ExtraDamage, item.knockBack, Critical, true, " was sliced again.");
                                }
                            }
                        }
                    }
                }
            }
            data.LastAttackAnimationTime = player.itemAnimation;
        }

        public class DoubleStrikeData : SkillData
        {
            public bool Trigger = false;
            public int LastAttackAnimationTime = 0;

            public DoubleStrikeData()
            {

            }
        }
    }
}
