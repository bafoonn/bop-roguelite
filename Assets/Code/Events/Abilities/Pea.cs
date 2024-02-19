using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Pea : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;
        private Vector2 _direction;
        private float _speed;
        public float Damage;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            GetComponent<Collider2D>().isTrigger = true;
        }

        public static Pea Spawn(Pea prefab, float damage, Vector2 position)
        {
            var newPea = Instantiate(prefab, position, Quaternion.identity);
            newPea.Damage = damage;
            newPea.gameObject.Activate();
            return newPea;
        }

        public void Shoot(Vector2 dir, float speed)
        {
            _direction = dir;
            _speed = speed;
        }

        private void FixedUpdate()
        {
            _rigidbody.MovePosition(_rigidbody.position + _direction * _speed * Time.fixedDeltaTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<IEnemy>(out var enemy))
            {
                enemy.Hit(Damage);
            }
        }
    }
}
