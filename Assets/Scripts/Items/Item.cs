using Effects;
using UnityEngine;

namespace Items
{
    public interface IItem
    {
        // Called when the item is picked up by the player
        // You should add the item to the player's inventory here
        public void OnEnterInventory(InventoryState state)
        {
        }

        public IEffect ProduceEffect()
        {
            return null;
        }
        
        public void OnPrimaryInteract()
        {
        }

        public void OnSecondaryInteract()
        {
            
        }
    }

    public interface IInstantiableItem : IItem
    {
        // The prefab of the item
        // This is used to instantiate the item in the world
        // For example, a coin pickup would have a coin prefab
        // This can also be generated from a texture
        public GameObject Prefab { get; }
        
        public string Name { get; }
        
        // The cost of the item in the shop
        // 0 if the item is not buyable
        public int Cost { get; }
    }
}