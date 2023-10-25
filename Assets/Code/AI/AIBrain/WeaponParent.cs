using Pasta;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class WeaponParent : MonoBehaviour
{
    //public SpriteRenderer characterRenderer, weaponRenderer;
    public Vector2 EnemyWeaponPos { get; set; } // Replace this!

    private GameObject AttackIndicatorImage;
    [SerializeField] private GameObject projectile;
    [SerializeField] public GameObject ProjectileSpawnPoint;
    public Vector2 direction;
    private AIData aidata;
    private Vector3 enemyDirectionLocal;
    private AgentAnimations animations;
    //public Transform circleOrigin;
    public float radius;
    [SerializeField] private GameObject detectors;
    private SpriteRenderer spriteRend;
    [SerializeField] private GameObject attackColliderHolder;
    [SerializeField] private Collider2D attackCollider;
    public bool Aim = true;
    [SerializeField] private Transform spriteTransform;
    private Vector2 weaponScale;
    private void Start()
    {
        //AttackIndicatorImage = gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;
        aidata = GetComponentInParent<AIData>();
        spriteRend = GetComponentInChildren<SpriteRenderer>();
        animations = GetComponentInParent<AgentAnimations>();
        weaponScale = transform.localScale;
    }

    private void Update()
    {

        //direction = (EnemyWeaponPos - (Vector2)transform.position);
        //transform.right = direction;
        //Vector2 scale = transform.localScale;
        //if (direction.x <= 0)
        //{

        //    scale.x = -0.2f;
        //}
        //else if (direction.x >= 0)
        //{
        //    scale.x = 0.2f; // chabged to x from y

        //}
        //transform.localScale = scale;
        if (Aim)
        {
            Vector3 weaponPos = EnemyWeaponPos; // THIS WHOLE THING IS A SHITSHOW BUT IT WORKS!
            direction = new Vector2(weaponPos.x - transform.position.x, weaponPos.y - transform.position.y);
            if (aidata.currentTarget != null)
            {
                enemyDirectionLocal = transform.InverseTransformPoint(aidata.currentTarget.transform.position);
            }

            Vector2 scale = attackColliderHolder.transform.localScale;
            transform.right = direction;
            if (enemyDirectionLocal.x < 0)
            {

                Debug.Log("LEFT");
                scale.x = -1f;
                weaponScale.x = -1f;
                weaponScale.y = 0f;
                //weaponSpriteTrans.position = new Vector3(-1, 0, 0);
                spriteRend.flipY = true;
                spriteTransform.localPosition = weaponScale;
                attackColliderHolder.transform.localScale = scale; // Changing child since editing parents scale fucks direction check
            }
            else if (enemyDirectionLocal.x > 0)
            {


                Debug.Log("RIGHT");
                scale.x = 1f;
                weaponScale.x = 1f;
                weaponScale.y = 0f;
                //weaponSpriteTrans.position = new Vector3(1, 0, 0);
                spriteRend.flipY = false;
                spriteTransform.localPosition = weaponScale;
                attackColliderHolder.transform.localScale = scale; // Changing child since editing parents scale fucks direction check
            }
        }
        
        
    }
   

    public void Attack()
    {
        attackCollider.enabled = true;
        StartCoroutine(StopAttack());

    }
    private IEnumerator StopAttack() // TEST STUFF
    {
        yield return new WaitForSeconds(0.06f);
        attackCollider.enabled = false;
        Aim = true;
        animations.aim = true;
    }
    public void RangedAttack()
    {
        Instantiate(projectile, ProjectileSpawnPoint.transform.position, ProjectileSpawnPoint.transform.rotation);
    }


    private void OnDrawGizmosSelected()
    {
        //Gizmos.color = Color.blue;
        //Vector3 position = circleOrigin == null ? Vector3.zero : circleOrigin.position;
        //Gizmos.DrawWireSphere(position, radius);
    }


}
