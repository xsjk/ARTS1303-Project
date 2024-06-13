using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InitType = EnemyAnimationModel.InitType;

public class EnemySpawn : EnemyStateBase
{
    public override void OnEnter()
    {
        Debug.Log("EnemySpawn Enter" + " " + enemy.config.animationModel.initType);
        switch (enemy.config.animationModel.initType)
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
        if (InIdleAnimation())
        {
            enemy.ChangeState<EnemyIdle>();
        }
    }
}
