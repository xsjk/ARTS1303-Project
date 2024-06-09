using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Items.Implementation
{
    internal struct ItemStackItem
    {
        public GameObject GameObject;
        public IInstantiableItem Item;
        public Material Material;
    }

    public class ItemStack : MonoBehaviour, IItem
    {
        private Queue<ItemStackItem> _items;
        private Shader _shader;
        private static readonly int MainTex = Shader.PropertyToID("_MainTex");
        private static readonly int Progress = Shader.PropertyToID("_Progress");
        private const float AnimationDuration = 1;
        private bool _allowInteract = true;

        protected void Awake()
        {
            _shader = Resources.Load<Shader>("Shaders/Dissolve");
        }

        public void BindItems(List<IInstantiableItem> items)
        {
            _items = new Queue<ItemStackItem>();
            foreach (var item in items)
            {
                var itemObject = Instantiate(item.Prefab, transform);
                var rendererComponent = itemObject.GetComponent<Renderer>();
                var texture = rendererComponent.sharedMaterial.GetTexture(MainTex);
                var material = new Material(_shader);
                material.SetTexture(MainTex, texture);
                material.SetFloat(Progress, 1);
                rendererComponent.material = material;
                _items.Enqueue(new ItemStackItem
                {
                    GameObject = itemObject,
                    Item = item,
                    Material = material
                });
            }

            _items.Peek().Material.SetFloat(Progress, 0);
        }

        public bool Empty => _items.Count == 0;

        public IInstantiableItem Peek()
        {
            return _items.Peek().Item;
        }

        public bool AllowInteract()
        {
            return !Empty && _allowInteract;
        }

        public void OnEnterInventory(InventoryState state)
        {
            Assert.IsTrue(_allowInteract);
            _allowInteract = false;

            StartCoroutine(AnimateItemDestruction(state));
        }

        private IEnumerator AnimateItemDestruction(InventoryState state)
        {
            var currentItem = _items.Peek();
            
            float elapsedTime = 0;

            while (elapsedTime < AnimationDuration)
            {
                elapsedTime += Time.deltaTime;
                currentItem.Material.SetFloat(Progress, Mathf.Lerp(0, 1, elapsedTime / AnimationDuration));
                yield return null;
            }

            Destroy(currentItem.GameObject);
            _items.Dequeue();
            currentItem.Item.OnEnterInventory(state);

            if (Empty) yield break;

            var nextItem = _items.Peek();
            elapsedTime = 0;
            while (elapsedTime < AnimationDuration)
            {
                elapsedTime += Time.deltaTime;
                nextItem.Material.SetFloat(Progress, Mathf.Lerp(1, 0, elapsedTime / AnimationDuration));
                yield return null;
            }

            _allowInteract = true;
        }
    }
}