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

    // Start is called before the first frame update
    void Start()
    {
        levelManager = GetComponentInParent<LevelManager>();
    }

    public void GenerateLevel()
    {
        levelIndex++;

        if (levelIndex == 6)
        {
            
        }
    }
}
