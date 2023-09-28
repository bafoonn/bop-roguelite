using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Pasta
{
    public class MeatballWallObject : MonoBehaviour
    {
        private GameObject player;
        [SerializeField] private GameObject[] meatBall;
        private Rigidbody2D rigidBody2d;
        [SerializeField] private float speed;
        private Vector3 target;
        private void Start()
        {
            rigidBody2d = GetComponent<Rigidbody2D>();
            player = GameObject.FindGameObjectWithTag("Player");
           
            int random = Random.Range(0, meatBall.Length);
            meatBall[random].SetActive(false);
            Vector3 playertransformPos = player.transform.position;
            Vector2 direction = new Vector2(playertransformPos.x - transform.position.x, playertransformPos.y - transform.position.y);
            transform.right = direction;
            target = player.transform.position;
        }
        private void Update()
        {
            var distance = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target, distance);
        }
    }
}
