using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Pasta;

public class BossAI : MonoBehaviour, IHittable
{
    [SerializeField] private List<Detector> detectors;
    [SerializeField] private AIData aiData;
    [SerializeField] private float detectionDelay = 0.05f, aiUpdateDelay = 0.06f, attackDelay = 2f;
    [SerializeField] public float attackDistance = 0.5f;
    [SerializeField] private List<SteeringBehaviour> steeringBehaviours;

    public MonoBehaviour Mono => this;

    public UnityEvent OnAttackPressed;
    public UnityEvent<Vector2> OnMovementInput, OnPointerInput;
    [SerializeField] public Vector2 movementInput;
    [SerializeField] private AISolver movementDirectionSolver;
    bool Chasing = false;
    public float damage = 10f;
    public bool isAttacking = false;
    [SerializeField] private AbilityHolder abilityHolder;
    private WeaponParent weaponParent;
    public Health Health { get; protected set; }
    public static event System.Action<BossAI> OnDeath;
    private Level level;
    [SerializeField] private Transform Player;
    private GameObject player;
    private Image attackIndicator;
    public float timeToAttack = 0; // When this reaches defaultTimeToAttack enemy will attack
    public float defaultTimeToAttack = 2; //Increase this if you want to make ai take longer
    public float stunTimer = 1; // Will be used or replaced when adding stagger
    private AgentAnimations animations;
    [SerializeField]private AttackEffects attackEffect;
    public bool hasAttackEffect;
    private AbilityHolder[] abilityHolders;
    private Drop drop;


    //[SerializeField] private float chaseDistanceThershold = 3, attackDistanceThershold = 0.8f;
    //private float passedTime = 1;

