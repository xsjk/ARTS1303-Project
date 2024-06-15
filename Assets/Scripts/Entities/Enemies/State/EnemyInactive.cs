using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InitType = EnemyModel.InitType;
public class EnemyInactive : EnemyStateBase
{
    public override void OnEnter()
    {
        Debug.Log("EnemyInactive Enter" + " " + enemy.model.initType);
        switch (enemy.model.initType)
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
