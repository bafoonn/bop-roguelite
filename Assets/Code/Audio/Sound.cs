using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Pasta
{
    public class Sound : ScriptableObject
    {
        [field: SerializeField] public AudioClip[] Clips { get; private set; }
        public AudioClip RandomClip => Clips.ElementAtOrDefault(Random.Range(0, Clips.Length));
    }
}
