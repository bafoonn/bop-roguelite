using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    [CreateAssetMenu(menuName = "Scriptables/StatProfile", fileName = "New StatProfile")]
    public class StatProfile : ScriptableObject
    {
        public float Health;
        [Tooltip("Damage per hit")]
        public float Damage = 10;
        public float MovementSpeed = 5;
        public float MaximumMovementSpeed = 15f;
        [Tooltip("Attacks per second")]
        public float AttackSpeed = 3;
    }
}
