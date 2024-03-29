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

public class EnemyAi : MonoBehaviour, IEnemy
{
    #region detector stuff
    [Header("detector stuff")]
    [SerializeField] public List<Detector> detectors;
    [SerializeField] public AIData aiData;
    [SerializeField] public float detectionDelay = 0.05f, aiUpdateDelay = 0.06f, attackDelay = 2f;
    [SerializeField] public float attackDistance = 0.5f, attackStopDistance = 1.5f;
    [SerializeField] public List<SteeringBehaviour> steeringBehaviours;
    private TargetDetector targetDetector;
    #endregion

    public float attackDefaultDist;
    public MonoBehaviour Mono => this;
    public Health Health { get; protected set; }
    public Movement Movement => agentMover;
    public StatusHandler Status { get; private set; }

    public Rigidbody2D Rigidbody { get; private set; }
    public bool IsBoss => false;


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

    private float cooldown = 10f;
    private bool canuseAbility = true;

    public UnityEvent OnAttackPressed;
    public float dontattackdist = 5f;

    private int RandomInt;

    public UnityEvent<Vector2> OnMovementInput, PointerEnemy;
    [SerializeField] public Vector2 movementInput;
    [SerializeField] public AISolver movementDirectionSolver;

    public bool Chasing = false;
    private EnemyCarrier enemySpawningCarrier; // Only used by carrier enemies
    public WeaponParent weaponParent;
    public AbilityHolder abilityHolder;
    private GameObject player;
    private float defaultDetectionDelay;
    private Image attackIndicator;

    [Header("Attack timers")]
    public float timeToAttack = 0; // When this reaches defaultTimeToAttack enemy will attack
    public float defaultTimeToAttack = 2; //Increase this if you want to make ai take longer
    public bool canAttack = false;
    private bool canAttackAnim = true;
    private bool firstAttack = true;
    public float stunTimer = 1; // Will be used or replaced when adding stagger

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
    public bool shouldMaintainDistance = false;

    [SerializeField] private Image attackplaceholderindicator;

    [Header("animator bools")]
    public bool IsIdle = true;
    public bool isAttacking = false;
    public bool stunned = false;
    public bool Death = false;

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

    public static event System.Action<EnemyAi> OnSpawn;
    public static event System.Action<EnemyAi> OnDie;

    protected virtual void Awake()
    {
        InvokeRepeating("PerformDetection", 0, detectionDelay);
        Health = GetComponent<Health>();
        Debug.Assert(Health != null);
        Health.Reset();
        if (OnSpawn != null) OnSpawn(this);
    }

    protected virtual void Start()
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

    protected virtual void OnEnable()
    {
        Health.OnDeath += DeathAction;
        Health.OnDamaged += OnDamaged;
    }

    protected virtual void OnDisable()
    {
        Health.OnDeath -= DeathAction;
        Health.OnDamaged -= OnDamaged;
    }

    protected virtual void OnDamaged()
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

            if ((player.transform.position - transform.position).magnitude < 20.0f && canTarget) // If player is close support enemy gets random enemy closeby to buff
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

                if (!gameObject.tag.Contains("Ranged")) // If not ranged enemy then execute staying away from player or attack if gotten attacktoken
                {

                    if ((player.transform.position - transform.position).magnitude > 7.0f)
                    {
                        canAttack = false;

                    }

                    if ((player.transform.position - transform.position).magnitude < 1.2f) // Stops enemies from pushing player
                    {
                        movementInput = Vector2.zero;

                    }

                    distance = Vector2.Distance(aiData.currentTarget.position, transform.position);

                    if (!canAttack)
                    {
                        // This puts the distance enemies will stay away from player if hasnt gotten attack token
                        SeekBehaviour seekbehaviour = gameObject.GetComponentInChildren<SeekBehaviour>();
                        seekbehaviour.targetReachedThershold = 2f; // Just here to stop seekbehaviour from reaching target too soon and stopping.
                        attackDistance = dontattackdist; // decrease attack dist
                        detectionDelay = defaultDetectionDelay; // How frequently "enemy" updates eg. looks for player.
                        float safeDistance = 5f;
                        if (distance < safeDistance && shouldMaintainDistance) // If inside safedistance stop moving & getting shouldMaintainDistance bool from PlayerCloseSensor which stops enemys from all attacking at the same time.
                        {
                            movementInput = Vector2.zero;
                        }
                    }
                    else
                    {
                        SeekBehaviour seekbehaviour = gameObject.GetComponentInChildren<SeekBehaviour>();
                        seekbehaviour.targetReachedThershold = 1f; // This is default 0.5f
                        shouldMaintainDistance = false;
                        attackDistance = attackDefaultDist;
                        detectionDelay = 0.1f;
                    }
                }
                else
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
                    if (isAttacking == true)
                    {
                        timeToAttack += Time.deltaTime;
                    }


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

