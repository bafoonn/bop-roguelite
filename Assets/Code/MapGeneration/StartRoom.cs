using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class StartRoom : MonoBehaviour
    {
        private GameObject player;
        private EndPoints endPoints;

        [SerializeField]
        private Transform spawnPoint;

        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = spawnPoint.transform.position;
            endPoints = GetComponentInChildren<EndPoints>();
            endPoints.GenerateRoomRewards();
            endPoints.OnlyItemRewards();
        }
    }
}
