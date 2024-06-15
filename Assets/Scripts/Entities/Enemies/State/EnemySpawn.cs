using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InitType = EnemyModel.InitType;

public class EnemySpawn : EnemyStateBase
{
    public override void OnEnter()
    {
        Debug.Log("EnemySpawn Enter" + " " + enemy.model.initType);
        switch (enemy.model.initType)
        {
            case InitType.SpawnAir:
                model.PlayAnimation("Spawn_Air");
                break;
            case InitType.SpawnGround:
                model.PlayAnimation("Spawn_Ground");
                break;
            case InitType.SpawnGroundSkeletons:
                model.PlayAnimation("Spawn_Ground_Skeletons");
                break;
        }
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