        //Debug.Log(movementInput);
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
        else
        {
            weaponParent.Attack();
            if (abilityHolder.ability != null) if (abilityHolder.ability.randomize)
                {

                    RandomInt = Random.Range(1, 8);
                    if (RandomInt == 1 && canuseAbility)
                    {
                        canuseAbility = false;
                        if (abilityHolder.ability != null) abilityHolder.UseAbility = true;
                    }

                }
                else
                {
                    if (abilityHolder.ability != null) abilityHolder.UseAbility = true;
                }


            if (hasAttackEffect) attackEffect.CancelAttack(); // Stops indicator
            if (hasAttackEffect) attackEffect.HeavyAttack(); // Does attack sprite
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

        //level.EnemyKilled();
        if (OnDie != null) OnDie(this);

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
                else
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
    IEnumerator StuckCheck()
    {
        yield return new WaitForSeconds(detectionDelay + 0.2f);
        if ((player.transform.position - transform.position).magnitude < 2.0f)
        {
            movementInput = Vector2.zero;

        }
        else
        {

            aiData.currentTarget = null;
            aiData.targets.Clear();
            PerformDetection();
        }

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


        StartCoroutine(ChaseAndAttack());

        //if (!gameObject.tag.Contains("Ranged")) // TEST TO REVERT
        //{
        //    if (targetDetector.SeenPlayer && aiData != null && player != null)
        //    {
        //        aiData.targets.Add(player.transform);
        //        PerformDetection();
        //        movementInput = movementDirectionSolver.GetDirectionToMove(steeringBehaviours, aiData);
        //        StartCoroutine(StuckCheck());
        //        SeekBehaviour seekbehaviour = gameObject.GetComponentInChildren<SeekBehaviour>();
        //        seekbehaviour.targetReachedThershold = 1f;
        //    }


        //}
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

    protected virtual IEnumerator ChaseAndAttack() // Enemy ai states "idle" "chase & look" "attack"
    {

        // Idle state
        if (aiData.currentTarget == null) // If current target is null go to idle.
        {
            attackDistance = attackDefaultDist;

            if (hasAttackEffect) attackEffect.CancelAttack();

            isAttacking = false; // FOR ANIMATOR

            IsIdle = true;

            if (abilityHolder.ability != null) abilityHolder.CanUseAbility = false;

            timeToAttack = 0;

            Chasing = false;

            yield break;
        }
        else
        {

            IsIdle = false;

            float distance = Vector2.Distance(aiData.currentTarget.position, transform.position);

            // start attack at distance 
            if (!canAttack)
            {
                attackDistance = dontattackdist;

                if (attackplaceholderindicator != null)
                {
                    attackplaceholderindicator.enabled = false;
                }
            }
            else
            {

                if (attackplaceholderindicator != null)
                {
                    attackplaceholderindicator.enabled = true;
                }

                gotAttackToken = true;

                Debug.Log(gotAttackToken + gameObject.name);

                attackDistance = attackDefaultDist;
            }
            // Attack state
            if (distance < attackDistance && canAttack && canAttackAnim)  // if distance is smaller than attackdistance execute attack.
            {
                CleavingWeaponAnimations clclcl = GetComponentInChildren<CleavingWeaponAnimations>();
                clclcl.Swing(defaultTimeToAttack, 1f, 0.2f, attackEffect.IsFlipped, 90f);
                isAttacking = true; // FOR ANIMATOR
                                    //Attacking 
                if (abilityHolder.ability != null) abilityHolder.CanUseAbility = true;

                movementInput = Vector2.zero;

                OnAttackPressed?.Invoke();

                if (timeToAttack >= defaultTimeToAttack) // Attack indicator stuff // Added timetoattack reset to chasing and idle states so that if player runs away it resets
                {
                    Debug.Log("Attacking");

                    Attack(); // Attack method

                    timeToAttack = 0;

                    isAttacking = false;

                    detectionDelay = defaultDetectionDelay;
                }
                attackDistance = attackStopDistance;
                if (firstAttack) // TODO: FIX THIS IF TIME // Here only since indicator bugged out if enemy attacking first time don't know why
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
            else // Chasing state.
            {

                attackDistance = attackDefaultDist;

                weaponParent.Aim = true;

                animations.aim = true;

                isAttacking = false; // FOR ANIMATOR
                                     //Chasing

                if (abilityHolder.ability != null) abilityHolder.CanUseAbility = true; // <- Here for testing purposes.

                if (hasAttackEffect) attackEffect.SetIndicatorLifetime(0);

                if (hasAttackEffect) attackEffect.CancelAttack();

                UseAbility();

                timeToAttack = 0;

                movementInput = movementDirectionSolver.GetDirectionToMove(steeringBehaviours, aiData);

                yield return new WaitForSeconds(aiUpdateDelay);

                StartCoroutine(ChaseAndAttack());

            }
        }
    }


    public virtual void Hit(float damage, HitType type, ICharacter source = null)
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
