using System;
using Effects;
using UnityEngine;


public class PlayerController : CharacterController<PlayerState>
{
    [SerializeField] public AudioClip AttackSound;
    [SerializeField] public AudioClip HurtSound;

    protected override void Awake() {
        base.Awake();
        gameObject.AddComponent<DamageHandler>().action = Hurt;
    }

    protected override void OnHurt(Transform souceTransform, Vector3 repelVelocity, float repelTransitionTime) {}
    protected override void OnHurtOver() {}
    protected override void OnDead() {}
}