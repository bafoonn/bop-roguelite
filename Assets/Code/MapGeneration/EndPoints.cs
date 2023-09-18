using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoints : MonoBehaviour
{
    [SerializeField]
    private EndPoint[] endPoints;
    private Region region;
    // Start is called before the first frame update
    void Start()
    {
        region = GetComponentInParent<Region>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // TODO: Return reward for the next level
            region.GenerateLevel();
        }
    }

}
