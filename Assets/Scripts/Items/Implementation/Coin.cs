using UnityEngine;

namespace Items.Implementation
{
    public class Coin : IInstantiableItem
    {
        public GameObject Prefab { get; } = Resources.Load<GameObject>("Prefabs/coin");

        public string Name { get; } = "1 coin";

        public void OnEnterInventory(InventoryState state)
        {
            state.PickupCounts[Pickups.Coin] += 1;
        }
    }
}