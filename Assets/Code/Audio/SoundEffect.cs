using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    [CreateAssetMenu(menuName = "Scriptables/SoundEffect", fileName = "New SoundEffect")]
    public class SoundEffect : Sound
    {
        [SerializeField] private string soundName;
        public string Name => soundName.ToLower();
    }
}
