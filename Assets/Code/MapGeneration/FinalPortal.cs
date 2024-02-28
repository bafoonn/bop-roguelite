using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class FinaPortal : MonoBehaviour
    {
        private bool isActive = false;

        // When player collides with an endpoint, level generation is called and the reward for this endpoint is passed to the next level as its reward
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player") && isActive)
            {
                LevelManager levelManager = GetComponentInParent<LevelManager>();
                if (GetComponentInParent<Region>())
                {
                    this.WaitAndRun(2f, () =>
                    {
                        HUD.Current.OpenWindow(HUD.Window.Victory, true);
                    });
                }
                levelManager.DisableShopKeeper();
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