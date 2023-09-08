using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnemyAi : MonoBehaviour
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

    [SerializeField] private Transform Player;
    private GameObject player;
    private Image attackIndicator;
    public float timeToAttack = 0;
    public float defaultTimeToAttack = 2;
    public float stunTimer = 1;

    //[SerializeField] private float chaseDistanceThershold = 3, attackDistanceThershold = 0.8f;
    //private float passedTime = 1;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Player = player.transform;
        attackIndicator = GetComponentInChildren<Image>();
        //Detect objects
        InvokeRepeating("PerformDetection", 0, detectionDelay);
        if (gameObject.tag.Contains("Ranged"))
        {
            //Ranged attack method
        }
        else
        {
            //Melee attack Method
        }
    }

    private void PerformDetection()
    {
        foreach(Detector detector in detectors)
        {
            detector.Detect(aiData);    
        }
        
    }
    private void Update()
    {
        if(aiData.currentTarget != null)
        {
            //Looking at target.
            OnPointerInput?.Invoke(aiData.currentTarget.position);
            if(Chasing == false)
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
        else if(aiData.GetTargetsCount() > 0)
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
    }

    private IEnumerator ChaseAndAttack()
    {
        if(aiData.currentTarget == null)
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
            if(distance < attackDistance)
            {
                Debug.Log("Attacking");
                //Attacking //TODO: ADD DELAY TO LET PLAYER KNOW THAT ENEMY IS ATTACKING ETC
                
                movementInput = Vector2.zero;
                OnAttackPressed?.Invoke();
                if (timeToAttack >= defaultTimeToAttack)
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
}