    private void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");
        Player = player.transform;
        level = FindFirstObjectByType<Level>();
        weaponParent = GetComponentInChildren<WeaponParent>();
        //attackIndicator = weaponParent.GetComponentInChildren<Image>();
        abilityHolder = GetComponent<AbilityHolder>();
        abilityHolders = GetComponents<AbilityHolder>(); // Boss usually has more than one ability so added more abilityholders to gameobject.
        animations = GetComponent<AgentAnimations>();
        //Detect objects
        attackEffect = GetComponentInChildren<AttackEffects>();
        hasAttackEffect = attackEffect != null;
        drop = GetComponent<Drop>();
        

    }
    private void Awake()
    {
        InvokeRepeating("PerformDetection", 0, detectionDelay);
        Health = GetComponent<Health>();
        Debug.Assert(Health != null);
        Health.Reset();
    }
    protected virtual void OnEnable()
    {
        Health.OnDeath += DeathAction;
    }
    private void PerformDetection()
    {
        foreach (Detector detector in detectors)
        {
            detector.Detect(aiData);
        }

    }
    private void Update()
    {

        if (aiData.currentTarget != null)
        {

            //Looking at target.
            OnPointerInput?.Invoke(aiData.currentTarget.position);

            if (Chasing == false)
            {
                Chasing = true;
                StartCoroutine(ChaseAndAttack());
            }
            float distance = Vector2.Distance(aiData.currentTarget.position, transform.position);
            if (distance < attackDistance)
            {
                //attackIndicator.enabled = true;
                if (isAttacking == true)
                {
                    timeToAttack += Time.deltaTime;
                }
                
                if (timeToAttack >= defaultTimeToAttack / 1.5)
                {
                    weaponParent.Aim = false;
                    animations.aim = false;
                    Debug.Log("Stopping Aiming");
                }
                //attackIndicator.fillAmount = timeToAttack / defaultTimeToAttack;
            }
        }
        else if (aiData.GetTargetsCount() > 0)
        {
            //Getting the target
            aiData.currentTarget = aiData.targets[0];
        }

        //Debug.Log(movementInput);
        OnMovementInput?.Invoke(movementInput);
    }

    public void Attack()
    {
        Debug.Log("Swing");
        //abilityHolder.UseAbility = true; // <- Here for testing purposes.
        for(int i = 0; i < abilityHolders.Length; i++)
        {
            if (abilityHolders[i] != null)
            {
                abilityHolders[i].UseAbility = true;
            }
        }
        weaponParent.Attack();
        if (hasAttackEffect) attackEffect.CancelAttack();
        if (hasAttackEffect) attackEffect.HeavyAttack();
    }
    public void UseAbility()
    {
        for (int i = 0; i < abilityHolders.Length; i++)
        {
            if (abilityHolders[i] != null)
            {
                if(abilityHolders[i].ability.usableOutsideAttackRange == true)
                {
                    abilityHolders[i].UseAbility = true;
                }
            }
        }
        //if (abilityHolder.ability.usableOutsideAttackRange == true)
        //{
        //    abilityHolder.UseAbility = true;
        //}
    }

    protected virtual void DeathAction()
    {
        level.EnemyKilled();
        drop.RollDrop();
        Destroy(gameObject);
    }

    private IEnumerator ChaseAndAttack()
    {
        if (aiData.currentTarget == null)
        {
            //abilityHolder.CanUseAbility = false; // <- Here for testing purposes.
            for (int i = 0; i < abilityHolders.Length; i++)
            {
                if (abilityHolders[i] != null)
                {
                    abilityHolders[i].CanUseAbility = false;
                }
            }
            isAttacking = false;
            movementInput = Vector2.zero;
            timeToAttack = 0;
            //attackIndicator.fillAmount = 0;
            Chasing = false;
            yield break;
        }
        else
        {
            float distance = Vector2.Distance(aiData.currentTarget.position, transform.position);
            if (distance < attackDistance)
            {
                movementInput = Vector2.zero;
                Debug.Log("Attacking");
                isAttacking = true;
                //Attacking 
                //abilityHolder.CanUseAbility = true; // <- Here for testing purposes.
                for (int i = 0; i < abilityHolders.Length; i++)
                {
                    if (abilityHolders[i] != null)
                    {
                        abilityHolders[i].CanUseAbility = true;
                    }
                }
                //if (abilityHolder.UseAbility == false)
                //{
                //    movementInput = Vector2.zero;
                    for (int i = 0; i < abilityHolders.Length; i++)
                    {
                        if (abilityHolders[i] != null)
                        {
                            if (abilityHolders[i].UseAbility == false)
                            {
                                movementInput = Vector2.zero;
                            }
                        }
                    }
                //}
                OnAttackPressed?.Invoke();

                if (hasAttackEffect) attackEffect.AttackIndicator();
                if (hasAttackEffect) attackEffect.SetIndicatorLifetime(attackDelay);
                if (timeToAttack >= defaultTimeToAttack) // Attack indicator stuff // Added timetoattack reset to chasing and idle states so that if player runs away it resets
                {
                    Attack(); // Attack method
                    timeToAttack = 0;
                    //attackIndicator.fillAmount = 0;
                    isAttacking = false;
                }
                yield return new WaitForSeconds(attackDelay);
                StartCoroutine(ChaseAndAttack());
            }
            else
            {
                isAttacking = false;
                weaponParent.Aim = true;
                animations.aim = true;
                Debug.Log("Chasing");
                //Chasing
                //abilityHolder.CanUseAbility = true; // <- Here for testing purposes.
                for (int i = 0; i < abilityHolders.Length; i++)
                {
                    if (abilityHolders[i] != null)
                    {
                        abilityHolders[i].CanUseAbility = true;
                    }
                }
                UseAbility();
                timeToAttack = 0;
                //attackIndicator.fillAmount = 0;
                movementInput = movementDirectionSolver.GetDirectionToMove(steeringBehaviours, aiData);
                yield return new WaitForSeconds(aiUpdateDelay);
                StartCoroutine(ChaseAndAttack());
            }
        }
    }

    public void Hit(float damage)
    {
        if (OnDeath != null)
        {
            OnDeath(this);
        }
        Health.TakeDamage(damage);
    }
}
