using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

namespace Pasta
{
    public class AbilityMoveTowards : MonoBehaviour
    {
        private GameObject player;
        public float speed = 1f;
        public LayerMask WhatLayerDestroysThis;
        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        // Update is called once per frame
        void Update()
        {
            var distance = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, distance);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("triggering with " + " " + collision.gameObject.layer);
            if (WhatLayerDestroysThis.Includes(collision.gameObject.layer))
            {
                Debug.Log("triggered with obstacle");
                //TODO: ADD SOME ANIM HERE OR SOMETHING :)
                Destroy(gameObject);
            }
        }

    }
}
