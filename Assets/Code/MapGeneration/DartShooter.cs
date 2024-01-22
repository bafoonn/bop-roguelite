using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class DartShooter : MonoBehaviour
    {
        [SerializeField]
        private Transform target;
        public void Shoot(float damage, Dart dart)
        {
            Dart activeDart = Instantiate(dart, transform.position, Quaternion.identity);
            activeDart.transform.right = transform.position - target.position;
            SpriteRenderer sprite = activeDart.GetComponent<SpriteRenderer>();
            sprite.transform.Rotate(0, 0, 180);
            activeDart.StartMoving(damage);
        }
    }
}
