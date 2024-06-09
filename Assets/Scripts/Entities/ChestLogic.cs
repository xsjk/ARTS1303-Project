using System.Collections.Generic;
using Items;
using Items.Implementation;
using UnityEngine;

namespace Entities
{
    public class ChestLogic : MonoBehaviour, IInteractable
    {
        private ChestLidRotationLogic _chestLidRotationLogic;
        private ItemStack _itemStack;
        public Transform innerItemSpawnPoint;

        public void Start()
        {
            _chestLidRotationLogic = GetComponent<ChestLidRotationLogic>();
            BindItemStack(new List<IInstantiableItem>
            {
                new Coin(),
                new HealthBottle(),
                new Coin(),
            });
        }

        public void BindItemStack(List<IInstantiableItem> items)
        {
            var itemStackContainer = new GameObject("ItemStack");
            itemStackContainer.transform.SetParent(transform);
            itemStackContainer.transform.position = innerItemSpawnPoint.position;
            _itemStack = itemStackContainer.AddComponent<ItemStack>();
            _itemStack.BindItems(items);
        }

        public InteractableAction[] AvailableInteractions()
        {
            if (_itemStack.Empty)
            {
                _chestLidRotationLogic.TakeItem();
                return new InteractableAction[] { };
            }

            return new[]
            {
                new InteractableAction
                {
                    Key = KeyBinding.PrimaryInteraction,
                    Prompt = $"Take {_itemStack.Peek().Name}",
                    Disabled = !_chestLidRotationLogic.IsChestOpen() || !_itemStack.AllowInteract()
                }
            };
        }

        public void Interact(KeyBinding key, Inventory playerInventory)
        {
            if (key != KeyBinding.PrimaryInteraction) return;
            playerInventory.AddItem(_itemStack);
        }
    }
}