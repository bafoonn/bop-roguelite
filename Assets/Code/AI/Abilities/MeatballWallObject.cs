using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Pasta
{
    public class MeatballWallObject : MonoBehaviour
    {
        private GameObject player;
        [SerializeField] private GameObject[] meatBall;
        private Rigidbody2D rigidBody2d;
        [SerializeField] private float speed;
        private Vector3 target;
        private WallofMeatballs ablilityOrigin;
        private Tilemap tileMap;
        private void Start()
        {
            tileMap = FindFirstObjectByType<Tilemap>();
            rigidBody2d = GetComponent<Rigidbody2D>();
            player = GameObject.FindGameObjectWithTag("Player");
            int random = Random.Range(0, meatBall.Length);
            meatBall[random].SetActive(false);
            Vector3 playertransformPos = player.transform.position;
            Vector2 direction = new Vector2(0 - transform.position.x, 0 - transform.position.y);
            transform.right = direction;
            ablilityOrigin = FindFirstObjectByType<WallofMeatballs>();
            var top = new Vector3(0, tileMap.cellBounds.yMax);
            var bottom = new Vector3(0, tileMap.cellBounds.yMin);
            var left = new Vector3(tileMap.cellBounds.xMin, 0);
            var right = new Vector3(tileMap.cellBounds.xMax, 0);
            if ((left - transform.position).magnitude < 5f)
            {
                target = new Vector2(tileMap.cellBounds.xMax, 0);
                
            }
            if((right - transform.position).magnitude < 5f)
            {
                target = new Vector2(tileMap.cellBounds.xMin, 0);
            }
            if ((top - transform.position).magnitude < 5f)
            {
                target = new Vector2(0, tileMap.cellBounds.yMin);
            }
            if ((bottom - transform.position).magnitude < 5f)
            {
                target = new Vector2(0, tileMap.cellBounds.yMax);
            }
            Invoke("OnDestroy", 25);
        }

        private void OnDestroy()
        {
            Destroy(this.gameObject);
        }

        private void Update()
        {
            var distance = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target, distance);
        }
    }
}
