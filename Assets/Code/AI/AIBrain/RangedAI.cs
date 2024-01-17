using Pasta;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.U2D.IK;
using UnityEngine.UI;
using UnityEngine.VFX;

// This requires: (PlayerCloseSensor script attached to player, WeaponParent script attached to enemys weapon parent, AgentMover, Detector Scripts, Steering scripts, AIData, AgentAnimations, Health, EnemyDeath scripts attached to gameobject.)

public class RangedAI : MonoBehaviour, IEnemy
{
    #region detector stuff
    [Header("detector stuff")]
    [SerializeField] public List<Detector> detectors;
    [SerializeField] public AIData aiData;
    [SerializeField] public float detectionDelay = 0.05f, aiUpdateDelay = 0.06f, attackDelay = 2f;
    [SerializeField] public float attackDistance = 1f, attackStopDistance = 1.5f;
    [SerializeField] public List<SteeringBehaviour> steeringBehaviours;
    private TargetDetector targetDetector;
    #endregion


    // General enemy properties and components
    [Header("General enemy properties and components")]
    public float attackDefaultDist;
    public MonoBehaviour Mono => this;
    public Health Health { get; protected set; }
    public Movement Movement => agentMover;
    public StatusHandler Status { get; private set; }

    public Rigidbody2D Rigidbody { get; private set; }


    #region supportenemy stuff
    [Header("Support enemy variables")]
    public Transform supportEnemyTarger;
    [SerializeField] private LayerMask layermask;
    public float radius = 20;
    private bool canTarget = true;
    #endregion

    private Level level;

    [Header("Damage that ai does")]
    public float damage = 5;

    // Ability-related variables
    private float cooldown = 10f;
    private bool canuseAbility = true;

    
    public float dontattackdist = 5f;

    private int RandomInt;

    [Header("Unity events")]
    public UnityEvent OnAttackPressed;
    public UnityEvent<Vector2> OnMovementInput, PointerEnemy;
    [SerializeField] public Vector2 movementInput;
    [SerializeField] public AISolver movementDirectionSolver;

    // Enemy state variables and diffrent enemy "logic"
    public bool Chasing = false;
    private EnemyCarrier enemySpawningCarrier; // Only used by carrier enemies
    public WeaponParent weaponParent;
    public AbilityHolder abilityHolder;
    [SerializeField] private Transform Player;
    private GameObject player;
    public float defaultDetectionDelay;
    private Image attackIndicator;

    [Header("Attack timers")]
    public float timeToAttack = 0; // When this reaches defaultTimeToAttack enemy will attack
    public float defaultTimeToAttack = 2; //Increase this if you want to make ai take longer
    public bool canAttack = false;
    public bool canAttackAnim = true;
    private bool firstAttack = true;
    private float stunTimer = 1; // Will be used or replaced when adding stagger

    [Header("Animations & Speed")]
    public bool gotAttackToken = false;
    public AgentAnimations animations;
    [SerializeField] private Drop drop;
    private AgentMover agentMover;
    private float defaultMaxSpeed;
    public AttackEffects attackEffect;
    public bool hasAttackEffect;
    [SerializeField] private bool hasDeathAnim = false;
    private EnemyDeath enemyDeathScript;
    [SerializeField] private GameObject Corpse;

    private SeekBehaviour seekBehaviour;
    public bool shouldMaintainDistance = true;

    [SerializeField] public Image attackplaceholderindicator;

    [Header("animator bools")]
    public bool IsIdle = true;
    public bool isAttacking = false;
    public bool stunned = false;
    public bool Death = false;

    // Attack effect-related variables
    private bool stopAttacking;
    private bool goAheadAttack = false;
    private Material _defaultMaterial = null;
    [SerializeField] private Material _damagedMaterial = null;
    [SerializeField] private SoundEffect deathSound;

