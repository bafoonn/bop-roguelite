using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    [CreateAssetMenu(menuName = "Scriptables/StatProfile", fileName = "New StatProfile")]
    public class StatProfile : ScriptableObject
    {
        [Header("Health")]
        public float Health;
        public float MinHealth = 10;
        [Tooltip("Damage per hit")]
        [Header("Damage")]
        public float Damage = 10;
        public float MinDamage = 1f;
        [Header("Movement Speed")]
        public float MovementSpeed = 5;
        public float MinMovementSpeed = 2;
        public float MaxMovementSpeed = 15f;
        [Header("Attack Speed")]
        [Tooltip("Attacks per second")]
        public float AttackSpeed = 3;
        public float MinAttackSpeed = 0.5f;
        [Header("Dodge")]
        public int DodgeCount = 1;
        public float DodgeCooldown = 3f;
    }
}
