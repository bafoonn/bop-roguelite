using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    [RequireComponent(typeof(StatusUI))]
    public class PlayerStatusHelper : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<StatusUI>().Setup(Player.Current.Status);
            Destroy(this);
        }
    }
}