    #region Damage taking effects
    [Header("damage taking effects or other effects")]
    public SpriteRenderer spriteRenderer; // TAKE DAMAGE STUFF
    private Color defaultColor; // TAKE DAMAGE STUFF
    private ParticleSystem m_particleSystem; // TAKE DAMAGE STUFF
    [SerializeField] private VisualEffect takeDamageEffects;
    private bool hasDamageEffects;
    [SerializeField] private GameObject ParticleSystemHolder;
    #endregion


    private void Start()
    {
        if (gameObject.name.Contains("Support"))
        {
            gameObject.AddComponent<LineRenderer>();
            canAttack = false;
        }
        seekBehaviour = GetComponentInChildren<SeekBehaviour>();

        Status = this.AddOrGetComponent<StatusHandler>();

        Status.Setup(this);

        Rigidbody = GetComponent<Rigidbody2D>();

        attackDefaultDist = attackDistance;

        agentMover = GetComponent<AgentMover>();

        spriteRenderer = GetComponent<SpriteRenderer>(); // TAKE DAMAGE STUFF

        _defaultMaterial = spriteRenderer.material;

        defaultColor = spriteRenderer.color; // TAKE DAMAGE STUFF

        m_particleSystem = GetComponentInChildren<ParticleSystem>(); // TAKE DAMAGE STUFF

        enemyDeathScript = GetComponent<EnemyDeath>();

        player = GameObject.FindGameObjectWithTag("Player");

        Player = player.transform;

        level = FindFirstObjectByType<Level>();

        weaponParent = GetComponentInChildren<WeaponParent>();

        animations = GetComponent<AgentAnimations>();

        abilityHolder = GetComponent<AbilityHolder>();

        targetDetector = GetComponentInChildren<TargetDetector>();

        defaultDetectionDelay = detectionDelay;

        aiData = GetComponent<AIData>();

        //Detect objects
        if (this.gameObject.name.Contains("Carrier"))
        {
            enemySpawningCarrier = GetComponent<EnemyCarrier>();
        }

        attackEffect = GetComponentInChildren<AttackEffects>();

        hasAttackEffect = attackEffect != null;

        hasDamageEffects = takeDamageEffects != null;

        drop = GetComponent<Drop>();

        attackDistance = dontattackdist;

        if (attackplaceholderindicator != null)
        {
            attackplaceholderindicator.enabled = false;
        }
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
        if (m_particleSystem != null)
        {
            //ParticleSystemHolder.transform.rotation = Quaternion.Euler(0, 0, player.transform.Find("AttackHandler").transform.localEulerAngles.z);
            //m_particleSystem.Play();
        }
        if (hasDamageEffects)
        {
            //takeDamageEffects.SetFloat("Rotation", player.transform.Find("AttackHandler").transform.localEulerAngles.z);
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
        if ((player.transform.position - transform.position).magnitude > 7.0f || aiData.currentTarget == null) // Deactivates attack indicator if player is not seen or is far enough away
        {
            if (attackplaceholderindicator != null)
            {
                attackplaceholderindicator.enabled = false;
            }

        }

        #region supportenemy stuff
        if (gameObject.name.Contains("Support"))
        {
            if (supportEnemyTarger != null)
            {
                LineRenderer lineRenderer;



                lineRenderer = GetComponent<LineRenderer>(); // Here to visualise for testing
                Vector3[] positions = new Vector3[2];
                positions[0] = gameObject.transform.position;
                positions[1] = supportEnemyTarger.position;
                lineRenderer.SetPositions(positions);

            }

            if ((player.transform.position - transform.position).magnitude < 15.0f && canTarget) // If player is close support enemy gets random enemy closeby to buff
            {

                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius, layermask);
                int random = Random.Range(1, hitColliders.Length);
                for (int i = 0; i < hitColliders.Length; i++)
                {

                    if (i == random) // Check if the current hitcollider is inside the result
                    {

                        if (hitColliders[i].gameObject.name.Contains("Support")) // Stops from buffing itself or other support enemies
                        {
                            break;
                        }
                        Debug.Log(i);

                        if (hitColliders[i].gameObject.TryGetComponent(out Health health))
                        {
                            supportEnemyTarger = hitColliders[i].gameObject.transform;
                            hitColliders[i].gameObject.GetComponent<Health>().immune = true; // added check to health script called immune. // goes false on death method.

                            canTarget = false;
                        }
                    }

                }


            }
        }
        #endregion


        if (aiData.currentTarget != null)
        {
            cooldown -= Time.deltaTime;

            if (cooldown <= 0)
            {
                canuseAbility = true;
            }

            float distance = Vector2.Distance(aiData.currentTarget.position, transform.position);
            //Looking at target.
            PointerEnemy?.Invoke(aiData.currentTarget.position);

            if (Chasing == false)
            {
                Chasing = true;
            }

            if (aiData.currentTarget != null)
            {



                
                    canAttack = true;
                    if (!canAttack) // Ranged enemy
                    {
                        SeekBehaviour seekbehaviour = gameObject.GetComponentInChildren<SeekBehaviour>();
                        seekbehaviour.targetReachedThershold = 10f;
                        attackDistance = dontattackdist;
                        detectionDelay = defaultDetectionDelay;
                        float safeDistance = 10f;
                        if (distance < safeDistance && shouldMaintainDistance)
                        {
                            movementInput = Vector2.zero;
                        }
                    }
                
                if (distance < attackDistance)
                {
                    if (weaponParent.Scoot && transform.gameObject.name.Contains("Ranged")) // Ranged enemy "runs" away from player
                    {
                        if ((player.transform.position - transform.position).magnitude < 5.0f) // if player is in range of enemy do this.
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

                    if (timeToAttack >= defaultTimeToAttack / 1.5) // Stops enemy from aiming when close to attacking.
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

        #region Attacking // TODO: Test this more (added here since seemed to bug out after putting to attack state)
        if (isAttacking == true) // If is attacking start increasing timetoattack and when that reaches defaulttimetoattack then attack
        {
            timeToAttack += Time.deltaTime;
        }
        if (timeToAttack >= defaultTimeToAttack - 0.1f) // Attack indicator stuff // Added timetoattack reset to chasing and idle states so that if player runs away it resets
        {
            Debug.Log("Attacking");
            Attack(); // Attack method
            timeToAttack = 0;
            isAttacking = false;
            detectionDelay = defaultDetectionDelay;
        }
        if (timeToAttack >= defaultTimeToAttack / 1.5) // Stops enemy from aiming when close to attacking.
        {
            weaponParent.Aim = false;
            animations.aim = false;
        }
        if (canAttack)
        {
            if (attackplaceholderindicator != null)
            {
                attackplaceholderindicator.enabled = true;
            }
        }
        #endregion

        OnMovementInput?.Invoke(movementInput);
    }

    public void Attack()
    {
        if (gameObject.tag.Contains("Ranged"))
        {
            weaponParent.RangedAttack();
            if (hasAttackEffect) attackEffect.CancelAttack(); // Stops indicator
        }
        if (gameObject.name.Contains("Support"))
        {
            weaponParent.ImmunityBeam();
            canAttack = false;
        }
        
    }

    protected virtual void DeathAction()
    {
        AudioManager.Current.PlaySoundEffect(deathSound, 1f);
        if (this.gameObject.name.Contains("Carrier"))
        {
            enemySpawningCarrier.SpawnMinions();
        }

        if (gameObject.name.Contains("Support"))
        {
            if (supportEnemyTarger != null)
            {
                supportEnemyTarger.GetComponent<Health>().immune = false;
            }

        }

        Death = true;
        ItemAbilities.InvokeEvent(EventActionType.OnKill);

        level.EnemyKilled();

        //HitStopper.Stop(0.2f);

        if (Corpse == null)
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

    public void UseAbility()
    {
        if (abilityHolder.ability != null)
        {

            if (abilityHolder.ability.usableOutsideAttackRange == true)
            {

                if (abilityHolder.ability.randomize) // If ability has randomize bool true execute.
                {

                    RandomInt = Random.Range(1, 8);
                    if (RandomInt == 1 && canuseAbility)
                    {
                        canuseAbility = false;
                        movementInput = Vector2.zero;
                        abilityHolder.UseAbility = true;
                        canAttackAnim = false;
                        if (hasAttackEffect) attackEffect.CancelAttack();
                        StartCoroutine(CanAttack());
                    }
                    else
                    {
                        canuseAbility = false;
                        if (hasAttackEffect) attackEffect.CancelAttack();
                        StartCoroutine(CanAttack());
                    }
                }
                else // If not just do normally
                {
                    movementInput = Vector2.zero;
                    abilityHolder.UseAbility = true;
                    canAttackAnim = false;
                    if (hasAttackEffect) attackEffect.CancelAttack();
                    StartCoroutine(CanAttack());
                }

            }
        }

    }
    public void ToggleMaintainDistance(bool value) // Gets bool value from playerclosesensor script.
    {
        shouldMaintainDistance = value;
    }
    IEnumerator CanAttack()
    {
        yield return new WaitForSeconds(attackDelay);
        canAttackAnim = true;
    }

    public void ActivateIndicator() // Called from PlayerCloseSensor script when getting attack token. // DELETE THIS
    {
        if (!goAheadAttack)
        {
            goAheadAttack = true;
            Debug.Log("Got attack go ahead");
            if (hasAttackEffect) attackEffect.SetIndicatorLifetime(1f);
            if (hasAttackEffect) attackEffect.AttackIndicator();
        }
    }

    public void DeActivateIndicator()
    {
        goAheadAttack = false;

        if (attackplaceholderindicator != null)
        {
            attackplaceholderindicator.enabled = false;
        }

        if (hasAttackEffect) attackEffect.SetIndicatorLifetime(0);
        if (hasAttackEffect) attackEffect.CancelAttack();
    }

    public void StartAttack()
    {
        if (!stopAttacking)
        {
            Debug.Log("going inside AttackCourotine");
            stopAttacking = true;
            StartCoroutine(AttackCourotine());
        }
    }

    public IEnumerator AttackCourotine()
    {
        IsIdle = false;
        Debug.Log("Inside attack courotine");
        isAttacking = true; // FOR ANIMATOR
                            //Attacking 
        if (abilityHolder.ability != null) abilityHolder.CanUseAbility = true;
        movementInput = Vector2.zero;
        OnAttackPressed?.Invoke();

        if (timeToAttack >= defaultTimeToAttack - 0.1f) // Attack indicator stuff // Added timetoattack reset to chasing and idle states so that if player runs away it resets
        {
            Debug.Log("Attacking");
            Attack(); // Attack method
            timeToAttack = 0;
            isAttacking = false;
            detectionDelay = defaultDetectionDelay;
        }
        attackDistance = attackStopDistance;
        yield return new WaitForSeconds(defaultTimeToAttack);
        isAttacking = false;
        canAttack = false;
        stopAttacking = false;


    }

    public void Hit(float damage, HitType type, ICharacter source = null)
    {
        Health.TakeDamage(damage);
        if (type == HitType.Hit && source != null)
        {
            Vector2 direction = transform.position - source.Mono.transform.position;

            direction.Normalize();

            Rigidbody.position = Rigidbody.position + direction * 0.33f;
        }
    }


    private IEnumerator TakingDamage() // TAKE DAMAGE STUFF
    {
        if (_damagedMaterial != null) spriteRenderer.material = _damagedMaterial;
        else spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.2f);

        spriteRenderer.material = _defaultMaterial;
        spriteRenderer.color = Color.white;
    }
}