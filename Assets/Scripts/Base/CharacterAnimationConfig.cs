using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[Serializable]
public class CharacterAnimationConfig
{

    [Header("Idle Config")]
    [Range(0, 1)]
    public float Idle_Default;
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

    public void InitAnimator(Animator animator)
    {
        animator.SetFloat("Idle_Default", Idle_Default);
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
    }
    
}
