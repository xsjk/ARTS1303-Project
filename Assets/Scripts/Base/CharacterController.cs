using System;
using System.Collections;
using System.Collections.Generic;
using Effects;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public abstract class CharacterController<T> : FSMController<T>
{
    public CharacterController characterController { get; protected set; } // only used for characterMoveModels
    public CharacterModel<T> model { get; protected set; }
    
    [SerializeField]
    public Attributes attributes;

    public bool isDead = false;

    public SkillModel[] SkillModels;


    protected virtual void Awake()
    {
        attributes.Health = attributes.MaxHealth;
        model = transform.GetComponent<CharacterModel<T>>();
        if (model == null)
            throw new Exception("CharacterModel is not found in " + name);
        // model.Init(this);
        characterController = GetComponent<CharacterController>();
        gameObject.AddComponent<DamageHandler>().action = Hurt;
    }

    public void Hurt(float hardTime, Transform souceTransform, Vector3 repelVelocity, float repelTransitionTime, IEffectResult effectResult)
    {
        Debug.Log(name + " hurt " + effectResult);
        if (isDead) return;
        attributes = attributes.ApplyEffect(effectResult);
        if (attributes.Health <= 0)
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


    public void PlayAudio(AudioClip audioClip)
    {
        if (audioClip == null)
            return;
        model.PlayAudio(audioClip);
    }

    protected override void Update() {
        base.Update();
        UpdateMoveAnimation();
    }
    
    protected virtual void UpdateMoveAnimation() {
        Vector3 localVelocity = characterController.velocity;
        model.SetMoveVelocity(new Vector2(localVelocity.x, localVelocity.z));
    }

}
