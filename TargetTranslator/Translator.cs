using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TerraClasses.TargetTranslator
{
    public abstract class Translator //What kind of info would be useful to get from the target?
    {
        public virtual string Name { get; }
        public virtual string CharacterIdentifier { get; }
        public virtual bool Male { get; set; }
        public virtual int Health { get; set; }
        public virtual int MaxHealth { get; set; }
        public virtual int Mana { get; set; }
        public virtual int MaxMana { get; set; }
        public virtual int Defense { get; set; }
        public virtual Vector2 Position { get; set; }
        public virtual Vector2 Velocity { get; set; }
        public virtual Rectangle GetRectangle { get; }
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }
        public virtual bool Immunity { get; set; }
        public Vector2 Center
        {
            get { return Position + new Vector2(Width, Height) * 0.5f; }
        }
        public float HealthValueDecimal { get { return (float)Health / MaxHealth; } }
        public float HealthValuePercentage { get { return HealthValueDecimal * 100; } }
        public virtual void PlayHurtSound()
        {

        }

        public virtual bool IsActive()
        {
            return false;
        }

        public virtual bool IsDead()
        {
            return false;
        }

        public virtual void ApplyKnockback(float Knockback, int Direction)
        {

        }

        public virtual void AddBuff(int BuffID, int BuffTime)
        {

        }

        public virtual void HitEffect(double Damage, int Direction)
        {

        }

        public virtual bool HasBuff(int BuffID)
        {
            return false;
        }

        public virtual void DelBuff(int BuffID)
        {

        }

        public virtual double Hurt(int Damage, float Knockback, bool Critical = false, bool Pvp = false, string HitText = " was slain...")
        {
            return 0;
        }

        public virtual void Kill(int Damage, int HitDirection, bool Pvp = false, string KillText = " was slain...")
        {

        }

        public virtual void ChangeHealth(int Change, bool DoT = false, string DeathReason = "")
        {

        }

        public virtual void ChangeMana(int Change, bool DoT = false)
        {

        }
    }
}
