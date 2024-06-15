using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerConfig
{

    [Header("Base Config")]
    
    [SerializeField] public float ChaseRange = 10.0f;

    [SerializeField] public float AttackRange = 1.0f;

    [SerializeField] public float AttackCooldown = 3.0f;

    [SerializeField] public float AttackDamage = 1.0f;

    [SerializeField] public float Health = 1.0f;

    [SerializeField] public float PatrolSpeed = 1.0f;

    [SerializeField] public float PatrolCooldown = 5.0f;

    [SerializeField] public float ChaseSpeed = 10.0f;

    [SerializeField] public float TurnSpeed = 10.0f;

    [SerializeField] public float Acceleration = 4.0f;

    [SerializeField] public float Deceleration = 10.0f;

    [SerializeField] public AudioClip HurtSound;

    [SerializeField] public AudioClip DeathSound;
    
}

