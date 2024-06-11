using System.Collections.Generic;
using Effects;
using UnityEngine;

namespace Items.Implementation
{
    internal class CoinEffect : IEffect
    {
        public bool Hidden { get; } = false;
        public string Name { get; } = "Coin";
        public Texture Icon { get; } = Resources.Load<Texture>("Textures/Items/Coin/CoinEffectTexture");

        public IEffectResult PerformEffect()
        {
            return new CombinedEffectResult(new List<IEffectResult>
            {
                new AdditionEffectResult(new Attributes
                {
                    AttackDamage = 1,
                }),
                new PercentageEffectResult(new Attributes
                {
                    AttackDamage = 0.1f,
                }),
            });
        }

        public bool EffectEnded(float deltaTime)
        {
            return false;
        }
    }

    public class Coin : IInstantiableItem
    {
        public GameObject Prefab { get; } = Resources.Load<GameObject>("Prefabs/coin");

        public string Name { get; } = "1 coin";

        public IEffect ProduceEffect()
        {
            return new CoinEffect();
        }

        public void OnEnterInventory(InventoryState state)
        {
            state.PickupCounts[Pickups.Coin] += 1;
        }
    }
}