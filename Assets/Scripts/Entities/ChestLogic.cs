using System.Collections;
using UnityEngine;

namespace Entities
{
    public class ChestLogic : MonoBehaviour, IInteractable
    {
        [SerializeField] public GameObject innerItem;
        [SerializeField] public Shader shader;
        public float animationDuration = 2;
        private Material _innerItemMaterial;
        private ChestLidRotationLogic _chestLidRotationLogic;
        private static readonly int MainTex = Shader.PropertyToID("_MainTex");
        private static readonly int Progress = Shader.PropertyToID("_Progress");

        public void Start()
        {
            _chestLidRotationLogic = GetComponent<ChestLidRotationLogic>();
            var rendererComponent = innerItem.GetComponent<Renderer>();
            var texture = rendererComponent.sharedMaterial.GetTexture(MainTex);
            _innerItemMaterial = new Material(shader);
            _innerItemMaterial.SetTexture(MainTex, texture);
            rendererComponent.material = _innerItemMaterial;
        }

        public InteractableAction[] AvailableInteractions()
        {
            return new[]
            {
                new InteractableAction
                {
                    Key = KeyBinding.PrimaryInteraction,
                    Prompt = "Open Chest",
                    Disabled = !_chestLidRotationLogic.IsChestOpen() || _chestLidRotationLogic.ChestItemTaken
                }
            };
        }

        public void Interact(KeyBinding key)
        {
            if (key != KeyBinding.PrimaryInteraction) return;
            _chestLidRotationLogic.TakeItem();
            StartCoroutine(AnimateItemDestruction());
        }

        private IEnumerator AnimateItemDestruction()
        {
            float elapsedTime = 0;
            
            while(elapsedTime < animationDuration)
            {
                elapsedTime += Time.deltaTime;
                _innerItemMaterial.SetFloat(Progress, Mathf.Lerp(0, 1, elapsedTime / animationDuration));
                yield return null;
            }

            gameObject.layer = LayerMask.GetMask("Default");
            Destroy(innerItem);
            Destroy(this);
        }
    }
}