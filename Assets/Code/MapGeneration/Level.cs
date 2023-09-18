using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    private EndPoints endPoints;
    private GameObject player;

    [SerializeField]
    private Transform spawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        endPoints = GetComponentInChildren<EndPoints>();
        // endPoints.gameObject.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = spawnPoint.transform.position;
    }

    public void CheckIfRoomComplete()
    {
        if (GameObject.FindGameObjectWithTag("Enemy") == null)
        {
            endPoints.gameObject.SetActive(true);
        }
    }

}
