using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : EnemyStateBase
{
    public override void OnEnter()
    {
        Debug.Log("EnemyAttack Enter");
        enemy.StopNavigation();
    }

    public override void OnExit()
    {
        
    }

    public override void OnUpdate()
    {
        if (!IsPlayerInAttackRange())
        {
            enemy.ChangeState<EnemyChase>();
        }
    }
}
