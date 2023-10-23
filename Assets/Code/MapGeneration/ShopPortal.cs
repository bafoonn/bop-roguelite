using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class ShopPortal : MonoBehaviour
    {
        private bool isActive = false;
        private float timer = 0.5f;

        private void Update()
        {
            // Small time window to prevent the player from activating an endpoint the moment they are spawned
            if (timer <= 0)
            {
                isActive = true;
            }

            else
            {
                timer -= Time.deltaTime;
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player") && isActive)
            {
                Region region = GetComponentInParent<Region>();
                region.ActivateShopLevel();
            }
        }

        // Keeps the player from activating the exit in case the player is inside the endpoint when they are spawned
        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player") && !isActive)
            {
                isActive = true;
            }
        }
    }
}
