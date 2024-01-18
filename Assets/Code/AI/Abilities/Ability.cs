using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{

    public class Ability : ScriptableObject
    {
        public string AbilityName = null;
        public float coolDown = 0f;
        public float ActiveTime = 0f;
        public float damage = 1f;
        public bool usableOutsideAttackRange = false;
        public bool AbilityWithAnim = false;
        public bool randomize = false;
        public float usableAtHealthPercentage = 60;

        public virtual void Activate(GameObject parent)
        {
            
        }

        

        public virtual void Deactivate()
        {

        }

    }
}
