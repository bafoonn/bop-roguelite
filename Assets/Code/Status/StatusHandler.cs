using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Pasta
{
    public class StatusHandler : MonoBehaviour
    {
        private ICharacter _character;

        private Dictionary<IStatusEffect, Coroutine> _activeEffects = new();
        public List<StatusType> ActiveEffects
        {
            get
            {
                List<StatusType> effectTypes = new();
                foreach (var effect in _activeEffects.Keys)
                {
                    effectTypes.Add(effect.Type);
                }
                return effectTypes;
            }
        }

        public event Action<StatusType> OnApply;
        public event Action<StatusType> OnUnapply;

        public void Setup(ICharacter character)
        {
            _character = character;
            var statusUI = GetComponentInChildren<StatusUI>();
            if (statusUI != null)
            {
                statusUI.Setup(this);
            }
        }

        /// <summary>
        /// Applies a status effect. If duration is zero the effect will be applied and only unapplies by manually calling UnapplyStatus
        /// </summary>
        /// <param name="statusEffect">Status to be applied.</param>
        /// <param name="duration">Duration of the status.</param>
        /// <returns>True if status is not applied yet, false otherwise.</returns>
        public bool ApplyStatus(IStatusEffect statusEffect, float duration)
        {
            if (_activeEffects.ContainsKey(statusEffect))
            {
                return false;
            }

            if (statusEffect.CanStack == false)
            {
                var activeEffect = _activeEffects.FirstOrDefault(kvp => kvp.Key.Type == statusEffect.Type).Key;
                if (activeEffect != null && statusEffect.Compare(activeEffect) < 0)
                {
                    RemoveStatus(activeEffect);
                    AddStatus(activeEffect, duration);
                    return true;
                }
            }

            AddStatus(statusEffect, duration);
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

            RemoveStatus(statusEffect);
            return true;
        }

        private void AddStatus(IStatusEffect statusEffect, float duration)
        {
            _activeEffects.Add(statusEffect, StartCoroutine(StatusEffect(statusEffect, duration)));
            statusEffect.Apply(_character, duration);
            if (OnApply != null) OnApply(statusEffect.Type);
        }

        private void RemoveStatus(IStatusEffect statusEffect)
        {
            var coroutine = _activeEffects[statusEffect];
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            _activeEffects.Remove(statusEffect);
            statusEffect.UnApply(_character);
            if (OnUnapply != null) OnUnapply(statusEffect.Type);
        }

        private IEnumerator StatusEffect(IStatusEffect effect, float duration)
        {
            bool hasTickRate = effect.Interval > 0;
            float timer = 0;
            do
            {
                effect.Update(Time.deltaTime);
                if (hasTickRate)
                {
                    timer += effect.Interval;
                    yield return new WaitForSeconds(effect.Interval);
                }
                else
                {
                    timer += Time.deltaTime;
                    yield return null;
                }
            }
            while (duration > timer);
            yield return null;
            effect.Update(Time.deltaTime);
            RemoveStatus(effect);
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
