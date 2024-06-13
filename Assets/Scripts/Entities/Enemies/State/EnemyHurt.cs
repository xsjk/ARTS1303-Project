using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurt : EnemyStateBase
{
    private Vector3 repelVelocity;
    private float repelTime;
    private float currRepelTime;

    public void SetData(Transform souceTransform, Vector3 repelVelocity, float repelTransitionTime)
    {
        this.repelVelocity = souceTransform.TransformDirection(repelVelocity);
        this.repelTime = repelTransitionTime;
        this.currRepelTime = 0;
    }
    public override void OnEnter()
    {
        Debug.Log("EnemyHurt Enter");

        enemy.showHpBar();
    }

    public override void OnExit()
    {
        
    }

    private bool isStop = false;
    public override void OnUpdate()
    {
        if (isStop) return;

        if(currRepelTime < repelTime)
        {
            currRepelTime += Time.deltaTime;
            // 用hurtTime的时间 移动了 hurtVelocity 的距离
            enemy.moveMotion = repelVelocity / repelTime;
        }
        else
        {
            enemy.StopRepel();
            isStop = true;
        }

    }
}
