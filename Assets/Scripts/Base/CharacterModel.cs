using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterModel : MonoBehaviour
{
    public abstract void PlayAudio(AudioClip audioClip);
    public abstract void Spawn(SpawnConfig spawn);
}



[RequireComponent(typeof(Animator))]
public abstract class CharacterModel<T> : CharacterModel
{
    private CharacterController<T> character;
    protected Animator animator;
    public WeaponLogic[] weaponLogics;
    protected SkillConfig skillData;
    public virtual void Init(CharacterController<T> character)
    {
        this.character = character;
        animator = GetComponent<Animator>();
        for (int i = 0; i < weaponLogics.Length; i++)
        {
            weaponLogics[i].Init(this);
        }
    }
    public override void PlayAudio(AudioClip audioClip)
    {
        character.PlayAudio(audioClip);
    }

    private int _weaponIndex;
    public int currHitIndex;
    public void StartAttack(SkillConfig conf)
    {
        currHitIndex = 0;
        skillData = conf;
        SetTrigger(skillData.triggerName);
        Spawn(skillData.releaseConfig.spawn);
        PlayAudio(skillData.releaseConfig.sound);
    }
    
    private void EndAttack(string skillName)
    {
        if (skillName == skillData.name)
        {
            Spawn(skillData.endConfig.spawn);
            animator.SetTrigger(skillData.overTriggerName);
            currHitIndex = 0;
        }
        OnSkillOver();
    }


    public override void Spawn(SpawnConfig spawn)
    {
        if (spawn != null && spawn.prefab != null)
        {
            StartCoroutine(_SpawnObject(spawn));
        }
    }

    private IEnumerator _SpawnObject(SpawnConfig spawn)
    {
        yield return new WaitForSeconds(spawn.delayTime);
        var temp = Instantiate(spawn.prefab);
        temp.transform.position = transform.position;
        temp.transform.eulerAngles = transform.eulerAngles;
        temp.transform.Translate(spawn.offset, Space.Self);
        temp.transform.eulerAngles += spawn.rotation;
        PlayAudio(spawn.sound);
    }

    private int _currHurtAnimationIndex = 1;
    public void PlayHurtAnimtion(bool isFloat = true)
    {
        if (skillData != null)
        {
            animator.ResetTrigger(skillData.triggerName);
        }
        if (isFloat)
        {
            animator.SetTrigger("Knock");
        }
        animator.SetTrigger("Hurt " + _currHurtAnimationIndex);
        if (_currHurtAnimationIndex == 1) _currHurtAnimationIndex = 2;
        else _currHurtAnimationIndex = 1;
    }

    public void StopHurtAnimtion()
    {
        animator.SetTrigger("HurtOver");
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

    public void PlayDeadAnimation()
    {
        animator.SetTrigger("Dead");
    }


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

    public int GetCurrentAnimationTag() {
        return animator.GetCurrentAnimatorStateInfo(0).tagHash;
    }

    public void ResetWeapon()
    {
        for (int i = 0; i < weaponLogics.Length; i++)
            weaponLogics[i].StopSkillHit();
    }

    #region Animation Events

    private void ActivateSkillHit()
    {
        if (currHitIndex < skillData.hitConfigs.Length)
        {
            weaponLogics[_weaponIndex].StartSkillHit(skillData.hitConfigs[currHitIndex]);
            PlayAudio(skillData.hitConfigs[currHitIndex].sound);
            currHitIndex++;
        }
    }
    // Stop Skill Damage
    private void DeactivateSkillHit()
    {
        weaponLogics[_weaponIndex].StopSkillHit();
    }
    protected abstract void OnSkillOver();


    public void SkillCanSwitch()
    {
    }

    public void CharacterMoveForAttack(int index)
    {
        if (index >= skillData.characterMoveModels.Length)
            Debug.LogError("index out of range" + index + " " + skillData.characterMoveModels.Length);
        CharacterMoveModel model = skillData.characterMoveModels[index];
        character.CharacterAttackMove(model.offset, model.duration);
    }

    public void SpawnObj(int index)
    {
        Spawn(skillData.spawns[index]);
    }
    #endregion


}
