using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class StatusTest : MonoBehaviour
    {
        private SlipperyStatus _status = new SlipperyStatus();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<ICharacter>(out var character))
            {
                character.Status.ApplyStatus(_status);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent<ICharacter>(out var character))
            {
                character.Status.UnapplyStatus(_status);
            }
        }
    }
}
