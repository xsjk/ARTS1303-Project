using System.Linq;
using Entities;
using Items;
using UnityEngine;
using UnityEngine.UIElements;

namespace Player
{
    class InteractableEntryRender
    {
        private Image _keyImage;
        private Label _prompt;
    
        public void SetVisualElement(Image keyImage, Label prompt)
        {
            _keyImage = keyImage;
            _prompt = prompt;
        }
    
        public void SetInteractableAction(InteractableAction action)
        {
            _keyImage.image = KeyTextureManager.Instance.ActiveTexture[action.Key];
            _prompt.text = action.Prompt;
        }
    }
    class InteractableRender
    {
        private InteractableAction[] _availableActions = { };
        private InteractableAction[] _nextAvailableActions = { };
        private float _switchingProgress = 1.0f;
        private const float SwitchingSpeed = 1.0f;
        private readonly ListView _root;
        private readonly VisualTreeAsset _interactableActionTemplate;

        public InteractableRender(ListView root, VisualTreeAsset interactableActionTemplate)
        {
            _root = root;
            _interactableActionTemplate = interactableActionTemplate;
        }

        public void SetActions(InteractableAction[] availableActions, bool invalidate)
        {
            invalidate = invalidate || _availableActions.Length != availableActions.Length;
            if (!invalidate)
            {
                if (_switchingProgress < 0.5f)
                {
                    _nextAvailableActions = availableActions;
                }
                else
                {
                    _availableActions = availableActions;
                }
                return;
            }

            _nextAvailableActions = availableActions;
            if (_switchingProgress < 0.5f)
            {
                // we have not finished fading out the previous actions
                return;
            }
            // we have finished fading out the previous actions
            _switchingProgress = 1.0f - _switchingProgress;
        }

        public void Render()
        {
            _root.makeItem = () =>
            {
                var newListEntry = _interactableActionTemplate.Instantiate();
                var keyImage = newListEntry.Q<Image>("KeyImage");
                var prompt = newListEntry.Q<Label>("Prompt");
                var render = new InteractableEntryRender();
                render.SetVisualElement(keyImage, prompt);
                newListEntry.userData = render;
                return newListEntry;
            };

            _root.bindItem = (item, index) =>
            {
                (item.userData as InteractableEntryRender)?.SetInteractableAction(_availableActions[index]);
            };

            var nextSwitchingProgress = Mathf.Clamp01(_switchingProgress + SwitchingSpeed * Time.deltaTime);
            if(_switchingProgress < 0.5f && nextSwitchingProgress >= 0.5f)
            {
                _availableActions = _nextAvailableActions;
                nextSwitchingProgress = 0.5f;
            }

            _switchingProgress = nextSwitchingProgress;
            _root.style.opacity = Mathf.Abs(2 *_switchingProgress - 1);
            _root.fixedItemHeight = 80;
            _root.itemsSource = _availableActions;
        }
    }

    [RequireComponent(typeof(Inventory))]
    public class InteractableDetection : MonoBehaviour
    {
        private GameObject _activeGameObject;
        private IInteractable _activeInteractable;
        private InteractableAction[] _availableActions;
        private readonly Collider[] _colliders = new Collider[8];
        private UIDocument _uiDocument;
        private InteractableRender _interactableRender;
        private Inventory _playerInventory;


        public float detectionRadius = 2.0f;
        public float detectionOffset = 2.0f;
        public LayerMask detectionLayer;

        [SerializeField] public GameObject hud;
        [SerializeField] public VisualTreeAsset interactableActionTemplate;

        public void Start()
        {
            _uiDocument = hud.GetComponent<UIDocument>();
            _playerInventory = GetComponent<Inventory>();
            _interactableRender = new InteractableRender(_uiDocument.rootVisualElement.Q<ListView>("Interaction"), interactableActionTemplate);
        }

        private void OnInteraction(KeyBinding activeKey)
        {
            if (_activeInteractable is null)
            {
                return;
            }

            foreach (var action in _availableActions)
            {
                if (action.Key != activeKey) continue;
                if (action.Disabled) continue;
                _activeInteractable.Interact(action.Key, _playerInventory);
                return;
            }
        }

        public void OnPrimaryInteraction()
        {
            OnInteraction(KeyBinding.PrimaryInteraction);
        }

        public void OnSecondaryInteraction()
        {
            OnInteraction(KeyBinding.SecondaryInteraction);
        }

        private void Update()
        {
            var interactionPoint = transform.position + transform.forward * detectionOffset;
            var numberFound = Physics.OverlapSphereNonAlloc(interactionPoint, detectionRadius, _colliders, detectionLayer);
            if (numberFound == 0)
            {
                _interactableRender.SetActions(new InteractableAction[] { }, _activeGameObject is not null);
                _activeGameObject = null;
                _activeInteractable = null;
                _availableActions = null;
                return;
            }

            var c = _colliders.Take(numberFound).OrderBy(x => (transform.position - x.transform.position).magnitude)
                .First();
            if (c.gameObject == _activeGameObject)
            {
                _availableActions = _activeInteractable.AvailableInteractions();
                _interactableRender.SetActions(_availableActions, false);
                return;
            }

            _activeGameObject = c.gameObject;
            _activeInteractable = _activeGameObject.GetComponent<IInteractable>();
            _availableActions = _activeInteractable.AvailableInteractions();
            _interactableRender.SetActions(_availableActions, true);
        }

        private void LateUpdate()
        {
            _interactableRender.Render();
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + transform.forward * detectionOffset, detectionRadius);
        }
    }
}