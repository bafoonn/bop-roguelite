using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    private EndPoints endPoints;
    private GameObject player;
    private GameObject[] activeEnemies;
    private int enemiesLeft;

    [SerializeField]
    private Transform spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        endPoints = GetComponentInChildren<EndPoints>();
        endPoints.gameObject.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = spawnPoint.transform.position;
    }

    public void AddToEnemyCount()
    {
        enemiesLeft++;
    }
    public void EnemyKilled()
    {
        enemiesLeft--;
        if (enemiesLeft == 0)
        {
            endPoints.gameObject.SetActive(true);
        }
    }

}
