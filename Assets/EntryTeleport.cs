using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class EntryTeleport : MonoBehaviour
{
    [SerializeField] public GameObject teleportTarget;

    public void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        var player = other.gameObject;
        var relativeToTeleportCenter = player.transform.position - gameObject.transform.position;
        var angle = Vector3.Angle(relativeToTeleportCenter, transform.forward);

        if (angle > 90)
        {
            // var cameraLockTransform = player.transform.GetChild(0);
            // var currentCameraPosition = virtualCamera.State.RawPosition;
            // var relativeToCameraLock = currentCameraPosition - cameraLockTransform.position;

            var targetTransform = teleportTarget.transform;
            var relativeProjectedForward = Vector3.Dot(relativeToTeleportCenter, transform.forward);
            var relativeProjectedRight = Vector3.Dot(relativeToTeleportCenter, transform.right);
            var relativeProjectedUp = Vector3.Dot(relativeToTeleportCenter, transform.up);
            player.transform.position = teleportTarget.transform.position
                                        - targetTransform.forward * relativeProjectedForward
                                        - targetTransform.right * relativeProjectedRight
                                        + targetTransform.up * relativeProjectedUp;

            var playerRigidbody = player.GetComponent<Rigidbody>();
            var playerVelocity = playerRigidbody.velocity;
            var velocityProjectedForward = Vector3.Dot(playerVelocity, transform.forward);
            var velocityProjectedRight = Vector3.Dot(playerVelocity, transform.right);
            var velocityProjectedUp = Vector3.Dot(playerVelocity, transform.up);
            playerRigidbody.velocity = -targetTransform.forward * velocityProjectedForward
                                       - targetTransform.right * velocityProjectedRight
                                       + targetTransform.up * velocityProjectedUp;

            // align the rotation of the player
            // {
            //     var rotation = Quaternion.Inverse(transform.rotation) * targetTransform.rotation;
            //     var newRotation = Quaternion.Euler(0, 180, 0) * (rotation * virtualCamera.State.RawOrientation);
            //     // var newPosition = cameraLockTransform.position + newRotation * relativeToCameraLock;
            //     virtualCamera.ForceCameraPosition(cameraLockTransform.position, newRotation);
            // }
        }
    }
}