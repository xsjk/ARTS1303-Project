using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputLogic : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Animator _animator;
    private Vector2 _move;
    private PlayerLogic _playerLogic;

    public float maxSpeed = 12.0f; // how fast we run forward
    public float acceleration = 2.0f; // how fast we can accelerate
    public float directionControl = 2.0f; // how fast we can change direction
    public float attackCooldown = 1f; // how long we have to wait between attacks
    private float _speedXZ; // our current speed
    private float _attackCooldown; // how long we have to wait before we can attack again


    public void OnMove(InputValue value)
    {
        _move = value.Get<Vector2>();
    }

    public void OnAttack()
    {
        if (_attackCooldown <= 0)
        {
            _attackCooldown = attackCooldown;
            _animator.SetTrigger("IsFiring");
            _playerLogic.DealDamageToEnemy();
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
        var rigidbodyVelocity = _rigidbody.velocity;
        // find the direction to move in, based on the direction inputs
        Vector3 velocityDirection = trans.forward * _move.y + trans.right * _move.x;

        // if we are no longer pressing and input, carry on moving in the last direction we were set to move in
        if (_move == Vector2.zero)
            velocityDirection = rigidbodyVelocity;

        velocityDirection.y = 0;
        velocityDirection = velocityDirection.normalized;

        // apply adjustment to acceleration
        float localAcceleration = directionControl;

        // apply the speed along the XZ plane to the direction
        var velocity = velocityDirection * _speedXZ;
        velocity.y = rigidbodyVelocity.y;

        // apply the velocity to the rigidbody
        _rigidbody.velocity = Vector3.Lerp(rigidbodyVelocity, velocity, deltaTime * localAcceleration);
        var rigidbodyVelocityXZ = new Vector2(rigidbodyVelocity.x, rigidbodyVelocity.z).magnitude;
        _animator.SetFloat("SpeedX", rigidbodyVelocityXZ / _speedXZ * 2);
        _animator.SetFloat("SpeedZ", rigidbodyVelocityXZ / _speedXZ * 2);
        // Debug.Log(_rigidbody.velocity);
    }

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _playerLogic = GetComponent<PlayerLogic>();
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