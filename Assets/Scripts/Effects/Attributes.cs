using UnityEngine;
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

        public static Attributes PositiveInfinity = new Attributes
        {
            Speed = float.PositiveInfinity,
            AttackDamage = float.PositiveInfinity,
            AttackSpeed = float.PositiveInfinity,
            BulletSpeed = float.PositiveInfinity,
            AttackRange = float.PositiveInfinity,
            Luck = float.PositiveInfinity,
            Health = float.PositiveInfinity,
            MaxHealth = float.PositiveInfinity
        };

        public static Attributes NegativeInfinity = new Attributes
        {
            Speed = float.NegativeInfinity,
            AttackDamage = float.NegativeInfinity,
            AttackSpeed = float.NegativeInfinity,
            BulletSpeed = float.NegativeInfinity,
            AttackRange = float.NegativeInfinity,
            Luck = float.NegativeInfinity,
            Health = float.NegativeInfinity,
            MaxHealth = float.NegativeInfinity
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

        public static Attributes ApplyMin(Attributes a, Attributes b)
        {
            return new Attributes
            {
                Speed = Mathf.Min(a.Speed, b.Speed),
                AttackDamage = Mathf.Min(a.AttackDamage, b.AttackDamage),
                AttackSpeed = Mathf.Min(a.AttackSpeed, b.AttackSpeed),
                BulletSpeed = Mathf.Min(a.BulletSpeed, b.BulletSpeed),
                AttackRange = Mathf.Min(a.AttackRange, b.AttackRange),
                Luck = Mathf.Min(a.Luck, b.Luck),
                Health = Mathf.Min(a.Health, b.Health),
                MaxHealth = Mathf.Min(a.MaxHealth, b.MaxHealth),
            };
        }

        public static Attributes ApplyMax(Attributes a, Attributes b)
        {
            return new Attributes
            {
                Speed = Mathf.Max(a.Speed, b.Speed),
                AttackDamage = Mathf.Max(a.AttackDamage, b.AttackDamage),
                AttackSpeed = Mathf.Max(a.AttackSpeed, b.AttackSpeed),
                BulletSpeed = Mathf.Max(a.BulletSpeed, b.BulletSpeed),
                AttackRange = Mathf.Max(a.AttackRange, b.AttackRange),
                Luck = Mathf.Max(a.Luck, b.Luck),
                Health = Mathf.Max(a.Health, b.Health),
                MaxHealth = Mathf.Max(a.MaxHealth, b.MaxHealth),
            };
        }

        public Attributes ApplyEffect(IEffectResult effectResult)
        {
            var result = this;
            result += effectResult.ApplyAdditionEffect();
            result *= effectResult.ApplyPercentageEffect();
            var (min, max) = effectResult.ApplyMinMaxEffect();
            result = ApplyMin(result, max);
            result = ApplyMax(result, min);
            return result;
        }
        
        public Attributes ApplyEffects(List<IEffectResult> effectResults)
        {
            return ApplyEffect(new CombinedEffectResult(effectResults));
        }
    }
}