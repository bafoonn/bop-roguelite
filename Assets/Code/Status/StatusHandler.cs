using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class StatusHandler : MonoBehaviour
    {

        private ICharacter _character;

        private Dictionary<IStatusEffect, Coroutine> _activeEffects = new();

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

        /// <summary>
        /// Applies a status effect. If duration is not provided (is zero) effect will be applied and only unapplies by manually calling UnapplyStatus
        /// </summary>
        /// <param name="statusEffect">Status to be applied.</param>
        /// <param name="duration">Duration of the status.</param>
        /// <returns>True if status is not applied yet, false otherwise.</returns>
        public bool ApplyStatus(IStatusEffect statusEffect, float duration = 0)
        {
            if (_activeEffects.ContainsKey(statusEffect))
            {
                return false;
            }

            _activeEffects.Add(statusEffect, StartCoroutine(StatusEffect(statusEffect, duration)));
            return true;
        }

        /// <summary>
        /// Unapplies the specified effect if it is applied.
        /// </summary>
        /// <param name="statusEffect">Status to be unapplied</param>
        /// <returns>True if status was applied before, false otherwise.</returns>
        public bool UnapplyStatus(IStatusEffect statusEffect)
        {
            if (!_activeEffects.ContainsKey(statusEffect))
            {
                return false;
            }

            var coroutine = _activeEffects[statusEffect];
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }

            _activeEffects.Remove(statusEffect);
            statusEffect.UnApply(_character);
            return true;
        }

        private IEnumerator StatusEffect(IStatusEffect effect, float duration)
        {
            float timer = 0;
            effect.Apply(_character, duration);
            while (timer <= duration)
            {
                effect.Update(Time.deltaTime);
                yield return null;
                if (duration != 0) timer += Time.deltaTime;
            }
            effect.UnApply(_character);
            _activeEffects.Remove(effect);
        }

        private void OnDestroy()
        {
            foreach (var pair in _activeEffects)
            {
                pair.Key.UnApply(_character);
                if (pair.Value != null) StopCoroutine(pair.Value);
            }
            _activeEffects.Clear();
        }
    }
}
