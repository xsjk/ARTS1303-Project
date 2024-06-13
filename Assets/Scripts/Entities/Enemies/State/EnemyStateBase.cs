using UnityEngine;

public abstract class EnemyStateBase : StateBase<EnemyState>
{
    protected EnemyController enemy;
    protected EnemyModel model;
    protected GameObject player = GameObject.FindWithTag("Player");
    protected override void OnInit(FSMController<EnemyState> controller, EnemyState stateType)
    {
        base.OnInit(controller, stateType);
        enemy = controller as EnemyController;
        model = enemy.model as EnemyModel;
    }

    protected bool IsPlayerInChaseRange()
    {
        return DistanceToPlayer() < enemy.ChaseRange;
    }

    protected bool IsPlayerInAttackRange()
    {
        return DistanceToPlayer() < enemy.AttackRange;
    }

    protected float DistanceToPlayer()
    {
        return Vector3.Distance(player.transform.position, enemy.transform.position);
    }
}

