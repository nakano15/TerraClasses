using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TerraClasses.SkillList;

namespace TerraClasses
{
    public class SkillDB
    {

        public static SkillBase GetSkill(int ID)
        {
            switch (ID)
            {
                case 0:
                    return new SkillList.Terrarian.BasicTraining();
                case 1:
                    return new SkillList.Archer.ExtraArrows();
                case 2:
                    return new SkillList.Archer.RangedMastery();
                case 3:
                    return new SkillList.Archer.ArrowRain();
                case 4:
                    return new SkillList.Fighter.ProgressiveAttack();
                case 5:
                    return new SkillList.Fighter.TrustyLongsword();
                case 6:
                    return new SkillList.Fighter.Endure();
                case 7:
                    return new SkillList.Fighter.DoubleArrows();
                case 8:
                    return new SkillList.Mage.MagicArrows();
                case 9:
                    return new SkillList.Archer.FireArrow();
                case 10:
                    return new SkillList.Fighter.MeleeMastery();
                case 11:
                    return new SkillList.Fighter.CrowdControl();
                case 12:
                    return new SkillList.Cleric.Heal();
                case 13:
                    return new SkillList.Cleric.Bless();
                case 14:
                    return new SkillList.Mage.MagicMastery();
                case 15:
                    return new SkillList.Mage.ManaControl();
                case 16:
                    return new SkillList.Mage.MeteorBarrage();
                case 17:
                    return new SkillList.Mage.Fireball();
                case 18:
                    return new SkillList.Mage.SoulRage();
                case 19:
                    return new SkillList.Archer.ArrowBarrage();
                case 20:
                    return new SkillList.Merchant.Barter();
                case 21:
                    return new SkillList.Merchant.Avarice();
                case 22:
                    return new SkillList.Cerberus.CerberusForm();
                case 23:
                    return new SkillList.Sharpshooter.SupressiveFire();
                case 24:
                    return new SkillList.Merchant.Discount();
                case 25:
                    return new SkillList.Cerberus.ElementalBreath();
                case 26:
                    return new SkillList.Sharpshooter.Snipe();
                case 27:
                    return new SkillList.Sharpshooter.GunKnowledge();
                case 28:
                    return new SkillList.Sharpshooter.Birdfall();
                case 29:
                    return new SkillList.Sharpshooter.Flintlock();
                case 30:
                    return new SkillList.Berseker.Anger();
                case 31:
                    return new SkillList.Berseker.Constitution();
                case 32:
                    return new SkillList.Berseker.AxePower();
                case 33:
                    return new SkillList.Berseker.Frenzy();
                case 34:
                    return new SkillList.Cleric.RepelUndead();
                case 35:
                    return new SkillList.Summoner.MinionMastery();
                case 36:
                    return new SkillList.Summoner.SummonPotence();
                case 37:
                    return new SkillList.Summoner.SpiritFlame();
                case 38:
                    return new SkillList.Summoner.HauntingPresence();
                case 39:
                    return new SkillList.Summoner.InvokeBats();
                case 40:
                    return new SkillList.Archer.Precision();
                case 41:
                    return new SkillList.Archer.SwiftStep();
                case 42:
                    return new SkillList.Berseker.Bully();
                case 43:
                    return new SkillList.Berseker.ThreateningPresence();
                case 44:
                    return new SkillList.Summoner.PiercingAttacks();
                case 45:
                    return new SkillList.Merchant.ThrowCoins();
                case 46:
                    return new SkillList.Mage.SpellResist();
                case 47:
                    return new SkillList.Cerberus.Fire_Breath();
                case 48:
                    return new SkillList.Mage.ChainLightning();
                case 49:
                    return new SkillList.Knight.Cavalry();
            }
            return new SkillBase();
        }
    }
}
