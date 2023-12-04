using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class DartTrap : MonoBehaviour
    {
        [SerializeField]
        private Transform[] dartShooters;
        [SerializeField]
        private Dart dart;
        private Dart activeDart;
        [SerializeField]
        private int damage = 30;
        private bool isActivated = false;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!isActivated && col.TryGetComponent<IHittable>(out _))
            {
                for (int i = 0; i < dartShooters.Length; i++)
                {
                    activeDart = Instantiate(dart, dartShooters[i].transform.position, Quaternion.identity);
                    activeDart.transform.up = transform.position - activeDart.transform.position;
                    SpriteRenderer sprite = activeDart.GetComponent<SpriteRenderer>();
                    sprite.transform.Rotate(0, 0, 90);
                    activeDart.StartMoving(damage);
                }
                isActivated = true;
            }
        }
    }
}
