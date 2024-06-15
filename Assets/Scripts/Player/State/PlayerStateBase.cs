using UnityEngine;

public abstract class PlayerStateBase : StateBase<PlayerState>
{
    protected PlayerController player;
    protected PlayerModel model;
    protected override void OnInit(FSMController<PlayerState> controller, PlayerState stateType)
    {
        base.OnInit(controller, stateType);
        player = controller as PlayerController;
        model = player.model as PlayerModel;
    }

    protected bool ShouldWakeUp()
    {
        return IsPlayerInChaseRange();
    }
    
    protected bool ShouldSpawn()
    {
        return IsPlayerInChaseRange();
    }

    protected bool IsPlayerInChaseRange()
    {
        return DistanceToPlayer() < player.config.ChaseRange;
    }

    protected bool IsPlayerInAttackRange()
    {
        return DistanceToPlayer() < player.config.AttackRange;
    }

    protected float DistanceToPlayer()
    {
        return Vector3.Distance(player.transform.position, player.transform.position);
    }
}

