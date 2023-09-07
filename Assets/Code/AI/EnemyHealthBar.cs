using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float Health;
    [SerializeField] private float damage;




    public bool takeDamage = false;
    public bool attacking = false;
    public bool stunned = false;
    public bool attacked = false;
    public Image attackIndicator;
    public float timeToAttack = 0;
    public float defaultTimeToAttack = 2;
    public float stunTimer = 1;


    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponentInChildren<Image>();
        SetHealth(Health);
        healthBar.enabled = false;
    }

    
    public void TakeDamage(float damage)
    {
        Debug.Log("Taking damage");
        Health -= damage;
        SetHealth(Health);
    }

    public void SetHealth(float health)
    {   
        healthBar.fillAmount = health / maxHealth;
    }

    private void Update()
    {
        if(takeDamage == true)
        {
            healthBar.enabled = true;
            takeDamage = false;
            TakeDamage(damage);
        }




        //Attack indicator thingies below
        
        if (attacking  == true && stunned == false)
        {
            attackIndicator.enabled = true;
            timeToAttack += Time.deltaTime;
            attackIndicator.fillAmount = timeToAttack / defaultTimeToAttack;
            if(attacked == true)
            {   
                timeToAttack = 0;
                attacked = false;
                Debug.Log("Stunned");
                stunned = true;
                Invoke("UnStunned", stunTimer);
            }
            if(timeToAttack >= defaultTimeToAttack)
            {
                Attack(); // Attack method
                timeToAttack = 0;
                attackIndicator.fillAmount = 0;
                attacking = false;
            }
        }
        
    }

    public void Attack()
    {
        Debug.Log("Swing");
    }


    void UnStunned()
    {
        stunned = false;
        
    }





}
