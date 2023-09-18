using Pasta;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class EnemyAi : MonoBehaviour, IHittable
{
    [SerializeField] private List<Detector> detectors;
    [SerializeField] private AIData aiData;
    [SerializeField] private float detectionDelay = 0.05f, aiUpdateDelay = 0.06f, attackDelay = 2f;
    [SerializeField] private float attackDistance = 0.5f;
    [SerializeField] private List<SteeringBehaviour> steeringBehaviours;

    public UnityEvent OnAttackPressed;
    public UnityEvent<Vector2> OnMovementInput, OnPointerInput;
    [SerializeField] private Vector2 movementInput;
    [SerializeField] private AISolver movementDirectionSolver;
    bool Chasing = false;

    private WeaponParent weaponParent;

    [SerializeField] private Transform Player;
    private GameObject player;
    private Image attackIndicator;
    public float timeToAttack = 0; // When this reaches defaultTimeToAttack enemy will attack
    public float defaultTimeToAttack = 2; //Increase this if you want to make ai take longer
    public float stunTimer = 1; // Will be used or replaced when adding stagger

    //[SerializeField] private float chaseDistanceThershold = 3, attackDistanceThershold = 0.8f;
    //private float passedTime = 1;

    private void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");
        Player = player.transform;
        attackIndicator = GetComponentInChildren<Image>();
        weaponParent = GetComponentInChildren<WeaponParent>();

        //Detect objects


    }
    private void Awake()
    {
        InvokeRepeating("PerformDetection", 0, detectionDelay);
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
                attackIndicator.enabled = true;
                timeToAttack += Time.deltaTime;
                attackIndicator.fillAmount = timeToAttack / defaultTimeToAttack;
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
        if (gameObject.tag.Contains("Ranged"))
        {
            weaponParent.RangedAttack();
        }
        else
        {
            weaponParent.Attack();
        }
    }

    public void Death()
    {

    }

    //private IEnumerator UnStun()
    //{
    //    stunned = false;
    //    yield return new WaitForSeconds(stunDelay);
    //}

    private IEnumerator ChaseAndAttack()
    {
        if (aiData.currentTarget == null)
        {
            movementInput = Vector2.zero;
            timeToAttack = 0;
            attackIndicator.fillAmount = 0;
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
                Debug.Log("Attacking");
                //Attacking 

                movementInput = Vector2.zero;
                OnAttackPressed?.Invoke();
                if (timeToAttack >= defaultTimeToAttack) // Attack indicator stuff // Added timetoattack reset to chasing and idle states so that if player runs away it resets
                {
                    Attack(); // Attack method
                    timeToAttack = 0;
                    attackIndicator.fillAmount = 0;
                }
                yield return new WaitForSeconds(attackDelay);
                StartCoroutine(ChaseAndAttack());
            }
            else
            {
                Debug.Log("Chasing");
                //Chasing

                timeToAttack = 0;
                attackIndicator.fillAmount = 0;
                movementInput = movementDirectionSolver.GetDirectionToMove(steeringBehaviours, aiData);
                Debug.Log(movementInput);
                yield return new WaitForSeconds(aiUpdateDelay);
                StartCoroutine(ChaseAndAttack());
            }
        }
    }

    public void Hit(float damage)
    {
        //if(health <= 0)
        //{
        //    Destroy(gameObject);
        //}
        // TODO: Implement Hit Anim here and take damage.
    }
}
