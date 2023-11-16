using System.Linq;
using UnityEngine;

namespace Pasta
{
    [CreateAssetMenu(menuName = "Scriptables/Music", fileName = "New Music")]
    public class Music : Sound
    {
        [field: SerializeField] public GameStateType State { get; private set; }
    }
}
