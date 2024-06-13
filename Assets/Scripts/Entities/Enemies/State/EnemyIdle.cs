using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdle : EnemyStateBase
{
    public override void OnEnter() {}

    public override void OnExit() {}
    public override void OnUpdate() {
        if (IsPlayerInChaseRange()) {
            enemy.ChangeState<EnemyChase>();
        }
    }
}
