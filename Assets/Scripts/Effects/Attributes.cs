using System;
using System.Collections.Generic;

namespace Effects
{
    [Serializable]
    public struct Attributes
    {
        public float Speed;
        public float AttackDamage;
        public float AttackSpeed;
        public float AttackRange;
        public float BulletSpeed;
        public float Luck;
        public float Health;
        public float MaxHealth;

        public static Attributes Zero;

        public static Attributes One = new Attributes
        {
            Speed = 1,
            AttackDamage = 1,
            AttackSpeed = 1,
            BulletSpeed = 1,
            AttackRange = 1,
            Luck = 1,
            Health = 1,
            MaxHealth = 1
        };

        public static Attributes operator +(Attributes a, Attributes b)
        {
            return new Attributes
            {
                Speed = a.Speed + b.Speed,
                AttackDamage = a.AttackDamage + b.AttackDamage,
                AttackSpeed = a.AttackSpeed + b.AttackSpeed,
                BulletSpeed = a.BulletSpeed + b.BulletSpeed,
                AttackRange = a.AttackRange + b.AttackRange,
                Luck = a.Luck + b.Luck,
                Health = a.Health + b.Health,
                MaxHealth = a.MaxHealth + b.MaxHealth
            };
        }

        public static Attributes operator *(Attributes a, Attributes b)
        {
            return new Attributes
            {
                Speed = a.Speed * b.Speed,
                AttackDamage = a.AttackDamage * b.AttackDamage,
                AttackSpeed = a.AttackSpeed * b.AttackSpeed,
                BulletSpeed = a.BulletSpeed * b.BulletSpeed,
                AttackRange = a.AttackRange * b.AttackRange,
                Luck = a.Luck * b.Luck,
                Health = a.Health * b.Health,
                MaxHealth = a.MaxHealth * b.MaxHealth
            };
        }

        public Attributes ApplyEffect(IEffectResult er)
        {
            var result = this;
            result += er.ApplyAdditionEffect(result);
            result *= er.ApplyPercentageEffect(result);
            result = er.ApplyMinMaxEffect(result);
            return result;
        }
        
        public Attributes ApplyEffects(List<IEffectResult> effectResults)
        {
            return ApplyEffect(new CombinedEffectResult(effectResults));
        }
    }
}