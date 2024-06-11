using System;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    private Animator _animator;
    private IDungeonRoom _room;
    private GameObject _player;
    private Rigidbody _rigidbody;
    private AudioSource _audioSource;
    private float _attackCooldown;
    private float _patrolCooldown;
    private float _remainingHealth;

    [SerializeField] public float ViewRange = 20.0f;

    [SerializeField] public float AttackRange = 0.6f;

    [SerializeField] public float AttackCooldown = 3.0f;

    [SerializeField] public float AttackDamage = 1.0f;

    [SerializeField] public float Health = 1.0f;

    [SerializeField] public float PatrolSpeed = 1.0f;

    [SerializeField] public float PatrolCooldown = 5.0f;

    [SerializeField] public float ChaseSpeed = 10.0f;

    [SerializeField] public float TurnSpeed = 10.0f;

    [SerializeField] public float Acceleration = 4.0f;

    [SerializeField] public float Deceleration = 10.0f;

    [SerializeField] public AudioClip HurtSound;

    [SerializeField] public AudioClip DeathSound;

    public void Start()
    {
        _remainingHealth = Health;
        _animator = GetComponent<Animator>();
        _player = GameObject.FindWithTag("Player");
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void SetRoom(IDungeonRoom room)
    {
        _room = room;
    }

    public void Update()
    {
        if (_room is null || _player is null)
        {
            return;
        }

        if (_remainingHealth <= 0)
        {
            _rigidbody.velocity = Vector3.zero;
            return;
        }

        _patrolCooldown -= Time.deltaTime;
        _attackCooldown -= Time.deltaTime;

        var distanceToThePlayer = Vector3.Distance(_player.transform.position, transform.position);
        var seePlayer = distanceToThePlayer < ViewRange;
        _animator.SetBool("See Player", seePlayer);

        float targetSpeed;

        if (!seePlayer)
        {
            // Patrol in the room
            targetSpeed = PatrolSpeed;
            if (_patrolCooldown < 0)
            {
                // rotate clockwise
                transform.Rotate(0, 90, 0);
                _patrolCooldown = PatrolCooldown;
            }
        }
        else
        {
            // look at the player
            var targetRotation = Quaternion.LookRotation(_player.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, TurnSpeed * Time.deltaTime);
            targetSpeed = ChaseSpeed;

            // if we are close enough, attack
            _animator.SetBool("In Attack Range", distanceToThePlayer < AttackRange);
            if (distanceToThePlayer < AttackRange)
            {
                targetSpeed = 0;
                if (_attackCooldown < 0)
                {
                    _animator.SetTrigger("Attack Player");
                    _attackCooldown = AttackCooldown;
                    _player.GetComponent<PlayerLogic>().TakeDamage(AttackDamage);
                }
            }
        }

        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

        // Set the speed
        _rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, transform.forward * targetSpeed,
            (targetSpeed == 0 ? Deceleration : Acceleration) * Time.deltaTime);
    }

    public bool IsAlive()
    {
        return _remainingHealth != 0;
    }

    public void TakeDamage(float damage)
    {
        if (!IsAlive())
        {
            return;
        }

        Debug.Log($"Enemy took {damage} damage!");
        _remainingHealth -= damage;
        if (IsAlive())
        {
            _audioSource.PlayOneShot(HurtSound);
            return;
        }

        _audioSource.PlayOneShot(DeathSound);
        _animator.SetBool("Alive", false);
        _room.DecreaseEnemyCount();
        GetComponent<CapsuleCollider>().enabled = false;
    }
}