using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName ="Skill Configuration", menuName ="Config/Skill Config")]
public class SkillConfig : ScriptableObject
{

    [Header("Animation Config")]
    [Tooltip("The trigger name of the start animation of the skill")]
    public string triggerName;
    [Tooltip("The trigger name of the end animation of the skill")]
    public string overTriggerName;

    [Header("Camera Config")]
    [Tooltip("The camera move model when releasing the skill")]
    public CameraMoveConfig[] cameraMoveModels;

    [Header("Character Config")]
    [Tooltip("The character move model when releasing the skill")]
    public CharacterMoveModel[] characterMoveModels;

    [Header("Spawn Config")]
    public SpawnConfig[] spawns;

    [Header("Skill Config")]
    public SkillReleaseConfig releaseConfig;
    public SkillEndConfig endConfig;
    public WeaponConfig[] weaponConfigs;
}

[Serializable]
public class SkillReleaseConfig
{
    public SpawnConfig spawn;
    public AudioClip sound;
    public bool canRotate = true; // Whether the character can rotate when releasing the skill
    public bool canMove = true; // Whether the character can move when releasing the skill
}

[Serializable]
public class WeaponConfig
{
    public string weaponName;
    public float damageValue;
    public float hardTime;
    public Vector3 repelVelocity;
    public float repelTransitionTime;
    public HitEffect hitEffect;

    public SpawnConfig spawn;
    public AudioClip sound;
}

[Serializable]
public class SkillEndConfig
{
    public SpawnConfig spawn;
}

[Serializable]
public class SpawnConfig
{
    public GameObject prefab;
    public AudioClip sound;
    public Vector3 offset;
    public Vector3 rotation;
    public float delayTime;
}

[Serializable]
public class CameraMoveConfig
{
    public Vector3 offset;
    public float duration;
    public float backTime;

}

[Serializable]
public class CharacterMoveModel
{
    public Vector3 offset;
    public float duration;
}


