using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIData : MonoBehaviour
{
    public List<Transform> targets = null; // List of targets
    public Collider2D[] obstacles = null; // Add obstacles here
    public Transform currentTarget;

    private void Awake()
    {
        
    }

    public int GetTargetsCount() => targets == null ? 0 : targets.Count; //Avoid having null
}
