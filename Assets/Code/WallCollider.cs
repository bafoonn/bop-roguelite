using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    
    public class WallCollider : MonoBehaviour // This if carrot goes through walls then change them to ghostly appearance.(was a bug but can be a feature:D)
    {
        private EnemyAi enemyai;
        private Color color;
        


        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.name.Contains("Carrot"))
            {
                enemyai = collision.gameObject.GetComponent<EnemyAi>();
                color = enemyai.spriteRenderer.color;
                color.a = 0.25f;
            }
        }


        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.name.Contains("Carrot"))
            {
                enemyai = collision.gameObject.GetComponent<EnemyAi>();
                color = enemyai.spriteRenderer.color;
                color.a = 1;
            }
        }
    }
}
