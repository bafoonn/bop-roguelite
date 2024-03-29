using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class FinaPortal : MonoBehaviour
    {

        // When player collides with an endpoint, level generation is called and the reward for this endpoint is passed to the next level as its reward
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                LevelManager levelManager = GetComponentInParent<LevelManager>();
                if (GetComponentInParent<Region>())
                {
                    HUD.Current.OpenWindow(HUD.Window.Victory, true);
                }
            }
        }
    }
}