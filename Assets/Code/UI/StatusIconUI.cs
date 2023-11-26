using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Pasta
{
    public class StatusIconUI : MonoBehaviour
    {
        public StatusType StatusType;

        [SerializeField] private Image _image;
        [SerializeField] private Text _text;
        private int _current;

        private void Awake()
        {
            _image = GetComponentInChildren<Image>();
            _text = GetComponentInChildren<Text>();
            SetCount(0);
        }

        public void Add()
        {
            SetCount(_current + 1);
        }

        public void Remove()
        {
            SetCount(_current - 1);
        }

        public void SetCount(int count)
        {
            _current = count;
            _text.text = count > 0 ? count.ToString() : string.Empty;
            gameObject.SetActive(count > 0);
        }
    }
}
