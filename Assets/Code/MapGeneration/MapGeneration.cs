using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGeneration : MonoBehaviour
{
    private EndPoint[] endpoints;
    [SerializeField] private Tilemap map;

    // Start is called before the first frame update
    void Start()
    {
        map.CompressBounds();
        Debug.Log(map.size);
        endpoints = GetComponentsInChildren<EndPoint>();
        for (int i = 0; i < endpoints.Length; i++)
        {
            endpoints[i].gameObject.SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateSlotForEndPoint()
    {

    }
}
