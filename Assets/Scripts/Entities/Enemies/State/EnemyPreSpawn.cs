using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InitType = EnemyAnimationModel.InitType;

public class EnemyPreSpawn : EnemyStateBase
{
    public override void OnEnter()
    {
        Debug.Log("EnemyPreSpawn Enter" + " " + enemy.animationConfig.initType);
        switch (enemy.animationConfig.initType)
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
        model.PauseAnimation();
    }

    public override void OnExit()
    {
        model.ContinueAnimation();
    }

    public override void OnUpdate()
    {
        if (IsPlayerInChaseRange())
        {
            enemy.ChangeState<EnemyChase>();
        }
    }
}
