using Pasta;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using UnityEngine.U2D.IK;
using UnityEngine.UI;
using UnityEngine.VFX;

public class EnemyAi : MonoBehaviour, IEnemy
{
    #region detector stuff
    [SerializeField] private List<Detector> detectors;
    [SerializeField] private AIData aiData;
    [SerializeField] private float detectionDelay = 0.05f, aiUpdateDelay = 0.06f, attackDelay = 2f;
    [SerializeField] private float attackDistance = 0.5f, attackStopDistance = 1f;
    [SerializeField] private List<SteeringBehaviour> steeringBehaviours;
    private TargetDetector targetDetector;
    #endregion
    private float attackDefaultDist;
    public MonoBehaviour Mono => this;
    public Health Health { get; protected set; }
    public Movement Movement => agentMover;
    public StatusHandler Status { get; private set; }

    public Rigidbody2D Rigidbody { get; private set; }

    private Level level;
    public float damage = 5;
    private Separation seperation;
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
    private float defaultTimeToAttackWorkAround = 0; // TODO: DELETE THIS AT SOME POINT ONLY A WORKAROUND
    private float workaroundTimeToAttack = 0; // TODO: DELETE THIS AT SOME POINT ONLY A WORKAROUND
    public bool canAttack = false;
    private bool firstAttack = true;
    public float stunTimer = 1; // Will be used or replaced when adding stagger
    private AgentAnimations animations;
    [SerializeField] private Drop drop;
    //[SerializeField] private float chaseDistanceThershold = 3, attackDistanceThershold = 0.8f;
    //private float passedTime = 1;
    private AgentMover agentMover;
    private float defaultMaxSpeed;
    private AttackEffects attackEffect;
    private bool hasAttackEffect;
    [SerializeField] private bool hasDeathAnim = false;
    private EnemyDeath enemyDeathScript;
    [SerializeField] private GameObject Corpse;
    public bool IsIdle = true;
    public bool isAttacking = false;
    public bool Death = false;
    #region Damage taking effects
    private SpriteRenderer spriteRenderer; // TAKE DAMAGE STUFF
    private Color defaultColor; // TAKE DAMAGE STUFF
    private ParticleSystem m_particleSystem; // TAKE DAMAGE STUFF
    [SerializeField] private VisualEffect takeDamageEffects;
    private bool hasDamageEffects;
    [SerializeField] private GameObject ParticleSystemHolder;
    #endregion


    private void Start()
    {
        Status = this.AddOrGetComponent<StatusHandler>();
        Status.Setup(this);
        Rigidbody = GetComponent<Rigidbody2D>();
        attackDefaultDist = attackDistance;
        agentMover = GetComponent<AgentMover>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // TAKE DAMAGE STUFF
        defaultColor = spriteRenderer.color; // TAKE DAMAGE STUFF
        m_particleSystem = GetComponentInChildren<ParticleSystem>(); // TAKE DAMAGE STUFF
        enemyDeathScript = GetComponent<EnemyDeath>();
        player = GameObject.FindGameObjectWithTag("Player");
        Player = player.transform;
        defaultTimeToAttackWorkAround = defaultTimeToAttack;
        level = FindFirstObjectByType<Level>();
        weaponParent = GetComponentInChildren<WeaponParent>();
        //attackIndicator = weaponParent.GetComponentInChildren<Image>();
        animations = GetComponent<AgentAnimations>();
        abilityHolder = GetComponent<AbilityHolder>();
        targetDetector = GetComponentInChildren<TargetDetector>();

        //Detect objects
        if (this.gameObject.name.Contains("Carrier"))
        {
            enemySpawningCarrier = GetComponent<EnemyCarrier>();
        }
        attackEffect = GetComponentInChildren<AttackEffects>();
        hasAttackEffect = attackEffect != null;
        hasDamageEffects = takeDamageEffects != null;
        drop = GetComponent<Drop>();
        seperation = GetComponent<Separation>();
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
        Health.OnDamaged += OnDamaged;
    }

    private void OnDisable()
    {
        Health.OnDeath -= DeathAction;
        Health.OnDamaged -= OnDamaged;
    }

