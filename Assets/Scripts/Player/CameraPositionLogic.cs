using UnityEngine;
using UnityEngine.InputSystem;

public class CameraPositionLogic : MonoBehaviour
{
    private Vector2 _look;
    public float lookSpeed = 1.0f;

    private float _rotationX;
    private float _rotationY;

    public void OnLook(InputValue value)
    {
        _look = value.Get<Vector2>();
        _rotationX = Mathf.Clamp(_rotationX + _look.x, -60, 60);
        _rotationY = Mathf.Clamp(_rotationY + _look.y, 60, 60);
    }

    public void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, _rotationY, 0), Time.deltaTime);
    }
}