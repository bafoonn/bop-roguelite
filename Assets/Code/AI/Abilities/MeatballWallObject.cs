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

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
           
            int random = Random.Range(0, meatBall.Length);
            meatBall[random].SetActive(false);
            Vector3 playertransformPos = player.transform.position;
            Vector2 direction = new Vector2(playertransformPos.x - transform.position.x, playertransformPos.y - transform.position.y);
            transform.right = direction;
            //TODO : MOVE FORWARDS
        }
    }
}
