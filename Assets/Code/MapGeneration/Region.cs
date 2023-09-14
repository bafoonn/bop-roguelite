using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Region : MonoBehaviour
{
    [SerializeField]
    private Level[] levels;

    [SerializeField]
    private int levelIndex = 0;

    private LevelManager levelManager;
    private EndPoints endPoints;
    private Level activeLevel;






    private float timer = 5;

    // Start is called before the first frame update
    void Start()
    {
        levelManager = GetComponentInParent<LevelManager>();
    }
    private void Update()
    {
        /*timer -= Time.deltaTime;
        if (timer <= 0)
        {
            GenerateLevel();
            timer = 5;
        }*/
    }

    public void GenerateLevel()
    {
        if (levelIndex != 0)
        {
            Destroy(activeLevel.gameObject);
        }
        levelIndex++;

        if (levelIndex == 6)
        {
            levelManager.ChangeRegion();
            levelIndex = 0;
        }

        int random = Random.Range(0, levels.Length);
        activeLevel = Instantiate(levels[random], transform.position, Quaternion.identity);
    }
}
