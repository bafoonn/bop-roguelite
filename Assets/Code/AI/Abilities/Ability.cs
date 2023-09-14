using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    
    public class Ability : ScriptableObject
    {
        public string AbilityName = null;
        public float coolDown = 0f;
        public float damage = 1f;
        
        public virtual void Activate(GameObject parent)
        {

        }

        public virtual void Deactivate(GameObject parent)
        {

        }

    }
}
