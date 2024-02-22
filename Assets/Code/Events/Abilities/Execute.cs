using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class Execute : ItemAbility
    {
        [SerializeField, Range(0, 1f)] private float _executeThreshold = 0.1f;
        [SerializeField] private float _bossThresholdMultiplier = 0.5f;
        [SerializeField] private float _thresholdGainPerStack = 0.01f;

        private float Threshold => _executeThreshold + _thresholdGainPerStack * (_item.Amount - 1);

        protected override void Trigger(EventContext context)
        {
            if (context is HitContext hitContext)
            {
                if (hitContext.Target is IEnemy enemy)
                {
                    float threshold = enemy.IsBoss ? Threshold * _bossThresholdMultiplier : Threshold;
                    ExecuteCharacter(enemy, threshold);
                }
            }
        }

        public static bool ExecuteCharacter(ICharacter character, float threshold)
        {
            if (character == null) return false;

            float currentHealth = character.Health.CurrentHealth;
            float maxHealth = character.Health.MaxHealth;

            float percentage = currentHealth / maxHealth;
            if (percentage < threshold)
            {
                character.Health.TakeDamage(maxHealth);
                return true;
            }

            return false;
        }
    }
}
