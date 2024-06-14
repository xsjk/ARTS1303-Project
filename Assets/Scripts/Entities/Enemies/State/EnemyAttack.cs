using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : EnemyStateBase
{
    private bool isAttack;

    private void LookAtPlayer()
    {
        // look at the player
        float TurnSpeed = 5f;
        var targetRotation = Quaternion.LookRotation(player.transform.position - enemy.transform.position);
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, TurnSpeed * Time.deltaTime);
        // Debug.Log(targetRotation);
    }

    public override void OnEnter()
    {
        Debug.Log("EnemyAttack Enter");
        enemy.StopNavigation();
        // LookAtPlayer();
        // enemy.transform.LookAt(player.transform);
        // enemy.Attack();
    }

    public override void OnExit()
    {
        model.DeactivateWeapons();
    }

    public override void OnUpdate()
    {
        LookAtPlayer();
        if (!IsPlayerInAttackRange())
        {
            enemy.ChangeState<EnemyChase>();
        }
        else if (!isAttack && InIdleAnimation())
        {
            Debug.Log("Attack");
            isAttack = enemy.Attack();
        }
        else if (InAttackAnimation())
        {
            isAttack = false;
        }
    }

}
