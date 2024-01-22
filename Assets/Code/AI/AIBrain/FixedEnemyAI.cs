using Pasta;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.U2D.IK;
using UnityEngine.UI;
using UnityEngine.VFX;

// This requires: (PlayerCloseSensor script attached to player, WeaponParent script attached to enemys weapon parent, AgentMover, Detector Scripts, Steering scripts, AIData,Enemy, AgentAnimations, Health, EnemyDeath scripts attached to gameobject.)

public class FixedEnemyAI : MonoBehaviour, IEnemy
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

    private Level level;

    [Header("Damage that ai does")]
    public float damage = 5;

    // Ability-related variables
    private float cooldown = 10f;
    private bool canuseAbility = true;

    private bool isTakingStepsBack = false;

    public float dontattackdist = 5f; 

    private int RandomInt; // For randomizing

    [Header("Unity events")]
    public UnityEvent OnAttackPressed;
    public UnityEvent<Vector2> OnMovementInput, PointerEnemy;
    [SerializeField] public Vector2 movementInput;
    [SerializeField] public AISolver movementDirectionSolver;

    // Enemy state variables and diffrent enemy "logic"
    public WeaponParent weaponParent; // Has activate collider inside and deactivate as well.
    public AbilityHolder abilityHolder; // Abilities for enemy goes inside abilityholder.
    private GameObject player;
    public float defaultDetectionDelay; // If changing detection delay this is here to easily go back to default

    [Header("Attack timers")]
    public float timeToAttack = 0; // When this reaches defaultTimeToAttack enemy will attack
    public float defaultTimeToAttack = 2; // Attack speed
    public bool canAttack = false; // Used in getting attacktoken
    public bool canAttackAnim = true; // For animations and some if checks

    [Header("Animations & Speed")]
    public AgentAnimations animations; // Animations
    private Drop drop; // Drop script
    private AgentMover agentMover; // has acceleration and other movement variables inside.
    public AttackEffects attackEffect; // Attack effects
    public bool hasAttackEffect; // for error checking
    [SerializeField] private bool hasDeathAnim = false; // here to error check and just tick to true when adding corpse to prefab
    [SerializeField] private GameObject Corpse; // Has death anim inside

    private SeekBehaviour seekBehaviour;
    public bool shouldMaintainDistance = true;

    [SerializeField] private Image attackplaceholderindicator;

    [Header("animator bools")]
    public bool IsIdle = true;
    public bool isAttacking = false;
    public bool Death = false;

    // Attack effect-related variables
    private bool stopAttacking;
    private bool goAheadAttack = false;
    private Material _defaultMaterial = null;
    [SerializeField] private Material _damagedMaterial = null;
    [SerializeField] private SoundEffect deathSound;
    private bool attacked = false;

    public static event System.Action<FixedEnemyAI> OnSpawn;
    public static event System.Action<FixedEnemyAI> OnDie;


    #region Damage taking effects
    [Header("damage taking effects or other effects")]
    private SpriteRenderer spriteRenderer; // TAKE DAMAGE STUFF
    private Color defaultColor; // TAKE DAMAGE STUFF
    private ParticleSystem m_particleSystem; // TAKE DAMAGE STUFF
    [SerializeField] private VisualEffect takeDamageEffects;
    private bool hasDamageEffects;
    [SerializeField] private GameObject ParticleSystemHolder;
    #endregion


    private void Start()
    {
        seekBehaviour = GetComponentInChildren<SeekBehaviour>();
        Status = this.AddOrGetComponent<StatusHandler>();
        Status.Setup(this);
        Rigidbody = GetComponent<Rigidbody2D>();
        agentMover = GetComponent<AgentMover>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // TAKE DAMAGE STUFF
        _defaultMaterial = spriteRenderer.material;
        defaultColor = spriteRenderer.color; // TAKE DAMAGE STUFF
        m_particleSystem = GetComponentInChildren<ParticleSystem>(); // TAKE DAMAGE STUFF
        player = GameObject.FindGameObjectWithTag("Player");
        level = FindFirstObjectByType<Level>();
        weaponParent = GetComponentInChildren<WeaponParent>();
        animations = GetComponent<AgentAnimations>();
        abilityHolder = GetComponent<AbilityHolder>();
        targetDetector = GetComponentInChildren<TargetDetector>();        
        aiData = GetComponent<AIData>();
        attackEffect = GetComponentInChildren<AttackEffects>();     
        drop = GetComponent<Drop>();

        // Setting variable values.
        hasAttackEffect = attackEffect != null;
        hasDamageEffects = takeDamageEffects != null;
        attackDefaultDist = attackDistance;
        defaultDetectionDelay = detectionDelay;
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
        if (OnSpawn != null) OnSpawn(this);
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

        if (attacked) // If has just performed attack
        {
            if ((player.transform.position - transform.position).magnitude < 3.5f) // Back away from player if not attacking.
            {
                Vector3 direction = transform.position - player.transform.position;
                direction = Vector3.Normalize(direction);
                transform.rotation = Quaternion.Euler(direction);
                movementInput = direction;
            }
            else
            {
                attacked = false;
                movementInput = Vector2.zero;
            }

        }

        if(movementInput.x == 0 && movementInput.y == 0)
        {
            IsIdle = true;
        }

        if (aiData.currentTarget != null)
        {
            cooldown -= Time.deltaTime; // ability cooldown

            if (cooldown <= 0) 
            {
                canuseAbility = true;
            }

            float distance = Vector2.Distance(aiData.currentTarget.position, transform.position);
            //Looking at target.
            PointerEnemy?.Invoke(aiData.currentTarget.position);


            if (aiData.currentTarget != null)
            {

                if ((player.transform.position - transform.position).magnitude > 7.0f) // If far away and hasent returned to false then return false
                {
                    canAttack = false;

                }
                if ((player.transform.position - transform.position).magnitude < 1.2f) // Stops enemies from pushing player
                {
                    movementInput = Vector2.zero;

                }
                distance = Vector2.Distance(aiData.currentTarget.position, transform.position);



                if (distance < attackDistance)
                {

                    
                    if (timeToAttack >= defaultTimeToAttack / 1.5) // Stops enemy from aiming when close to attacking.
                    {
                        weaponParent.Aim = false;
                        animations.aim = false;
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
        if (isAttacking == true)
        {
            timeToAttack += Time.deltaTime;
        }
        if (canAttack)
        {
            if (attackplaceholderindicator != null)
            {
                attackplaceholderindicator.enabled = true;
            }
        }
        else
        {
            if (attackplaceholderindicator != null)
            {
                attackplaceholderindicator.enabled = false;
            }
        }
        #endregion

        OnMovementInput?.Invoke(movementInput);
    }

    public virtual void Attack()
    {
        if (hasAttackEffect) attackEffect.CancelAttack(); // Stops indicator
        if (hasAttackEffect) attackEffect.HeavyAttack(); // Does attack sprite
        weaponParent.Attack(); // this just activates and deactivates collider on weapon.
        attacked = true;
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
        

    }

   

    protected virtual void DeathAction()
    {
        AudioManager.Current.PlaySoundEffect(deathSound, 1f);

        Death = true;
        ItemAbilities.InvokeEvent(EventActionType.OnKill);

        if (OnDie != null) OnDie(this);


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

    public void UseAbilityAtRange()
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
    IEnumerator CanAttack() // adds delay so enemy dosent attack nonstop
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

    public void DeActivateIndicator() // Called from PlayerCloseSensor script when not getting attack token.
    {
        goAheadAttack = false;

        if (attackplaceholderindicator != null)
        {
            attackplaceholderindicator.enabled = false;
        }

        if (hasAttackEffect) attackEffect.SetIndicatorLifetime(0);
        if (hasAttackEffect) attackEffect.CancelAttack();
    }

    public void StartAttack() // Comes from attackstate and activates attack
    {
        if (!stopAttacking)
        {
            attacked = false;
            Debug.Log("going inside AttackCourotine");
            stopAttacking = true;
            StartCoroutine(AttackCourotine());
        }
    }

    public IEnumerator AttackCourotine()
    {
        CleavingWeaponAnimations clclcl = GetComponentInChildren<CleavingWeaponAnimations>();
        clclcl.Swing(defaultTimeToAttack, 1f, 0.5f, !attackEffect.IsFlipped, 80f);
        
        IsIdle = false;
        Debug.Log("Inside attack courotine");
        isAttacking = true; // FOR ANIMATOR
                            //Attacking 
        if (abilityHolder.ability != null) abilityHolder.CanUseAbility = true;
        movementInput = Vector2.zero;
        OnAttackPressed?.Invoke();

        attackDistance = attackStopDistance;

        yield return new WaitForSeconds(defaultTimeToAttack);
        isAttacking = false;
        canAttack = false;
        shouldMaintainDistance = true;
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


