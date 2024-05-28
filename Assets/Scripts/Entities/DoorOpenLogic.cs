using UnityEngine;

namespace Entities
{
    public class DoorOpenLogic : MonoBehaviour
    {
        private static readonly float MaxRotationAngle = 90.0f;
        private static readonly float DoorRotationSpeed = 2.0f;
        private float _angle;
        [SerializeField] public GameObject door;

        private bool IsLocked()
        {
            return CombatStateManager.InCombat;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }

            if (IsLocked())
            {
                return;
            }

            var direction = other.gameObject.transform.position - transform.position;
            _angle = Vector3.Angle(direction, transform.forward) > 90 ? MaxRotationAngle : -MaxRotationAngle;
        }

        public void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }

            if (IsLocked())
            {
                return;
            }

            _angle = 0;
        }

        public void FixedUpdate()
        {
            door.transform.localRotation = Quaternion.Lerp(door.transform.localRotation,
                Quaternion.Euler(0, _angle, 0),
                DoorRotationSpeed * Time.fixedDeltaTime);
        }
    }
}