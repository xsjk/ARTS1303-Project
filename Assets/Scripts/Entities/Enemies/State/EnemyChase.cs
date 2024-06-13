using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : EnemyStateBase
{
    public override void OnEnter()
    {
        Debug.Log("EnemyChase Enter");

        enemy.StartNavigation();

        // TODO Play Run Animation
        // model.SetAnimation("Run", true);
    }

    public override void OnExit()
    {
        enemy.StopNavigation();
        // model.SetAnimation("Run", false);
    }

    public override void OnUpdate()
    {
        if (!IsPlayerInChaseRange())
        {
            enemy.ChangeState<EnemyIdle>();
        }
        else if (IsPlayerInAttackRange())
        {
            enemy.ChangeState<EnemyAttack>();
        }
        else
        {
            enemy.SetNavigationTarget(player.transform.position);
        }
    }
}