using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class MeatballWallObject : MonoBehaviour
    {
        private GameObject player;
        [SerializeField] private GameObject[] meatBall;
        private Rigidbody2D rigidBody2d;

        private void Start()
        {
            rigidBody2d = GetComponent<Rigidbody2D>();
            player = GameObject.FindGameObjectWithTag("Player");
           
            int random = Random.Range(0, meatBall.Length);
            meatBall[random].SetActive(false);
            Vector3 playertransformPos = player.transform.position;
            Vector2 direction = new Vector2(playertransformPos.x - transform.position.x, playertransformPos.y - transform.position.y);
            transform.right = direction;
        }
        private void Update()
        { 
            if(rigidBody2d != null)
            {
                rigidBody2d.AddForce(transform.right);
            }
            
        }
    }
}
