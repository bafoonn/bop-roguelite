using Pasta;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnemyAi : MonoBehaviour, IHittable
{
    #region detector stuff
    [SerializeField] private List<Detector> detectors;
    [SerializeField] private AIData aiData;
    [SerializeField] private float detectionDelay = 0.05f, aiUpdateDelay = 0.06f, attackDelay = 2f;
    [SerializeField] private float attackDistance = 0.5f;
    [SerializeField] private List<SteeringBehaviour> steeringBehaviours;
    #endregion

    public MonoBehaviour Mono => this;
    public Health Health { get; protected set; }
    public static event System.Action<EnemyAi> OnDeath;
    private Level level;
    public float damage = 5;
    public UnityEvent OnAttackPressed;
    public UnityEvent<Vector2> OnMovementInput, PointerEnemy;
    [SerializeField] public Vector2 movementInput;
    [SerializeField] private AISolver movementDirectionSolver;
    bool Chasing = false;
    private EnemyCarrier enemySpawningCarrier; // Only used by carrier enemies
    private WeaponParent weaponParent;
    private AbilityHolder abilityHolder;
    [SerializeField] private Transform Player;
    private GameObject player;
    private Image attackIndicator;
    public float timeToAttack = 0; // When this reaches defaultTimeToAttack enemy will attack
    public float defaultTimeToAttack = 2; //Increase this if you want to make ai take longer
    public float stunTimer = 1; // Will be used or replaced when adding stagger
    private AgentAnimations animations;
    //[SerializeField] private float chaseDistanceThershold = 3, attackDistanceThershold = 0.8f;
    //private float passedTime = 1;

    private AttackEffects attackEffect;
    private bool hasAttackEffect;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Player = player.transform;
        level = FindFirstObjectByType<Level>();
        weaponParent = GetComponentInChildren<WeaponParent>();
        //attackIndicator = weaponParent.GetComponentInChildren<Image>();
        animations = GetComponent<AgentAnimations>();
        abilityHolder = GetComponent<AbilityHolder>();
        //Detect objects
        if (this.gameObject.name.Contains("Carrier"))
        {
            enemySpawningCarrier = GetComponent<EnemyCarrier>();
        }
        attackEffect = GetComponentInChildren<AttackEffects>();
        hasAttackEffect = attackEffect != null;
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
            PointerEnemy?.Invoke(aiData.currentTarget.position);

            if (Chasing == false)
            {
                Chasing = true;
                StartCoroutine(ChaseAndAttack());
            }
            float distance = Vector2.Distance(aiData.currentTarget.position, transform.position);
            if (distance < attackDistance)
            {
                //attackIndicator.enabled = true;
                timeToAttack += Time.deltaTime;
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
        if (gameObject.tag.Contains("Ranged"))
        {
            weaponParent.RangedAttack();
        }
        else
        {
            weaponParent.Attack();
            abilityHolder.UseAbility = true;
            if (hasAttackEffect) attackEffect.CancelAttack();
            if (hasAttackEffect) attackEffect.HeavyAttack();
        }
    }

    protected virtual void DeathAction()
    {
        if (this.gameObject.name.Contains("Carrier"))
        {
            enemySpawningCarrier.SpawnMinions();
        }
        level.EnemyKilled();
        Destroy(gameObject);
    }

    //private IEnumerator UnStun()
    //{
    //    stunned = false;
    //    yield return new WaitForSeconds(stunDelay);
    //}


    public void UseAbility()
    {
        if (abilityHolder.ability.usableOutsideAttackRange == true)
        {
            movementInput = Vector2.zero;
            abilityHolder.UseAbility = true;

        }
    }

    private IEnumerator ChaseAndAttack()
    {
        if (aiData.currentTarget == null)
        {
            abilityHolder.CanUseAbility = false;
            movementInput = Vector2.zero;
            timeToAttack = 0;
            //if (hasAttackEffect) attackEffect.CancelAttack();
            //attackIndicator.fillAmount = 0;
            Chasing = false;
            yield break;
        }
        else
        {
            float distance = Vector2.Distance(aiData.currentTarget.position, transform.position);
            //if (stunned)
            //{
            //    movementInput = Vector2.zero;
            //    timeToAttack = 0;
            //    attackIndicator.fillAmount = 0;
            //    // StartCourotine(UnStun());
            //}
            if (distance < attackDistance)
            {
                //Attacking 
                abilityHolder.CanUseAbility = true;
                movementInput = Vector2.zero;
                OnAttackPressed?.Invoke();
                if (hasAttackEffect) attackEffect.AttackIndicator();
                if (timeToAttack >= defaultTimeToAttack) // Attack indicator stuff // Added timetoattack reset to chasing and idle states so that if player runs away it resets
                {
                    Attack(); // Attack method
                    timeToAttack = 0;
                    //attackIndicator.fillAmount = 0;
                }
                yield return new WaitForSeconds(attackDelay);
                StartCoroutine(ChaseAndAttack());
            }
            else
            {
                weaponParent.Aim = true;
                animations.aim = true;
                //Chasing
                abilityHolder.CanUseAbility = true; // <- Here for testing purposes.
                if(hasAttackEffect) attackEffect.CancelAttack();
                UseAbility();
                timeToAttack = 0;
                //if (hasAttackEffect) attackEffect.CancelAttack();
                //attackIndicator.fillAmount = 0;
                movementInput = movementDirectionSolver.GetDirectionToMove(steeringBehaviours, aiData);
                //Debug.Log(movementInput);
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
