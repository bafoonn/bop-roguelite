using Pasta;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponParent : MonoBehaviour
{
    //public SpriteRenderer characterRenderer, weaponRenderer;
    public Vector2 EnemyWeaponPos { get; set; } // Replace this!

    private GameObject AttackIndicatorImage;
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject ProjectileSpawnPoint;
    private Collider2D AttackCollider;



    //public Transform circleOrigin;
    public float radius;

    private void Start()
    {
        AttackCollider = GetComponent<BoxCollider2D>();
        AttackIndicatorImage = gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;

    }

    private void Update()
    {

        Vector2 direction = (EnemyWeaponPos - (Vector2)transform.position).normalized;
        transform.right = direction;

        Vector2 scale = transform.localScale;
        if (direction.x < 0)
        {

            scale.x = -0.2f;
        }
        else if (direction.x > 0)
        {
            scale.x = 0.2f; // chabged to x from y
        }
        transform.localScale = scale;


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.TryGetComponent<IHittable>(out var hittable))
            {
                hittable.Hit(10);
            }
        }
    }

    public void Attack()
    {
        AttackCollider.enabled = true;
        StartCoroutine(StopAttack());
    }
    private IEnumerator StopAttack() // TEST STUFF
    {
        yield return new WaitForSeconds(0.1f);
        AttackCollider.enabled = false;
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
