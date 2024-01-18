using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class PlayerWeaponHelper : MonoBehaviour
    {
        private PlayerInput _input;
        [SerializeField] private CleavingWeaponAnimations _weaponAnimations;

        private void Start()
        {
            _input = GetComponentInParent<PlayerInput>();
        }

        private void Update()
        {
            _weaponAnimations.SetAim(_input.Aim);
        }
    }
}
