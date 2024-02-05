using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class SlowStatus : IStatusEffect
    {
        public int SlowPercentage => Mathf.RoundToInt(_slowPercentage * 100);
        private float _slowPercentage;
        public StatusType Type => StatusType.Slow;
        public bool CanStack => true;

        public float Interval => 0;
        private List<ICharacter> _applied = new List<ICharacter>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="percentage">Value between 0 and 100</param>
        public SlowStatus(int percentage)
        {
            percentage = Mathf.Clamp(percentage, 0, 100);
            _slowPercentage = percentage * 0.01f;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="percentage">Value between 0 and 1</param>
        public SlowStatus(float percentage)
        {
            percentage = Mathf.Clamp01(percentage);
            _slowPercentage = percentage;
        }

        public void Apply(ICharacter character, float duration)
        {
            if (_applied.Contains(character)) return;
            character.Movement.Slow += _slowPercentage;
            _applied.Add(character);
        }

        public void Update(ICharacter character, float deltaTime)
        {
        }

        public void UnApply(ICharacter character)
        {
            if (!_applied.Contains(character)) return;
            character.Movement.Slow -= _slowPercentage;
            _applied.Remove(character);
        }

        public int Compare(IStatusEffect other)
        {
            if (other is SlowStatus slowStatus)
            {
                if (slowStatus.SlowPercentage > SlowPercentage) return -1;
                if (slowStatus.SlowPercentage < SlowPercentage) return 1;
            }
            return 0;
        }
    }
}
