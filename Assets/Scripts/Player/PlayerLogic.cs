using System;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    private AudioSource _audioSource;
    public float attackRange = 1.0f; // how far we can attack
    [SerializeField] public AudioClip AttackSound;
    [SerializeField] public AudioClip HurtSound;


    private void Awake() {
        gameObject.AddComponent<DamageHandler>().action = Hurt;
    }


    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void DealDamageToEnemy()
    {
        _audioSource.PlayOneShot(AttackSound);
        var hits = Physics.OverlapSphere(transform.position, attackRange);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                var enemy = hit.GetComponent<EnemyLogic>();
                enemy.TakeDamage(1);
                return;
            }
        }
    }
    
    public void TakeDamage(float amount)
    {
        _audioSource.PlayOneShot(HurtSound);
        Debug.Log("Player took damage");
    }

    public void Die()
    {
        Debug.Log("Player died");
    }

    
    public void Hurt(float hardTime, Transform souceTransform, Vector3 repelVelocity, float repelTransitionTime, float damgeValue)
    {
        Debug.Log("Player hurt " + "hardTime: " + hardTime + " repelVelocity: " + repelVelocity + " repelTransitionTime: " + repelTransitionTime + " damgeValue: " + damgeValue);
    }
}