using UnityEngine;
using UnityEngine.InputSystem;

public class InputLogic : MonoBehaviour
{
    private Vector2 _move;
    private PlayerController controller;

    public float maxSpeed => controller.attributes.Speed;
    public float acceleration = 2.0f; // how fast we can accelerate
    public float directionControl = 2.0f; // how fast we can change direction
    public float attackCooldown = 1f; // how long we have to wait between attacks
    private float _speedXZ; // our current speed
    private float _attackCooldown; // how long we have to wait before we can attack again
    private Vector3 _velocity;

    public void OnMove(InputValue value)
    {
        _move = value.Get<Vector2>();
    }

    public void OnAttack()
    {
        if (_attackCooldown <= 0)
        {
            _attackCooldown = attackCooldown;
            controller.ChangeState<PlayerAttack>(ignoreSame: false);
        }
    }

    private void AlignRotation()
    {
        if (Camera.main is null)
        {
            return;
        }

        var y = Camera.main.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0, y, 0);
    }

    private void LerpSpeed(float deltaTime)
    {
        var targetSpeed = maxSpeed;
        // multiply our speed by our input amount
        targetSpeed *= _move.magnitude;
        float localAcceleration = acceleration;
        // lerp by a factor of our acceleration
        _speedXZ = Mathf.Lerp(_speedXZ, targetSpeed, deltaTime * localAcceleration);
    }

    private void Move(float deltaTime)
    {
        var trans = transform;
        // find the direction to move in, based on the direction inputs
        Vector3 velocityDirection = trans.forward * _move.y + trans.right * _move.x;

        // if we are no longer pressing and input, carry on moving in the last direction we were set to move in
        if (_move == Vector2.zero)
            velocityDirection = _velocity;

        velocityDirection.y = 0;
        velocityDirection = velocityDirection.normalized;

        // apply the speed along the XZ plane to the direction
        _velocity.x = velocityDirection.x * _speedXZ;
        _velocity.z = velocityDirection.z * _speedXZ;

        // apply gravity
        if (controller.model.IsGrounded()) 
        {
            _velocity.y = 0;
        }
        else
        {
            _velocity.y += Physics.gravity.y * deltaTime;
        } 

        // move the character controller
        controller.model.Move(_velocity * deltaTime);
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        _attackCooldown -= Time.deltaTime;
        AlignRotation();
        LerpSpeed(Time.deltaTime);
        Move(Time.deltaTime);
    }
}
