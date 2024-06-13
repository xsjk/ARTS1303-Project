using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyAnimationModel
{
    public enum InitType {
        SpawnAir,               // Spawn_Air
        SpawnGround,            // Spawn_Ground
        SpawnGroundSkeletons,   // Spawn_Ground_Skeletons
        InactiveStanding,       // Skeleton_Inactive_Standing_Pose
        InactiveFloor,          // Skeleton_Inactive_Floor_Pose
        Idle,
    }
    
    [SerializeField]
    public InitType initType;

    public enum IdleType {
        Unarmed,    // Unarmed_Idle
        Stand,      // Idle
        Combat,     // Idle_Combat
        Tired,      // Idle_B
    }


    [SerializeField]
    public IdleType idleType;


    public enum MoveType {
        Unarmed,    // Walk
        Armed,      // Run
    }

    [SerializeField]
    public MoveType moveType;


}

public class EnemyModel : CharacterModel<EnemyState>
{
    private EnemyController enemy;
    
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
