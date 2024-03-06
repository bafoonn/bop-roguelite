using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pasta
{
    public class BulletSpawner : MonoBehaviour
    {
        [SerializeField] private Projectile Bullet;
        private DamageArea damageArea;
        private float[] rotations;
        [SerializeField] private float howmanyDirections;
        // Start is called before the first frame update


        private void OnEnable()
        {
            ItemAbilities.OnEvent += OnEvent;
        }

        private void OnDisable()
        {
            ItemAbilities.OnEvent -= OnEvent;
        }

        private void OnEvent(EventContext obj)
        {
            if (obj.EventType != EventActionType.OnRoomEnter) return;
            Destroy(this.gameObject);
        }


        void Start()
        {
            damageArea = GetComponentInParent<DamageArea>();
            float angle = 360 / howmanyDirections;

            StartCoroutine(Deactivate());
            if (damageArea != null)
            {
                if (damageArea.enabled)
                {
                    for (int i = 0; i < howmanyDirections; i++)
                    {
                        var bullet = Instantiate(Bullet, transform.position, Quaternion.identity);
                        transform.rotation = Quaternion.Euler(0, 0, angle * i);
                        Vector2 direction = transform.right;
                        direction.Normalize();
                        bullet.Launch(direction);
                    }
                    //Instantiate(Bullet, transform.position, Quaternion.Euler(0,0,));
                }
            }
        }



        private IEnumerator Deactivate()
        {
            yield return new WaitForSeconds(2f);
            Destroy(this.transform.parent.gameObject);
        }






    }
}
