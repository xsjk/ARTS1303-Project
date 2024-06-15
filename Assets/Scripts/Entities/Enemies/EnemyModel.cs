using UnityEngine;

public class EnemyModel : CharacterModel<EnemyState>
{
    private EnemyController enemy;
    public enum InitType {
        SpawnAir,               // Spawn_Air
        SpawnGround,            // Spawn_Ground
        SpawnGroundSkeletons,   // Spawn_Ground_Skeletons
        InactiveStanding,       // Skeleton_Inactive_Standing_Pose
        InactiveFloor,          // Skeleton_Inactive_Floor_Pose
        Idle,
    }
    [Tooltip("Initiation Type")]
    public InitType initType;

    public enum AwakeType {
        Awake,
        AwakeLong
    }
    [Tooltip("Awake Type (only for Initialization from Inactive State)")]
    public AwakeType awakeType;


    public void SetInitialState(EnemyController enemy)
    {
        switch(initType)
        {
            case InitType.SpawnAir:
            case InitType.SpawnGround:
            case InitType.SpawnGroundSkeletons:
                enemy.ChangeState<EnemyPreSpawn>(ignoreSame: false);
                break;
            case InitType.InactiveFloor:
            case InitType.InactiveStanding:
                enemy.ChangeState<EnemyInactive>(ignoreSame: false);
                break;
            case InitType.Idle:
                enemy.ChangeState<EnemyIdle>(ignoreSame: false);
                break;
        }
    }

    public void WakeUp() {
        Debug.Log("WakeUp " + awakeType);
        switch (awakeType) {
            case AwakeType.Awake:
            SetTrigger("Awake");
                break;
            case AwakeType.AwakeLong:
                SetTrigger("Awake Long");
                break;
        }
    }


    public override void Init(CharacterController<EnemyState> character)
    {
        base.Init(character);
        enemy = character as EnemyController;
    }

    protected override void OnSkillOver()
    {
        Debug.Log("OnSkillOver");
    }


}
