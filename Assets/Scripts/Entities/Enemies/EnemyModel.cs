using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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


    [Tooltip("Initiation Type")]
    public InitType initType;

    public enum AwakeType {
        Awake,
        AwakeLong
    }
    [Tooltip("Awake Type (only for Initialization from Inactive State)")]
    public AwakeType awakeType;


    [Header("Idle Config")]
    [Range(0, 1)]
    public float Idle;
    [Range(0, 1)]
    public float Idle_Tired;
    [Range(0, 1)]
    public float Idle_Combat;
    
    [Range(0, 1)]
    public float Idle_Unarmed;
    
    [Range(0, 1)]
    public float Idle_2H_Melee;

    [Header("Walk Config")]
    [Range(0, 1)]
    public float Walk_A;
    [Range(0, 1)]
    public float Walk_B;
    [Range(0, 1)]
    public float Walk_C;
    [Range(0, 1)]
    public float Walk_D;

    [Header("Run Config")]
    [Range(0, 1)]
    public float Run_A;
    
    [Range(0, 1)]
    public float Run_B;
    
    [Range(0, 1)]
    public float Run_C;



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

    public void WakeUp(EnemyModel model) {
        Debug.Log("WakeUp " + awakeType);
        switch (awakeType) {
            case AwakeType.Awake:
                model.SetTrigger("Awake");
                break;
            case AwakeType.AwakeLong:
                model.SetTrigger("Awake Long");
                break;
        }
    }

    public void SetAnimator(Animator animator)
    {
        animator.SetFloat("Idle", Idle);
        animator.SetFloat("Idle_Tired", Idle_Tired);
        animator.SetFloat("Idle_Combat", Idle_Combat);
        animator.SetFloat("Idle_Unarmed", Idle_Unarmed);
        animator.SetFloat("Idle_2H_Melee", Idle_2H_Melee);
        
        animator.SetFloat("Walk_A", Walk_A);
        animator.SetFloat("Walk_B", Walk_B);
        animator.SetFloat("Walk_C", Walk_C);
        animator.SetFloat("Walk_D", Walk_D);
        
        animator.SetFloat("Run_A", Run_A);
        animator.SetFloat("Run_B", Run_B);
        animator.SetFloat("Run_C", Run_C);

        // for (int i = 0; i < animator.parameterCount; i++)
        // {
        //     if (animator.parameters[i].type == AnimatorControllerParameterType.Float)
        //     Debug.Log(animator.parameters[i].name + "=" + animator.GetFloat(animator.parameters[i].name));
        // }
    }
    
}

public class EnemyModel : CharacterModel<EnemyState>
{
    private EnemyController enemy;
    
    public override void Init(CharacterController<EnemyState> character)
    {
        base.Init(character);
        enemy = character as EnemyController;


        // the the animator
        enemy.config.animationModel.SetAnimator(animator);
        

    }

    protected override void OnSkillOver()
    {
        Debug.Log("OnSkillOver");
    }

    protected void SetMoveSpeed(Vector2 speed)
    {
        animator.SetFloat("SpeedX", speed.x);
        animator.SetFloat("SpeedY", speed.y);
    }


}
