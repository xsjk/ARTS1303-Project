using System.Linq;
using UnityEngine;

namespace Player
{
    public class InteractableDetection : MonoBehaviour
    {
        private GameObject _activeGameObject;
        private IInteractable _activeInteractable;
        private InteractableAction[] _availableActions;
        private readonly Collider[] _colliders = new Collider[8];

        public float detectionRadius = 2.0f;
        public float detectionOffset = 2.0f;
        public LayerMask detectionLayer;

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
                _activeInteractable.Interact(action.Key);
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
            var numFound = Physics.OverlapSphereNonAlloc(interactionPoint, detectionRadius, _colliders, detectionLayer);
            if (numFound == 0)
            {
                _activeGameObject = null;   
                _activeInteractable = null;
                _availableActions = null;
                return;
            }

            var c = _colliders.Take(numFound).OrderBy(x => (transform.position - x.transform.position).magnitude).First();
            if (c.gameObject == _activeGameObject)
            {        
                _availableActions = _activeInteractable.AvailableInteractions();
                return;
            }

            _activeGameObject = c.gameObject;
            _activeInteractable = _activeGameObject.GetComponent<IInteractable>();
            _availableActions = _activeInteractable.AvailableInteractions();
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + transform.forward * detectionOffset, detectionRadius);
        }
    }
}