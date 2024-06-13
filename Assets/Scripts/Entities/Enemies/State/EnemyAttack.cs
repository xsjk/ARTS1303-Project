using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : EnemyStateBase
{
    private bool isAttack;
    public override void OnEnter()
    {
        Debug.Log("EnemyAttack Enter");
        enemy.StopNavigation();
        enemy.Attack();
    }

    public override void OnExit()
    {
        model.DeactivateWeapons();
    }

    public override void OnUpdate()
    {
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
