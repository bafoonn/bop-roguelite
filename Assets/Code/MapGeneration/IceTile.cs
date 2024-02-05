using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class IceTile : MonoBehaviour
    {
        private SlipperyStatus slipperyStatus = new SlipperyStatus();
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent(out ICharacter character))
            {
                character.Status.ApplyStatus(slipperyStatus);
                
            }
        }
        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.TryGetComponent(out ICharacter character))
            {
                character.Status.UnapplyStatus(slipperyStatus);
            }
        }
    }
}
