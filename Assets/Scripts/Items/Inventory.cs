using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    // Pickups are items those secondary items that can be picked up by the player
    // See https://bindingofisaacrebirth.fandom.com/wiki/Pickups
    public enum Pickups
    {
        Coin,
    }

    public class InventoryState
    {
        public readonly Dictionary<Pickups, int> PickupCounts = new();

        // Items are the primary items that can be picked up by the player
        // There are two types of items: passive and active
        // Passive items provide a benefit to the player without having to be activated
        // Active items must be activated by the player to provide their benefit
        // Only one active item can be held at a time
        // See https://bindingofisaacrebirth.fandom.com/wiki/Items
        public readonly List<IItem> Items = new();
        public IItem ActiveItem = null;

        public InventoryState()
        {
            // add all kinds of pickups to the dictionary
            foreach (Pickups pickup in System.Enum.GetValues(typeof(Pickups)))
                PickupCounts.Add(pickup, 0);
        }
    }

    public class Inventory : MonoBehaviour, IItem
    {
        private readonly InventoryState _state = new();
        
        public int Coins => _state.PickupCounts[Pickups.Coin];

        public void AddItem(IItem item)
        {
            item.OnEnterInventory(_state);
        }
    }
}