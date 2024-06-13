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
    public WeaponLogic[] WeaponLogics;
    protected SkillConfig skillData;
    public bool canSwitch { get; protected set; } = true;
    public virtual void Init(CharacterController<T> character)
    {
        this.character = character;
        animator = GetComponent<Animator>();
        for (int i = 0; i < WeaponLogics.Length; i++)
        {
            WeaponLogics[i].Init(this);
        }
    }
    public override void PlayAudio(AudioClip audioClip)
    {
        character.PlayAudio(audioClip);
    }

    private int weaponIndex;
    public int currHitIndex;
    public void StartAttack(SkillConfig conf)
    {
        currHitIndex = 0;
        skillData = conf;
        canSwitch = false;
        animator.SetTrigger(skillData.triggerName);
        Spawn(skillData.releaseConfig.spawn);
        PlayAudio(skillData.releaseConfig.sound);
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

    private int currHurtAnimationIndex = 1;
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
        animator.SetTrigger("Hurt " + currHurtAnimationIndex);
        if (currHurtAnimationIndex == 1) currHurtAnimationIndex = 2;
        else currHurtAnimationIndex = 1;
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


    public void SetAnimation(string name, bool bl)
    {
        animator.SetBool(name, bl);
    }

    public void ResetWeapon()
    {
        for (int i = 0; i < WeaponLogics.Length; i++)
            WeaponLogics[i].StopSkillHit();
    }

    #region Animation Events

    private void ActivateSkillHit()
    {
        if (currHitIndex < skillData.hitConfigs.Length)
        {
            WeaponLogics[weaponIndex].StartSkillHit(skillData.hitConfigs[currHitIndex]);
            PlayAudio(skillData.hitConfigs[currHitIndex].sound);
            currHitIndex++;
        }
    }
    // Stop Skill Damage
    private void DeactivateSkillHit()
    {
        WeaponLogics[weaponIndex].StopSkillHit();
    }

    // private void SkillOver(string skillName)
    // {
    //     if (skillName == skillData.name)
    //     {
    //         SpawnObject(skillData.endModel.spawn);
    //         canSwitch = true;
    //         animator.SetTrigger(skillData.overTriggerName);
    //         currHitIndex = 0;
    //     }
    //     OnSkillOver();
    // }

    protected abstract void OnSkillOver();


    public void SkillCanSwitch()
    {
        canSwitch = true;
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
