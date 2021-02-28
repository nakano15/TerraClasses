using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.SkillList.Berseker
{
    public class AxePower : SkillBase
    {
        public AxePower()
        {
            Name = "Axe Power";
            Description = "Increases damage of Axe type weapons based on 2% per level of their axe power.\n" +
                " Critical rate when using axes increases by 6%.\n" +
                " At level 6 and above, hitting a monster with an axe can restore 7% of health based on damage.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Passive;            
        }

        public override void UpdateStatus(Terraria.Player player, SkillData data)
        {
            if (player.HeldItem.axe > 0)
            {
                player.meleeDamage += 0.02f * player.HeldItem.axe * 0.01f;
                player.meleeCrit += 6;
            }
        }

        public override void OnHitNPC(Terraria.Player player, SkillData data, Terraria.Item item, Terraria.NPC target, int damage, float knockback, bool crit)
        {
            OnHitNPC(player, data, target, damage);
        }


        public override void OnHitNPCWithProj(Terraria.Player player, SkillData data, Terraria.Projectile proj, Terraria.NPC target, int damage, float knockback, bool crit)
        {
            OnHitNPC(player, data, target, damage);
        }

        public void OnHitNPC(Terraria.Player player, SkillData data, Terraria.NPC target, int damage)
        {
            if (data.Level > 5 && player.HeldItem.axe > 0 && Terraria.Main.rand.Next(3) == 0)
            {
                int ToRestore = (int)(damage * 0.07f);
                if (ToRestore < 1)
                    ToRestore = 1;
                Terraria.CombatText.NewText(player.getRect(), Terraria.CombatText.HealLife, ToRestore, false, true);
                player.statLife += ToRestore;
            }
        }
    }
}
