using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(CharacterController))]

public abstract class CharacterModel : MonoBehaviour
{
    [SerializeField]
    public CharacterAnimationConfig config;

    private Animator animator;
    private AudioSource audioSource;
    private CharacterController characterController;
    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        config.InitAnimator(animator);
    }
    
    #region Animator Control
    public void SetBool(string name, bool bl)
    {
        animator.SetBool(name, bl);
    }

    public void SetTrigger(string name)
    {
        animator.SetTrigger(name);
    }

    public void SetFloat(string name, float value)
    {
        animator.SetFloat(name, value);
    }
    public void ResetTrigger(string name)
    {
        animator.ResetTrigger(name);
    }

    public void PlayAnimation(string name, int layer = 0, float normalizedTime = 0)
    {
        animator.Play(name, layer, normalizedTime);
    }
    
    public void PauseAnimation()
    {
        animator.speed = 0;
    }

    public void ContinueAnimation()
    {
        animator.speed = 1;
    }

    public int GetCurrentAnimationTag() {
        return animator.GetCurrentAnimatorStateInfo(0).tagHash;
    }

    #endregion

    #region Audio Control
    public virtual void PlayAudio(AudioClip audioClip)
    {
        if (audioClip != null)
            audioSource.PlayOneShot(audioClip); 
    }
    #endregion

    #region Movement Control
    public void Move(Vector3 displacement)
    {
        characterController.Move(displacement);
    }
    public bool IsGrounded()
    {
        return characterController.isGrounded;
    }
    public void CharacterAttackMove(Vector3 target, float time)
    {
        StartCoroutine(DoCharacterAttackMove(transform.TransformDirection(target), time));
    }

    protected IEnumerator DoCharacterAttackMove(Vector3 target, float time)
    {
        characterController.enabled = true;
        float currTime = 0;
        while (currTime < time)
        {
            Vector3 moveDir = target * Time.deltaTime / time;
            characterController.Move(moveDir);
            currTime += Time.deltaTime;
            yield return null;
        }
        characterController.enabled = false;
    }
    public void CharacterMoveForAttack(CharacterMoveModel model)
    {
        CharacterAttackMove(model.offset, model.duration);
    }

    #endregion

    public virtual void Spawn(SpawnConfig spawn)
    {
        if (spawn != null && spawn.prefab != null)
        {
            StartCoroutine(_SpawnObject(spawn));
        }
    }

    private IEnumerator _SpawnObject(SpawnConfig spawn)
    {
        yield return new WaitForSeconds(spawn.delayTime);
        Vector3 spawnPosition = transform.position + spawn.offset;
        Quaternion spawnQuaternion = Quaternion.Euler(spawn.rotation);

        var temp = Instantiate(spawn.prefab, spawnPosition, spawnQuaternion);
        PlayAudio(spawn.sound);
    }
}


public enum AnimationType
{
    Idle,
    Attack,
    Hurt,
    Dead
}


public abstract class CharacterModel<T> : CharacterModel
{
    private CharacterController<T> character;
    public Dictionary<string, WeaponLogic> weaponLogics = new Dictionary<string, WeaponLogic>();
    protected SkillConfig curSkill;

    protected override void Awake() {
        base.Awake();
        // find weapon names and weapon logics in children and add to the dictionary
        foreach (var weapon in GetComponentsInChildren<WeaponLogic>()) {
            weaponLogics.Add(weapon.name, weapon);
            weapon.Init(this);
            Debug.Log("Weapon name: " + weapon.name + " Weapon logic: " + weaponLogics[weapon.name]);
        }
    }

    public virtual void Init(CharacterController<T> character)
    {
        this.character = character;
    }

    #region Skill and Weapon Control
    public void ActivateWeapons(SkillConfig conf)
    {
        foreach (var c in conf.weaponConfigs) {
            if (weaponLogics.ContainsKey(c.weaponName))
                weaponLogics[c.weaponName].Activate(c);
            else
                throw new System.Exception("Weapon name not found: " + c.weaponName);
        }
    }
    public void DeactivateWeapons()
    {
        foreach (var weapon in weaponLogics)
        {
            weapon.Value.Deactivate();
        }
    }
    
    public void TriggerSkill(SkillConfig conf)
    {
        SetTrigger(conf.triggerName);
        Spawn(conf.releaseConfig.spawn);
        PlayAudio(conf.releaseConfig.sound);
        curSkill = conf;
    }
    
    public void FinalizeSkill()
    {
        Spawn(curSkill.endConfig.spawn);
        SetTrigger(curSkill.overTriggerName);
        OnSkillOver();
        curSkill = null;
    }

    #endregion


    #region Animation Control

    public void SetMoveVelocity(Vector2 speed)
    {
        SetFloat("SpeedX", speed.x);
        SetFloat("SpeedY", speed.y);
    }

    public bool InAnimation(AnimationType type)
    {
        return GetCurrentAnimationTag() == Animator.StringToHash(type.ToString());
    }

    public void TriggerIdle() {
        SetTrigger("Idle");
    }

    private int _currHurtAnimationIndex = 1;
    public void PlayHurtAnimtion(bool isFloat = true)
    {
        if (curSkill != null)
        {
            ResetTrigger(curSkill.triggerName);
        }
        if (isFloat)
        {
            SetTrigger("Knock");
        }
        SetTrigger("Hurt " + _currHurtAnimationIndex);
        if (_currHurtAnimationIndex == 1) _currHurtAnimationIndex = 2;
        else _currHurtAnimationIndex = 1;
    }

    public void StopHurtAnimtion()
    {
        throw new System.Exception("StopHurtAnimtion is not implemented");
    }
    public void PlayDeadAnimation()
    {
        SetTrigger("Dead");
    }

    public void ResetWeapon()
    {
        foreach (var weapon in weaponLogics)
            weapon.Value.Deactivate();
    }

    #endregion

    #region Animation Events

    protected abstract void OnSkillOver();


    public void SkillCanSwitch()
    {
    }

    public void SpawnObj(int index)
    {
        Spawn(curSkill.spawns[index]);
    }
    #endregion


}
