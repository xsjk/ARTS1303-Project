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
    public Dictionary<string, WeaponLogic> weaponLogics = new Dictionary<string, WeaponLogic>();
    protected SkillConfig curSkill;

    private void Awake() {
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
        animator = GetComponent<Animator>();
    }
    public override void PlayAudio(AudioClip audioClip)
    {
        character.PlayAudio(audioClip);
    }


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
        animator.SetTrigger(curSkill.overTriggerName);
        OnSkillOver();
        curSkill = null;
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
        if (curSkill != null)
        {
            animator.ResetTrigger(curSkill.triggerName);
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
        foreach (var weapon in weaponLogics)
            weapon.Value.Deactivate();
    }

    #region Animation Events

    protected abstract void OnSkillOver();


    public void SkillCanSwitch()
    {
    }

    public void CharacterMoveForAttack(int index)
    {
        if (index >= curSkill.characterMoveModels.Length)
            Debug.LogError("index out of range" + index + " " + curSkill.characterMoveModels.Length);
        CharacterMoveModel model = curSkill.characterMoveModels[index];
        character.CharacterAttackMove(model.offset, model.duration);
    }

    public void SpawnObj(int index)
    {
        Spawn(curSkill.spawns[index]);
    }
    #endregion


}
