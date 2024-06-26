using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAwaken : EnemyStateBase
{
    public override void OnEnter()
    {
        Debug.Log("EnemyAwaken Enter");
        model.WakeUp();
    }

    public override void OnExit()
    {
        
    }

    public override void OnUpdate()
    {
        if (model.InAnimation(AnimationType.Idle))
        {
            enemy.ChangeState<EnemyIdle>();
        }
    }
}
