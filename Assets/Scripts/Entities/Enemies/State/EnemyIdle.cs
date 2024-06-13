using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IdleType = EnemyAnimationModel.IdleType;

public class EnemyIdle : EnemyStateBase
{
    public override void OnEnter() {
        Debug.Log("EnemyIdle Enter" + " " + enemy.animationConfig.idleType);
        switch (enemy.animationConfig.idleType) {
            case IdleType.Unarmed:
                model.PlayAnimation("Unarmed_Idle");
                break;
            case IdleType.Stand:
                model.PlayAnimation("Idle");
                break;
            case IdleType.Combat:
                model.PlayAnimation("Idle_Combat");
                break;
            case IdleType.Tired:
                model.PlayAnimation("Idle_B");
                break;
        }
    }

    public override void OnExit() {}
    public override void OnUpdate() {
        if (IsPlayerInChaseRange()) {
            enemy.ChangeState<EnemyChase>();
        }
    }
}
