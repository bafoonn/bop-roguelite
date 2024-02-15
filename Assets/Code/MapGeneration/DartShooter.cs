using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class DartShooter : MonoBehaviour
    {
        [SerializeField]
        private Transform target;
        private Dart dart;
        private float damage;
        private Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }
        public void Shoot(float passedDamage, Dart dartPrefab)
        {
            dart = dartPrefab;
            damage = passedDamage;
            animator.SetTrigger("Shoot");
            animator.SetTrigger("Default");
        }

        public void ShootDart()
        {
            Dart activeDart = Instantiate(dart, transform.position, Quaternion.identity);
            activeDart.transform.right = transform.position - target.position;
            SpriteRenderer sprite = activeDart.GetComponent<SpriteRenderer>();
            sprite.transform.Rotate(0, 0, 180);
            activeDart.StartMoving(damage);
        }
    }
}
