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

        void Awake()
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

        public void SetQuickAttackLifetime(float lifetime)
        {
            effect.SetFloat("QA Lifetime", lifetime);
        }
        public void SetHeavyAttackLifetime(float lifetime)
        {
            effect.SetFloat("HA Lifetime", lifetime);
        }

        public void SetIndicatorLifetime(float lifetime)
        {
            effect.SetFloat("Ind Lifetime", lifetime);
        }

        public void SetQuickAttackScale(float scaleX, float scaleY)
        {
            effect.SetVector2("QA Scale", new Vector2(scaleX, scaleY));
        }

        public void SetHeavyAttackScale(float scaleX, float scaleY)
        {
            effect.SetVector2("HA Scale", new Vector2(scaleX, scaleY));
        }

        public void SetIndicatorScale(float scaleX, float scaleY)
        {
            effect.SetVector2("Ind Scale", new Vector2(scaleX, scaleY));
        }
    }
}
