using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class MeatballWallObject : MonoBehaviour
    {
        [SerializeField] private GameObject[] meatBall;

        private void Start()
        {
            int random = Random.Range(0, meatBall.Length);

            meatBall[random].SetActive(false);
        }
    }
}
