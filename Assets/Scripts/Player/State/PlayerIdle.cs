using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : PlayerStateBase
{
    public override void OnEnter()
    {
        Debug.Log("PlayerIdle Enter");
        model.TriggerIdle();
    }

    public override void OnExit()
    {
        
    }

    public override void OnUpdate()
    {
        
    }
}
