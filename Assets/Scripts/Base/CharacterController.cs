using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public abstract class CharacterController<T> : FSMController<T>
{
    public CharacterController characterController { get; protected set; } // only used for characterMoveModels
    public CharacterModel<T> model { get; protected set; }

    public float maxHp = 1;
    protected float hp;
    public bool isDead = false;
    public abstract float Hp { get; set; }

    public SkillModel[] SkillModels;


    protected virtual void Awake()
    {
        hp = maxHp;
        model = transform.GetComponent<CharacterModel<T>>();
        if (model == null)
            throw new Exception("CharacterModel is not found in " + name);
        model.Init(this);
        characterController = GetComponent<CharacterController>();
        gameObject.AddComponent<HurtEnter>().action = Hurt;
    }

    protected override void Update()
    {
        base.Update();
        UpdateSkillCD();
    }

    private void UpdateSkillCD()
    {
        for (int i = 0; i < SkillModels.Length; i++)
        {
            SkillModels[i].Update();
        }
    }

    public void Hurt(float hardTime, Transform souceTransform, Vector3 repelVelocity, float repelTransitionTime, float damgeValue)
    {
        if (isDead) return;
        Hp -= damgeValue;
        if (Hp <= 0)
        {
            Dead();
        }
        else
        {
            model.PlayHurtAnimtion(repelVelocity.y > 0.5f);
            CancelInvoke("Hurt Done");
            Invoke("Hurt Done", hardTime);
            OnHurt(souceTransform, repelVelocity, repelTransitionTime);
        }
        model.SkillCanSwitch();
        model.ResetWeapon();
    }

    protected abstract void OnHurt(Transform souceTransform, Vector3 repelVelocity, float repelTransitionTime);



    private void HurtOver()
    {
        model.StopHurtAnimtion();
        OnHurtOver();
    }

    protected abstract void OnHurtOver();

    private void Dead()
    {
        isDead = true;
        model.PlayDeadAnimation();
        OnDead();
    }
    protected abstract void OnDead();

    public SkillConfig CurrSkillData { get; protected set; }



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

    public void PlayAudio(AudioClip audioClip)
    {
        if (audioClip == null)
            return;
        GetComponent<AudioSource>().PlayOneShot(audioClip);
    }




}
