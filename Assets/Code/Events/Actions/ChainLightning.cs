using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class ChainLightning : EventAction
    {
        [SerializeField] private float _procChance = 0.3f;
        [SerializeField] private float _radius = 5f;
        [SerializeField] private int _chainCount = 3;
        [SerializeField] private float _chainTime = 0.5f;

        protected override void Init()
        {
            base.Init();
        }

        protected override void Trigger()
        {
            base.Trigger();
            if (Random.value < _procChance)
            {
                StartCoroutine(SpawnLightning());
            }
        }

        private IEnumerator SpawnLightning()
        {
            yield return new WaitForSeconds(_chainTime);
        }
    }
}
