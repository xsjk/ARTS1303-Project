using System.Collections.Generic;
using Items;
using Items.Implementation;
using Player;
using UnityEngine;

namespace Entities
{
    public class ShopCabinetLogic : MonoBehaviour, IInteractable
    {
        private ItemStack _itemStack;
        public Transform innerItemSpawnPoint;

        public void BindItemStack(IInstantiableItem item)
        {
            var itemStackContainer = new GameObject("ItemStack");
            itemStackContainer.transform.SetParent(transform);
            itemStackContainer.transform.position = innerItemSpawnPoint.position;
            _itemStack = itemStackContainer.AddComponent<ItemStack>();
            _itemStack.BindItems(new List<IInstantiableItem> { new ShopItem(item) });
        }

        public InteractableAction[] AvailableInteractions(PlayerComponents playerComponents)
        {
            if (_itemStack.Empty)
            {
                return new InteractableAction[] { };
            }

            var item = _itemStack.Peek();
            var disabled = playerComponents.Inventory.Coins < item.Cost;
            return new[]
            {
                new InteractableAction
                {
                    Key = KeyBinding.PrimaryInteraction,
                    Prompt = $"Buy {item.Name} for ${item.Cost} {(disabled ? "(Disabled)" : "")}",
                    Disabled = disabled
                }
            };
        }

        public void Interact(KeyBinding key, PlayerComponents playerComponents)
        {
            if (key != KeyBinding.PrimaryInteraction) return;
            playerComponents.Inventory.AddItem(_itemStack);
        }
    }
}