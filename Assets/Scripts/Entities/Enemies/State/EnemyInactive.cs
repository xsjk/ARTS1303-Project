using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InitType = EnemyAnimationModel.InitType;
public class EnemyInactive : EnemyStateBase
{
    public override void OnEnter()
    {
        Debug.Log("EnemyInactive Enter" + " " + enemy.config.animationModel.initType);
        switch (enemy.config.animationModel.initType)
        {
            case InitType.InactiveFloor:
                model.PlayAnimation("Skeleton_Inactive_Floor_Pose");
                break;
            case InitType.InactiveStanding:
                model.PlayAnimation("Skeleton_Inactive_Standing_Pose");
                break;
        }
    }

    public override void OnExit()
    {
        
    }

    public override void OnUpdate()
    {
        if (ShouldWakeUp())
        {
            enemy.ChangeState<EnemyAwaken>();
        }
        
    }
}
