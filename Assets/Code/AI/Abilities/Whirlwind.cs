using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Pasta
{
    [CreateAssetMenu]
    public class Whirlwind : Ability
    {
        public float radius = 1f;
        private CircleCollider2D cc2d;
        public override void Activate(GameObject p)
        {
            GameObject boss = GameObject.FindGameObjectWithTag("Boss");
            
            cc2d = boss.AddComponent<CircleCollider2D>();
            cc2d.enabled = true;
            radius = cc2d.radius;
            
            cc2d.isTrigger = true;
            GameObject weapon = boss.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject;
            weapon.transform.RotateAround(boss.transform.position, new Vector3(0, 1, 0), 2f * Time.deltaTime); // TODO: TEST

            Collider2D[] collider = Physics2D.OverlapCircleAll(boss.transform.position, radius, 6);
            foreach(Collider2D collider2D in collider)
            {
                if (collider2D.TryGetComponent(out IHittable hittable))
                {
                    hittable.Hit(damage);// DO DAMAGE!
                }
            }
            

        }

        public override void Deactivate(GameObject p)
        {
            cc2d.enabled = false;   
        }
        
    }
}
