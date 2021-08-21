using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace TerraClasses.SkillList.Arachnomancer
{
    class Ensnare : SkillBase
    {
        public Ensnare()
        {
            Name = "Ensnare";
            Description = "Ensnares foes around mouse region with cobwebs.\n" +
                "  Ensnared foes will be imobilized.\n" +
                "  Foes affected by this skill will receive more damage from your attacks.\n" +
                "    Damage foes receive in this state increases by 1.5% per level.\n" +
                "    5% more damage received past level 5." +
                "  Defeating a foe in this state using melee attacks restores your health.\n" +
                "    Restores 10% of health based on damage.";
            MaxLevel = 10;
            skillType = Enum.SkillTypes.Active;
            Cooldown = GetTime(20, 1);
        }

        public override float GetEffectRange(SkillData sd)
        {
            return 120f + sd.Level * 15f;
        }

        public override void Update(Player player, SkillData data)
        {
            if(data.Step == 0)
            {
                float Range = GetEffectRange(data);
                for(int n = 0; n < 200; n++)
                {
                    if(Main.npc[n].active && !Main.npc[n].friendly && !Main.npc[n].dontTakeDamage && Main.npc[n].Distance(data.CastPosition) < Range)
                    {
                        data.ApplyNpcInteraction(Main.npc[n]);
                    }
                }
                data.ChangeStep();
            }
            foreach(NPC n in data.GetNpcsInteractedWith())
            {
                if (n.active)
                {
                    n.velocity *= 0;
                    n.position = n.oldPosition;
                }
            }
            if(data.Time >= (7f + 0.75f * data.Level) * 60)
            {
                data.EndUse();
            }
        }

        public override void ModifyHitNPC(Player player, SkillData data, Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            float DamageBonus = 0.015f * data.Level;
            if (data.Level > 5)
                DamageBonus += 0.05f;
            damage += (int)(damage * DamageBonus);
            if (data.Level >= 10 && item.melee)
            {
                int FinalDamage = (int)(damage - target.defense * 0.5f);
                if (FinalDamage < 1)
                    FinalDamage = 1;
                if (crit)
                    FinalDamage *= 2;
                int HealthRestored = (int)(Math.Max(FinalDamage * 0.1f, 1));
                player.statLife += HealthRestored;
                player.HealEffect(HealthRestored);
            }
        }

        public override void ModifyHitNPCWithProj(Player player, SkillData data, Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            float DamageBonus = 0.015f * data.Level;
            if (data.Level > 5)
                DamageBonus += 0.05f;
            damage += (int)(damage * DamageBonus);
            if(data.Level >= 10 && proj.melee)
            {
                int FinalDamage = (int)(damage - target.defense * 0.5f);
                if (FinalDamage < 1)
                    FinalDamage = 1;
                if (crit)
                    FinalDamage *= 2;
                int HealthRestored = (int)(Math.Max(FinalDamage * 0.1f, 1));
                player.statLife += HealthRestored;
                player.HealEffect(HealthRestored);
            }
        }

        public override void Draw(Player player, SkillData data, PlayerDrawInfo pdi)
        {
            Terraria.DataStructures.DrawData dd;
            Texture2D texture = Main.extraTexture[32];
            Microsoft.Xna.Framework.Vector2 CobwebOrigin = new Microsoft.Xna.Framework.Vector2(texture.Width, texture.Height) * 0.5f;
            foreach(NPC n in data.GetNpcsInteractedWith())
            {
                Microsoft.Xna.Framework.Vector2 DrawPosition = n.Center - Main.screenPosition;
                dd = new Terraria.DataStructures.DrawData(texture, DrawPosition, null, Microsoft.Xna.Framework.Color.Wheat, 
                    0f, CobwebOrigin, 1f, SpriteEffects.None, 0);
                Main.playerDrawData.Insert(0, dd);
            }
        }
    }
}
