using Terraria;
using Microsoft.Xna.Framework;

namespace TerraClasses.TargetTranslator
{
    public class NpcTarget : Translator
    {
        public NPC npc;

        public NpcTarget(NPC owner)
        {
            npc = owner;
        }

        public override object Target => npc;

        public override string CharacterIdentifier => "npc" + npc.whoAmI;
        public override string Name => npc.GivenOrTypeName;
        public override bool Male { get => false; }

        public override int Health { get => npc.life; set => npc.life = value; }
        public override int MaxHealth { get => npc.lifeMax; set => npc.lifeMax = value; }
        public override int Mana { get => 0; }
        public override int MaxMana { get => 0; }
        public override int Defense { get => npc.defense; set => npc.defense = value; }
        public override int Width { get => npc.width; set => npc.width = value; }
        public override int Height { get => npc.height; set => npc.height = value; }
        public override Vector2 Position { get => npc.position; set => npc.position = value; }
        public override Vector2 Velocity { get => npc.velocity; set => npc.velocity = value; }
        public override Rectangle GetRectangle => npc.getRect();

        public override bool Immunity { get => npc.dontTakeDamage; set => base.Immunity = value; }

        public override void PlayHurtSound()
        {
            Main.PlaySound((npc.life > 0 ? npc.HitSound : npc.DeathSound), npc.position);
        }

        public override void ApplyKnockback(float Knockback, int Direction)
        {
            if (Knockback > 0 && npc.knockBackResist > 0)
            {
                float NewKB = Knockback * npc.knockBackResist;
                npc.velocity.X += Direction * NewKB;
                npc.velocity.Y -= (npc.noGravity ? 0.75f : 0.5f);
            }
        }

        public override bool IsActive()
        {
            return npc.active;
        }

        public override bool IsDead()
        {
            return !npc.active;
        }

        public override void HitEffect(double Damage, int Direction)
        {
            npc.HitEffect(Direction, Damage);
        }

        public override void AddBuff(int BuffID, int BuffTime)
        {
            npc.AddBuff(BuffID, BuffTime);
        }

        public override bool HasBuff(int BuffID)
        {
            return npc.HasBuff(BuffID);
        }

        public override void DelBuff(int BuffID)
        {
            npc.DelBuff(BuffID);
        }

        public override double Hurt(int Damage, float Knockback, bool Critical = false, bool Pvp = false, string HitText = " was slain...")
        {
            return npc.StrikeNPC(Damage, Knockback, (Knockback > 0 ? 1 : -1), Critical);
        }

        public override void Kill(int Damage, int HitDirection, bool Pvp = false, string KillText = " was slain...")
        {
            npc.HitEffect(HitDirection, Damage);
            if (npc.life > 0)
                npc.life = 0;
            npc.checkDead();
        }
    }
}
