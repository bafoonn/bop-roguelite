using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class StatusHandler : MonoBehaviour
    {

        private ICharacter _character;

        private List<IStatusEffect> _appliedEffects = new();

        private void Start()
        {
            if (_character == null)
            {
                enabled = false;
                return;
            }
        }

        public void Setup(ICharacter character)
        {
            _character = character;
            enabled = true;
        }

        public bool AddStatus(IStatusEffect statusEffect, float duration)
        {
            if (_appliedEffects.Contains(statusEffect))
            {
                return false;
            }

            StartCoroutine(StatusEffect(statusEffect, duration));
            return true;
        }

        private IEnumerator StatusEffect(IStatusEffect effect, float duration)
        {
            _appliedEffects.Add(effect);
            effect.Apply(_character);
            yield return new WaitForSeconds(duration);
            _appliedEffects.Remove(effect);
            effect.UnApply();
        }

        private void OnDestroy()
        {
            while (_appliedEffects.Count > 0)
            {
                _appliedEffects[0].UnApply();
                _appliedEffects.RemoveAt(0);
            }
        }
    }
}
