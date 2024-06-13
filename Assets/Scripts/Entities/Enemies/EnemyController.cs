using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;
using InitType = EnemyAnimationModel.InitType;


[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : CharacterController<EnemyState>
{
   
    protected IDungeonRoom _room;
    protected GameObject _player;
    protected Rigidbody _rigidbody;
    protected AudioSource _audioSource;
    protected float _attackCooldown;
    protected float _patrolCooldown;
    protected float _remainingHealth;


    [SerializeField]

    public EnemyConfig config;

    public Vector3 moveMotion = new Vector3(0, -9f, 0);
    public GameObject HpBarPrefab;
    private HPBarManager _hpBarManager;
    private GameObject _HpBarObj;
    private NavMeshAgent _navMeshAgent;



    public override float Hp { 
        get => hp; 
        set 
        {
            hp = value;
            if (hp <= 0)
                hp = 0;
            if (_hpBarManager != null)
                _hpBarManager.percent = hp / (float)maxHp;
        }
    }

    public void SetRoom(IDungeonRoom room) {
        _room = room;
    }

    protected override void Awake()
    {
        base.Awake();
        _navMeshAgent = GetComponent<NavMeshAgent>();

        Hp = maxHp;

        // If _hpBarManager is set, bind the HpBarFollowTarget to it.
        if (_hpBarManager != null) {
            var followTarget = transform.Find("HpBarFollowTarget");
            if (followTarget == null)
                throw new System.Exception("Can't find HpBarFollowTarget in " + gameObject.name);
            _hpBarManager.followTarget = followTarget;
        }
    }

    private void OnEnable() {
        config.animationModel.SetInitialState(this);
    }
    protected override void OnHurt(Transform souceTransform, Vector3 repelVelocity, float repelTransitionTime)
    {
        ChangeState<EnemyHurt>(ignoreSame: false);
        (state as EnemyHurt).SetData(souceTransform, repelVelocity, repelTransitionTime);
    }

    protected override void OnHurtOver()
    {
        ChangeState<EnemyIdle>(ignoreSame: false);
    }

    protected override void Update()
    {
        base.Update();
        UpdateMoveAnimation();
    }

    public void StopRepel()
    {
        moveMotion.Set(0, -9f, 0);
    }

    protected override void OnDead()
    {
        _HpBarObj.SetActive(false);
        ChangeState<EnemyDead>();
        StopNavigation();
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<CharacterController>().enabled = false;
        Invoke("Destroy", 3);
    }

    private void Destroy()
    {
        gameObject.SetActive(false);
    }

    #region Navigation
    public void StartNavigation()
    {
        _navMeshAgent.enabled = true;
    }
    public void StopNavigation()
    {
        _navMeshAgent.enabled = false;
    }
    public void SetNavigationTarget(Vector3 target)
    {
        _navMeshAgent.SetDestination(target);
    }
    public void UpdateMoveAnimation() {
        Vector3 velocity = _navMeshAgent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        model.SetFloat("SpeedX", localVelocity.x);
        model.SetFloat("SpeedY", localVelocity.z);
    }

    #endregion

    #region Attack
    public bool Attack()
    {
        model.DeactivateWeapons();
        foreach(var skillModel in SkillModels)
        {
            if (skillModel.Ready)
            {
                model.ActivateWeapons(skillModel.skill);
                model.TriggerSkill(skillModel.skill);
                skillModel.OnRelease();
                return true;
            }
        }
        return false;
    }



    #endregion

    #region HpBar
    private void MakeHpBar() {
        if (HpBarPrefab == null) 
        {
            throw new System.Exception("HpBarPrefab is null");
        }
        var canvas = GameObject.Find("Canvas");
        _HpBarObj = Instantiate(HpBarPrefab, canvas.transform);
        _HpBarObj.name = "HpBar_" + gameObject.name;
        _HpBarObj.SetActive(false);
        _hpBarManager = _HpBarObj.GetComponent<HPBarManager>();
        Hp = maxHp;
    }
    
    public void showHpBar()
    {
        _HpBarObj?.SetActive(true);
    }

    public void hideHpBar()
    {
        _HpBarObj?.SetActive(false);
    }

    #endregion

}
