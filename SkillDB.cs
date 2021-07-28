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
                case 50:
                    return new SkillList.Knight.Vow();
                case 51:
                    return new SkillList.Knight.Charge();
                case 52:
                    return new SkillList.Knight.Unload();
                case 53:
                    return new SkillList.Knight.ShieldBash();
                case 54:
                    return new SkillList.Knight.HighGuard();
                case 55:
                    return new SkillList.Knight.HeavySlam();
                case 56:
                    return new SkillList.Knight.Provoke();
                case 57:
                    return new SkillList.Arachnomancer.SpiderSwarm();
                case 58:
                    return new SkillList.Thief.DoubleStrike();
                case 59:
                    return new SkillList.Arachnomancer.VenomousAmmo();
                case 60:
                    return new SkillList.Thief.PoisonousStrike();
                case 61:
                    return new SkillList.Thief.DodgeMastery();
                case 62:
                    return new SkillList.Thief.Steal();
                case 63:
                    return new SkillList.Thief.Hide();
                case 64:
                    return new SkillList.Cleric.BaneUndead();
                case 65:
                    return new SkillList.Cleric.IncreaseAgility();
                case 66:
                    return new SkillList.Cleric.BaneDemons();
                case 67:
                    return new SkillList.Knight.ConstantRest();
                case 68:
                    return new SkillList.Sharpshooter.Marksmanship();
                case 69:
                    return new SkillList.Thief.PoisonousArrows();
                case 70:
                    return new SkillList.Fighter.PiercingShot();
                case 71:
                    return new SkillList.Cleric.DivineSmite();
                case 72:
                    return new SkillList.Archer.PreciseStrike();
                case 73:
                    return new SkillList.Cerberus.Frost_Breath();
                case 74:
                    return new SkillList.Cerberus.Lightning_Breath();
                case 75:
                    return new SkillList.Summoner.SummonQueenBee();
                case 76:
                    return new SkillList.Summoner.SummonDemon();
                case 77:
                    return new SkillList.Cerberus.CerberusHead();
                case 78:
                    return new SkillList.Cerberus.PawStrike();
                case 79:
                    return new SkillList.Cerberus.ClawSlash();
            }
            return new SkillBase();
        }
    }
}
