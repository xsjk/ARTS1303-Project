using UnityEngine;

namespace Items.Implementation
{
    public class ShopItem: IInstantiableItem
    {
        private readonly IInstantiableItem _item;
        public ShopItem(IInstantiableItem item)
        {
            _item = item;
        }

        public void OnEnterInventory(InventoryState state)
        {
            state.PickupCounts[Pickups.Coin] -= _item.Cost;
            _item.OnEnterInventory(state);
        }

        public GameObject Prefab => _item.Prefab;

        public string Name => _item.Name;

        public int Cost => _item.Cost;
    }
}