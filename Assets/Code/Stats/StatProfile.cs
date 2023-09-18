using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    [CreateAssetMenu(menuName = "Scriptables/StatProfile", fileName = "New StatProfile")]
    public class StatProfile : ScriptableObject
    {
        public float Health;
        public float Damage;
        public float MovementSpeed;
        public float AttackSpeed;
    }
}
