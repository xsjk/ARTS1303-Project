using UnityEngine;

namespace Entities
{
    public class ChestLidRotationLogic : MonoBehaviour
    {
        private const float MaxRotation = -135;
        private Quaternion _maxRotationQuaternion;
        private Quaternion _initialRotationQuaternion;
        private bool _playerInRange;
        private GameObject _chestLid;

        public bool ChestItemTaken { get; private set; }

        public void Start()
        {
            _chestLid = transform.GetChild(0).gameObject;
            var oldRotation = _chestLid.transform.localRotation.eulerAngles;
            _initialRotationQuaternion = _chestLid.transform.localRotation;
            _maxRotationQuaternion = Quaternion.Euler(MaxRotation, oldRotation.y, oldRotation.z);
        }

        public void OnTriggerEnter(Collider other)
        {
            _playerInRange = true;
        }

        public void OnTriggerExit(Collider other)
        {
            _playerInRange = false;
        }

        public void FixedUpdate()
        {
            var target = _playerInRange || ChestItemTaken
                ? _maxRotationQuaternion
                : _initialRotationQuaternion;
            _chestLid.transform.localRotation =
                Quaternion.Lerp(_chestLid.transform.localRotation, target, Time.fixedDeltaTime);
        }

        public bool IsChestOpen()
        {
            return Quaternion.Angle(_initialRotationQuaternion, _chestLid.transform.localRotation) >= 90;
        }

        public void TakeItem()
        {
            ChestItemTaken = true;
        }
    }
}