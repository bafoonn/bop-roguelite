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
    [SerializeField] private List<Detector> detectors;
    [SerializeField] private AIData aiData;
    [SerializeField] private float detectionDelay = 0.05f, aiUpdateDelay = 0.06f, attackDelay = 2f;
    [SerializeField] private float attackDistance = 0.5f, attackStopDistance = 1.5f;
    [SerializeField] private List<SteeringBehaviour> steeringBehaviours;
    private TargetDetector targetDetector;
    #endregion

    private float attackDefaultDist;
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
    private float cooldown = 10f;
    private bool canuseAbility = true;
    private Separation seperation;
    public UnityEvent OnAttackPressed;
    private float dontattackdist = 5f;
    private int RandomInt;
    public UnityEvent<Vector2> OnMovementInput, PointerEnemy;
    [SerializeField] public Vector2 movementInput;
    [SerializeField] private AISolver movementDirectionSolver;
    bool Chasing = false;
    private EnemyCarrier enemySpawningCarrier; // Only used by carrier enemies
    private WeaponParent weaponParent;
    private AbilityHolder abilityHolder;
    [SerializeField] private Transform Player;
    private GameObject player;
    private float defaultDetectionDelay;
    private Image attackIndicator;
    [Header("Attack timers")]
    public float timeToAttack = 0; // When this reaches defaultTimeToAttack enemy will attack
    public float defaultTimeToAttack = 2; //Increase this if you want to make ai take longer
    private float defaultTimeToAttackWorkAround = 0; // TODO: DELETE THIS AT SOME POINT ONLY A WORKAROUND
    private float workaroundTimeToAttack = 0; // TODO: DELETE THIS AT SOME POINT ONLY A WORKAROUND
    public bool canAttack = false;
    private bool canAttackAnim = true;
    private bool firstAttack = true;
    public float stunTimer = 1; // Will be used or replaced when adding stagger

    public bool gotAttackToken = false;
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


    private bool shouldMaintainDistance = false;

    [Header("animator bools")]
    public bool IsIdle = true;
    public bool isAttacking = false;
    public bool stunned = false;
    public bool Death = false;

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
        defaultTimeToAttackWorkAround = defaultTimeToAttack;
        level = FindFirstObjectByType<Level>();
        weaponParent = GetComponentInChildren<WeaponParent>();
        //attackIndicator = weaponParent.GetComponentInChildren<Image>();
        animations = GetComponent<AgentAnimations>();
        abilityHolder = GetComponent<AbilityHolder>();
        targetDetector = GetComponentInChildren<TargetDetector>();
        defaultDetectionDelay = detectionDelay;
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
        attackDistance = dontattackdist;
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
        #region supportenemy stuff
        if (gameObject.name.Contains("Support"))
        {
            if(supportEnemyTarger != null)
            {
                LineRenderer lineRenderer;

                

                lineRenderer = GetComponent<LineRenderer>(); // Here to visualise for testing
                Vector3[] positions = new Vector3[2];
                positions[0] = gameObject.transform.position;
                positions[1] = supportEnemyTarger.position;
                lineRenderer.SetPositions(positions);
                //MeshCollider meshCollider = lineRenderer.AddComponent<MeshCollider>();
                //Mesh mesh = new Mesh();
                //lineRenderer.BakeMesh(mesh, true);
                //meshCollider.sharedMesh = mesh;
                //meshCollider.isTrigger = true;
                
            }
           
            if ((player.transform.position - transform.position).magnitude < 15.0f && canTarget) // If player is close support enemy gets random enemy closeby to buff
            {
                
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius, layermask);
                int random = Random.Range(1, hitColliders.Length);
                for (int i = 0; i < hitColliders.Length; i++)
                {
                    if (i == random) // Check if the current hitcollider is inside the result
                    {
                        if (hitColliders[i].gameObject.name.Contains("Support"))
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


        //if(canAttack == false)
        //{
        //    aiData.currentTarget = null; // REMEMBER TO DO SOMETHING WITH THIS
        //}
        if (aiData.currentTarget != null)
        {
            cooldown -= Time.deltaTime;
            if(cooldown <= 0)
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
                    if ((player.transform.position - transform.position).magnitude < 1.0f)
                    {
                        movementInput = Vector2.zero;

                    }
                    distance = Vector2.Distance(aiData.currentTarget.position, transform.position);
                    if (!canAttack)
                    {
                        SeekBehaviour seekbehaviour = gameObject.GetComponentInChildren<SeekBehaviour>();
                        seekbehaviour.targetReachedThershold = 5f;
                        attackDistance = dontattackdist;
                        detectionDelay = defaultDetectionDelay;
						float safeDistance = 5f;
						if (distance < safeDistance && shouldMaintainDistance)
						{
                            movementInput = Vector2.zero;
						}
					}
                    else
                    {
                        SeekBehaviour seekbehaviour = gameObject.GetComponentInChildren<SeekBehaviour>();
                        seekbehaviour.targetReachedThershold = 0.5f;
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
            if (hasAttackEffect) attackEffect.CancelAttack();
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
            
                
            if (hasAttackEffect) attackEffect.CancelAttack();
            if (hasAttackEffect) attackEffect.HeavyAttack();
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
            if(supportEnemyTarger != null)
            {
                supportEnemyTarger.GetComponent<Health>().immune = false;
            }
            
        }

        Death = true;
        EventActions.InvokeEvent(EventActionType.OnKill);

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

    //private IEnumerator UnStun()
    //{
    //    stunned = false;
    //    yield return new WaitForSeconds(stunDelay);
    //}


    public void UseAbility()
    {
        if(abilityHolder.ability != null)
        {
            if (abilityHolder.ability.usableOutsideAttackRange == true)
            {
				if (abilityHolder.ability.randomize) // If ability has randomize bool true execute.
				{
                    RandomInt = Random.Range(1, 8);
                    if (RandomInt == 1 && canuseAbility)
                    {
                        canuseAbility = false;
                        Debug.Log(RandomInt + "using ability randomly");
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
    public void ActivateIndicator() // Called from PlayerCloseSensor script when getting attack token.
	{
        Debug.Log("Activating indicator");
        if (hasAttackEffect) attackEffect.SetIndicatorLifetime(1f);
        if (hasAttackEffect) attackEffect.AttackIndicator();
    }

    public void DeActivateIndicator()
	{
        if (hasAttackEffect) attackEffect.SetIndicatorLifetime(0);
        if (hasAttackEffect) attackEffect.CancelAttack();
    }

    private IEnumerator ChaseAndAttack() // Enemy ai states "idle" "chase & look" "attack"
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
			if (!canAttack)
			{
                attackDistance = dontattackdist;
               

            }
			else
			{
                gotAttackToken = true;
                Debug.Log(gotAttackToken + gameObject.name);
                attackDistance = attackDefaultDist;
			}
            // Attack state
            if (distance < attackDistance && canAttack && canAttackAnim)  // if distance is smaller than attackdistance execute attack.
            {
                isAttacking = true; // FOR ANIMATOR
                                    //Attacking 
                if(abilityHolder.ability != null) abilityHolder.CanUseAbility = true;
                movementInput = Vector2.zero;
                OnAttackPressed?.Invoke();
                //if (hasAttackEffect) attackEffect.SetIndicatorLifetime(defaultTimeToAttack);
                //if (hasAttackEffect) attackEffect.AttackIndicator();
                if (timeToAttack >= defaultTimeToAttack) // Attack indicator stuff // Added timetoattack reset to chasing and idle states so that if player runs away it resets
                {
                    Debug.Log("Attacking");
                    Attack(); // Attack method
                    timeToAttack = 0;
                    //attackIndicator.fillAmount = 0;
                    isAttacking = false;
                    detectionDelay = defaultDetectionDelay;
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
                //if (hasAttackEffect) attackEffect.CancelAttack();
                //attackIndicator.fillAmount = 0;
                movementInput = movementDirectionSolver.GetDirectionToMove(steeringBehaviours, aiData);
                //Debug.Log(movementInput);
                yield return new WaitForSeconds(aiUpdateDelay);
                StartCoroutine(ChaseAndAttack());

            }
        }
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