    private void OnDamaged()
    {
        spriteRenderer.color = Color.red;
        if (m_particleSystem != null)
        {
            //ParticleSystemHolder.transform.rotation = Quaternion.Euler(0, 0, player.transform.Find("AttackHandler").transform.localEulerAngles.z);
            //m_particleSystem.Play();
        }
        if (hasDamageEffects)
        {
            takeDamageEffects.SetFloat("Rotation", player.transform.Find("AttackHandler").transform.localEulerAngles.z);
            takeDamageEffects.SendEvent("Hit");
        }
        StartCoroutine(TakingDamage());
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
            if (aiData.currentTarget != null)
            {

                float distance = Vector2.Distance(aiData.currentTarget.position, transform.position);
                if (distance < attackDistance)
                {

                    if (weaponParent.Scoot && transform.gameObject.name.Contains("Ranged"))
                    {
                        if ((player.transform.position - transform.position).magnitude < 5.0f)
                        {

                            Vector3 direction = transform.position - player.transform.position;
                            //direction.y = 0;
                            direction = Vector3.Normalize(direction);
                            transform.rotation = Quaternion.Euler(direction);
                            movementInput = direction;

                        }
                        else
                        {
                            weaponParent.Scoot = false;
                        }
                    }

                    //attackIndicator.enabled = true;
                    if (isAttacking == true)
                    {
                        timeToAttack += Time.deltaTime;
                    }


                    if (timeToAttack >= defaultTimeToAttack / 1.5)
                    {
                        weaponParent.Aim = false;
                        animations.aim = false;
                    }



                    //attackIndicator.fillAmount = timeToAttack / defaultTimeToAttack;
                    if (targetDetector.colliders == null && gameObject.tag.Contains("Ranged"))
                    {
                        aiData.currentTarget = null;
                    }
                }
                if (distance > attackDistance + 0.5f) // TEMP SOLUTION 
                {
                    if (hasAttackEffect) attackEffect.CancelAttack();
                }

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
            if (hasAttackEffect) attackEffect.CancelAttack();
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

        Death = true;
        EventActions.InvokeEvent(EventActionType.OnKill);

        level.EnemyKilled();
        if (!hasDeathAnim)
        {
            Destroy(gameObject);
            drop.RollDrop();
        }
        else
        {
            drop.RollDrop();
            Vector3 position = transform.position;
            Destroy(gameObject);
            Instantiate(Corpse, transform.position, transform.rotation);
        }

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
            attackDistance = attackDefaultDist;
            if (hasAttackEffect) attackEffect.CancelAttack();
            isAttacking = false; // FOR ANIMATOR
            IsIdle = true;
            abilityHolder.CanUseAbility = false;
            timeToAttack = 0;
            movementInput = Vector2.zero;
            //if (hasAttackEffect) attackEffect.CancelAttack();
            //attackIndicator.fillAmount = 0;
            Chasing = false;
            yield break;
        }
        else
        {
            IsIdle = false;
            float distance = Vector2.Distance(aiData.currentTarget.position, transform.position);
            //if (stunned)
            //{
            //    movementInput = Vector2.zero;
            //    timeToAttack = 0;
            //    attackIndicator.fillAmount = 0;
            //    // StartCourotine(UnStun());
            //}
            // ALOITA HY�KK�YS X DISTANCELLA JA LOPETA Y 
            if (distance < attackDistance)
            {
                isAttacking = true; // FOR ANIMATOR
                                    //Attacking 
                abilityHolder.CanUseAbility = true;
                movementInput = Vector2.zero;
                OnAttackPressed?.Invoke();
                if (hasAttackEffect) attackEffect.SetIndicatorLifetime(defaultTimeToAttack);
                if (hasAttackEffect) attackEffect.AttackIndicator();
                if (timeToAttack >= defaultTimeToAttack) // Attack indicator stuff // Added timetoattack reset to chasing and idle states so that if player runs away it resets
                {
                    Debug.Log("Attacking");
                    Attack(); // Attack method
                    timeToAttack = 0;
                    //attackIndicator.fillAmount = 0;
                    isAttacking = false;
                }
                attackDistance = attackStopDistance;
                if (firstAttack) // TODO: FIX THIS IF TIME
                {
                    timeToAttack = 0;
                    yield return new WaitForSeconds(0);
                }
                else
                {
                    yield return new WaitForSeconds(defaultTimeToAttack);
                }
                firstAttack = false;
                StartCoroutine(ChaseAndAttack());



            }
            else
            {

                attackDistance = attackDefaultDist;
                weaponParent.Aim = true;
                animations.aim = true;
                isAttacking = false; // FOR ANIMATOR
                                     //Chasing
                abilityHolder.CanUseAbility = true; // <- Here for testing purposes.
                if (hasAttackEffect) attackEffect.SetIndicatorLifetime(0);
                if (hasAttackEffect) attackEffect.CancelAttack();
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

    public void Hit(float damage, ICharacter source = null)
    {
        Health.TakeDamage(damage);
    }


    private IEnumerator TakingDamage() // TAKE DAMAGE STUFF
    {
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = defaultColor;
    }
}
