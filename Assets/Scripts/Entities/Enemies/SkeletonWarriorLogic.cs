using System;
using UnityEngine;

public class SkeletonWarriorLogic : EnemyLogic
{
    public override void Update()
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

}