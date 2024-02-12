using System.Collections.Generic;
using UnityEngine;

public class AIData : MonoBehaviour
{
    public List<Transform> targets = null; // List of targets
    public Collider2D[] obstacles = null; // Adds obstacles here
    public Transform currentTarget;


    public int GetTargetsCount() => targets == null ? 0 : targets.Count; //Avoid having null
}
