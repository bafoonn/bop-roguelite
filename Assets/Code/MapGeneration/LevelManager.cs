using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private Region[] regions;
    [SerializeField]
    private int regionIndex;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(regions.Length);
        for (int i = 0; i < regions.Length; i++)
        {
            if (i != 0)
            {
                regions[i].gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ChangeRegion()
    {

    }
}
