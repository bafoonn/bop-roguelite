using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

namespace Pasta
{
    public class AbilityMoveTowards : MonoBehaviour
    {
        private GameObject player;
        public float speed = 1f;
        int layer;
        public LayerMask WhatLayerDestroysThis;
        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            layer = WhatLayerDestroysThis;
        }
        // Update is called once per frame
        void Update()
        {
            var distance = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, distance);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.layer == layer)
            {
                //TODO: ADD SOME ANIM HERE OR SOMETHING :)
                Destroy(gameObject);
            }
        }
    }
}
