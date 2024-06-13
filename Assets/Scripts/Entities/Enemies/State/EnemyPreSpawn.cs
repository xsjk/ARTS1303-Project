using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InitType = EnemyAnimationModel.InitType;

public class EnemyPreSpawn : EnemyStateBase
{
    public override void OnEnter()
    {
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        if (ShouldSpawn())
            enemy.ChangeState<EnemySpawn>();
    }
}
