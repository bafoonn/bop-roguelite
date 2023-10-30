using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pasta
{
    public class StatUI : MonoBehaviour
    {
        public StatType Stat;
        private Stat _stat;
        [SerializeField] private Text _text = null;

        private void Awake()
        {
            if (_text == null)
            {
                enabled = false;
                return;
            }

            _stat = StatManager.Current.GetStat(Stat);
            SetText(_stat.Value);
        }

        private void OnEnable()
        {
            _stat.ValueChanged += SetText;
        }

        private void OnDisable()
        {
            _stat.ValueChanged -= SetText;
        }

        private void SetText(float value)
        {
            _text.text = value.ToString("#.##");
        }
    }
}
