using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Pasta
{
    public class RawStatDisplay : MonoBehaviour
    {
        private Dictionary<StatType, Stat> Stats = new Dictionary<StatType, Stat>();
        private List<TextMeshProUGUI> textMeshes = new();
        [SerializeField] private TextMeshProUGUI textMeshTemplate = null;

        private void Start()
        {
            if (textMeshTemplate == null)
            {
                gameObject.Deactivate();
                return;
            }
            textMeshTemplate.gameObject.Deactivate();

            var statManager = StatManager.Current;
            foreach (var value in Enum.GetValues(typeof(StatType)))
            {
                try
                {
                    var type = (StatType)value;
                    var stat = statManager.GetStat(type);
                    Stats.Add(type, stat);
                    stat.ValueChanged += StatValueChanged;
                }
                catch (NotImplementedException)
                {
                    Debug.LogWarning($"Stat {(StatType)value} is not implemented.");
                }
            }
            UpdateTexts();
        }

        private void OnDestroy()
        {
            foreach (var stat in Stats.Values)
            {
                stat.ValueChanged -= StatValueChanged;
            }
        }

        private void StatValueChanged(float obj)
        {
            UpdateTexts();
        }

        private void UpdateTexts()
        {
            int count = 0;
            foreach (var pair in Stats)
            {
                if (count >= textMeshes.Count)
                {
                    var newTextMesh = Instantiate(textMeshTemplate, transform);
                    newTextMesh.gameObject.Activate();
                    textMeshes.Add(newTextMesh);
                }

                var textMesh = textMeshes[count];
                textMesh.text = $"{pair.Key}: {pair.Value.Value}";
                count++;
            }
        }
    }
}
