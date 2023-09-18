using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private Region[] regions;
    [SerializeField]
    private int regionIndex = -1;

    // Start is called before the first frame update
    void Start()
    {
        ChangeRegion();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ChangeRegion()
    {
        if (regionIndex != regions.Length)
        {
            // Makes sure all regions are inactive before changing to the next one
            for (int i = 0; i < regions.Length; i++)
            {
                regions[i].gameObject.SetActive(false);
            }

            regionIndex++;

            // Regions are in order in a list which this loop activates in order, 
            // a new level generation is called by the level when it's over
            for (int i = 0; i < regions.Length; i++)
            {
                if (i == regionIndex)
                {
                    regions[i].gameObject.SetActive(true);
                    regions[i].GenerateLevel(0);
                }
            }
        }
        else
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        Debug.Log("Game over");
    }
}
