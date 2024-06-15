using Effects;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(PlayerModel))]
public class PlayerController : CharacterController<PlayerState>
{
    [SerializeField] public AudioClip AttackSound;
    [SerializeField] public AudioClip HurtSound;

    [SerializeField] public PlayerConfig config; // rest configs are in CharacterController attributes

    private void Start()
    {
        ChangeState<PlayerIdle>();
    }

    protected override void OnHurt(Transform souceTransform, Vector3 repelVelocity, float repelTransitionTime) { }
    protected override void OnHurtOver() { }
    protected override void OnDead() { }
}