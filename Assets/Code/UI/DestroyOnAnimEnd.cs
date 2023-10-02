using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class DestroyOnAnimEnd : MonoBehaviour
    {
        public void DestroyParent()
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}
