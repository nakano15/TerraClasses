using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace TerraClasses.SkillList.Mage
{
    public class SpellResist : SkillBase
    {
        public SpellResist()
        {
            Name = "Spell Resist";
            Description = "Gain 20% projectile damage resistance at max rank.\n(only works for certain projectiles like beams, blast, and energy waves)";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Passive;
        }

        public override void ModifyHitByProjectile(Player player, SkillData data, Projectile proj, ref int damage, ref bool crit)
        {
            switch (proj.type) {
                case 9: //Starfury
                case 16: //Magic Missile
                case 20: //Green Laser
                case 79: //Rainbow Rod
                case 83: //Eye Laser
                case 84: //Pink Laser
                case 88: //Purple Laser
                case 92: //Hallow Star
                case 100: //Death Laser
                case 114: //Unholy Trident
                case 115: //Unholy Trident
                case 116: //Sword Beam
                case 128: //Frost Blast
                case 129: //Rune Blast
                case 132: //Terra Beam
                case 156: //Light Beam
                case 157: //Night Beam
                case 173: //Enchanted Beam
                case 254: //Magnet Sphere Ball
                case 255: //Magnet Sphere Bolt
                case 257: //Frost Beam
                case 259: //Eye Beam
                case 260: //Heat Ray
                case 290: //Shadow Beam
                case 291: //Inferno Bolt
                case 292: //Inferno Blast
                case 294: //Shadow Beam
                case 295: //Inferno Bolt
                case 296: //Inferno Blast
                case 389: //Retinamini
                case 435: //Electric Bolt
                case 436: //Brain Scrambling Bolt
                case 438: //Laser Ray
                case 439: //Laser Machinegun
                case 443: //Electrosphere
                case 451: //Influx Weaver
                case 452: //Phantasmal Eye
                case 454: //Phantasmal Eye
                case 455: //Phantasmal Death Ray
                case 462: //Phantasmal Bolt
                case 465: //Lightning Orb
                case 466: //Lightning Orb Arc
                case 537: //Stardust Laser
                case 575: //Nebula Sphere
                case 576: //Nebula Laser
                case 577: //Vortex Laser
                case 592: //Laser Ray
                    damage -= (int)(damage * (0.02f * data.Level));
                    break;
            }
        }
    }
}
