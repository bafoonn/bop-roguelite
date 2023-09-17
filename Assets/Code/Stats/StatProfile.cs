using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    [CreateAssetMenu(menuName = "Scriptables/StatProfile", fileName = "New StatProfile")]
    public class StatProfile : ScriptableObject
    {
        public Stat Health;
        public Stat Damage;
        public Stat MovementSpeed;
        public Stat AttackSpeed;
    }
}
