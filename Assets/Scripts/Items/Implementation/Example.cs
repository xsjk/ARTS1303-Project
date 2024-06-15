using UnityEngine;

namespace Items.Implementation
{
    public class HealthBottle : IInstantiableItem
    {
        public GameObject Prefab { get; } = Resources.Load<GameObject>("Prefabs/Health Bottle");

        public string Name { get; } = "Health Bottle";
        public int Cost { get; } = 1;

        public void OnEnterInventory(InventoryState state)
        {
        }
    }
}