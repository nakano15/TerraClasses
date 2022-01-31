using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace TerraClasses.SkillList.Arachnomancer
{
    public class ToxicInjection : SkillBase
    {
        public ToxicInjection()
        {
            Name = "Toxic Injection";
            Description = "Applied Acid Venom debuff on enemies affected by Ensnare.\n" +
                "  Inflicts 87% + 2.5% per level of magic damage on use.\n" +
                "  Acid Venom debuff lasts for 12 seconds.";
            Cooldown = GetTime(10);
            skillType = Enum.SkillTypes.Active;
            MaxLevel = 10;
        }

        public override void Update(Player player, SkillData data)
        {
            SkillData EnsnareSkill = PlayerMod.GetPlayerSkillData(player, 83);
            if (EnsnareSkill == null)
            {
                Main.NewText("You don't have Ensnare skill.", Microsoft.Xna.Framework.Color.Red);
                data.EndUse(true);
                return;
            }
            if (!EnsnareSkill.Active)
            {
                Main.NewText("Ensnare is not in use!", Microsoft.Xna.Framework.Color.Red);
                data.EndUse(true);
                return;
            }
            float DamageMult = 0.87f + 0.025f * data.Level;
            foreach(NPC n in EnsnareSkill.GetNpcsInteractedWith())
            {
                data.HurtNpc(player, n, DamageTypes.Magic, DamageMult, 0, 0);
                for(int i = 0; i < 20; i++)
                {
                    Dust.NewDust(n.position, n.width, n.height, 171, Main.rand.Next(-100, 101) * 0.1f, Main.rand.Next(-100, 101) * 0.1f);
                }
                n.AddBuff(Terraria.ID.BuffID.Venom, 12 * 60);
            }
            data.EndUse(false);

        }
    }
}
