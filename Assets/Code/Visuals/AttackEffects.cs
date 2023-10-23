using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Pasta
{
    public class AttackEffects : MonoBehaviour
    {
        [SerializeField] private Transform rotationSource;
        
        private VisualEffect effect;
        public float _rotation;

        private bool _shouldUpdateRotation = true;

        private bool _isFlipped = true;

        void Start()
        {
            effect = GetComponent<VisualEffect>();
        }

        void Update()
        {
            if (_shouldUpdateRotation)
            {
                _rotation = rotationSource.rotation.eulerAngles.z;
                effect.SetFloat("Rotation", _rotation);
            }
        }

        public void QuickAttack()
        {
            _isFlipped = !_isFlipped;
            effect.SetBool("QA Is Flipped", _isFlipped);
            effect.SendEvent("QuickAttack");
        }
        public void HeavyAttack()
        {
            _isFlipped = !_isFlipped;
            effect.SetBool("HA Is Flipped", _isFlipped);
            effect.SendEvent("HeavyAttack");
        }

        public void AttackIndicator()
        {
            effect.SetBool("Indicator Alive", true);
            effect.SendEvent("Indicator");
        }

        public void CancelAttack()
        {
            effect.SetBool("Indicator Alive", false);
        }
    }
}
