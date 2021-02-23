using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;

namespace TerraClasses.TargetTranslator
{
    public class PlayerTarget : Translator
    {
        public Player player;

        public PlayerTarget(Player owner)
        {
            player = owner;
        }

        public override object Target => player;

        public override string CharacterIdentifier => "pl" + player.whoAmI;
        public override string Name => player.name;
        public override bool Male { get => player.Male; set => player.Male = value; }

        public override int Health { get => player.statLife; set => player.statLife = value; }
        public override int MaxHealth { get => player.statLifeMax2; set => player.statLifeMax2 = value; }
        public override int Mana { get => player.statMana; set => player.statMana = value; }
        public override int MaxMana { get => player.statManaMax2; set => player.statManaMax2 = value; }
        public override int Defense { get => player.statDefense; set => player.statDefense = value; }
        public override int Width { get => player.width; set => player.width = value; }
        public override int Height { get => player.height; set => player.height = value; }
        public override Vector2 Position { get => player.position; set => player.position = value; }
        public override Vector2 Velocity { get => player.velocity; set => player.velocity = value; }
        public override Rectangle GetRectangle => player.getRect();
        public override bool Immunity { get => player.immuneTime > 0 || player.immune; set => player.immune = value; }

        public override void PlayHurtSound()
        {
            if (player.stoned)
            {
                Main.PlaySound(0, (int)player.position.X, (int)player.position.Y, 1, 1f, 0f);
            }
            else if (player.frostArmor)
            {
                Main.PlaySound(Terraria.ID.SoundID.Item27, player.position);
            }
            else if ((player.wereWolf || player.forceWerewolf) && !player.hideWolf)
            {
                Main.PlaySound(3, (int)player.position.X, (int)player.position.Y, 6, 1f, 0f);
            }
            else if (player.boneArmor)
            {
                Main.PlaySound(3, (int)player.position.X, (int)player.position.Y, 2, 1f, 0f);
            }
            else if (!player.Male)
            {
                Main.PlaySound(20, (int)player.position.X, (int)player.position.Y, 1, 1f, 0f);
            }
            else
            {
                Main.PlaySound(1, (int)player.position.X, (int)player.position.Y, 1, 1f, 0f);
            }
        }

        public override bool IsActive()
        {
            return player.active;
        }

        public override bool IsDead()
        {
            return player.dead || player.ghost;
        }

        public override void ApplyKnockback(float Knockback, int Direction)
        {
            //player.velocity += 
        }

        public override void AddBuff(int BuffID, int BuffTime)
        {
            player.AddBuff(BuffID, BuffTime);
        }

        public override bool HasBuff(int BuffID)
        {
            return player.HasBuff(BuffID);
        }

        public override void DelBuff(int BuffID)
        {
            player.DelBuff(BuffID);
        }

        public override double Hurt(int Damage, float Knockback, bool Critical = false, bool Pvp = false, string HitText = " was slain...")
        {
            return player.Hurt(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(HitText), Damage, (Knockback > 0 ? 1 : -1), Pvp, false, Critical);
        }

        public override void Kill(int Damage, int HitDirection, bool Pvp = false, string KillText = " was slain...")
        {
            player.KillMe(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(KillText), Damage, HitDirection, Pvp);
        }

        public override void ChangeHealth(int Change, bool DoT = false, string DeathReason = " couldn't stand for longer.")
        {
            if (player.dead)
                return;
            int HealthChange = Change;
            if (player.statLife + HealthChange > player.statLifeMax2)
                HealthChange -= player.statLife + HealthChange - player.statLifeMax2;
            player.statLife += HealthChange;
            if (player.statLife <= 0)
                player.KillMe(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(DeathReason), HealthChange, 0, true);
            if (!DoT && Change > 0)
                player.HealEffect(Change);
            else
                CombatText.NewText(player.getRect(), (HealthChange > 0 ? CombatText.LifeRegen : CombatText.LifeRegenNegative), HealthChange, false, DoT);
        }

        public override void ChangeMana(int Change, bool DoT = false)
        {
            if (player.dead) return;
            int ManaChange = Change;
            if(ManaChange + player.statMana > player.statManaMax2)
            {
                ManaChange -= player.statMana + ManaChange - player.statManaMax2;
            }
            player.statMana += ManaChange;
            if (Change > 0 && !DoT)
            {
                player.ManaEffect(ManaChange);
            }
            else
            {
                CombatText.NewText(player.getRect(), (Change > 0 ? CombatText.HealMana : CombatText.HealMana * 0.6f), Change, false, DoT);
            }
        }
    }
}
