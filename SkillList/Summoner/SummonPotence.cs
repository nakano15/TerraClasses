using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace TerraClasses.SkillList.Summoner
{
    public class SummonPotence : SkillBase
    {
        public SummonPotence()
        {
            Name = "Summon Potence";
            Description = "Allows companions to inflict critical hit.\n" +
                " Every level increase Critical Rate by 1%.\n " +
                " Critical Rate increases by 5% after level 5.\n" +
                " The media of all your character critical rate will affect your companion.\n" +
                "  (MeleeCrit + RangedCrit + MagicCrit) / 3";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Passive;
        }

        public override SkillData GetSkillData => new SummonPotenceSkillData();

        public override void UpdateStatus(Player player, SkillData rawdata)
        {
            SummonPotenceSkillData data = (SummonPotenceSkillData)rawdata;
            data.LoggedCriticalBonus = (int)((float)(player.meleeCrit + player.rangedCrit + player.magicCrit) / 3);
        }

        public override void ModifyHitNPCWithProj(Terraria.Player player, SkillData rawdata, Terraria.Projectile proj, Terraria.NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (proj.minion || proj.type == 376 || proj.type == 378 || proj.type == 379 || proj.type == 389 || proj.type == 408 || proj.type == 614)
            {
                SummonPotenceSkillData data = (SummonPotenceSkillData)rawdata;
                int CriticalRate = data.Level + data.LoggedCriticalBonus;
                if (data.Level > 5)
                    CriticalRate += 5;
                if (Terraria.Main.rand.Next(100) < CriticalRate)
                {
                    //damage *= 2;
                    crit = true;
                }
            }
        }

        public class SummonPotenceSkillData : SkillData
        {
            public int LoggedCriticalBonus = 0;
        }
    }
}
