using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class DartTrap : Trap
    {
        private DartShooter[] dartShooters;
        private PressurePlate[] pressurePlates;
        [SerializeField]
        private Dart dart;
        private Dart activeDart;
        [SerializeField]
        private int damage = 30;
        private float cooldown = 3f;
        private bool onCooldown;

        private void Start()
        {
            dartShooters = GetComponentsInChildren<DartShooter>();
            pressurePlates = GetComponentsInChildren<PressurePlate>();
        }

        public void ActivateTrap()
        {
            if (!Disabled && !onCooldown)
            {
                for (int i = 0; i < dartShooters.Length; i++)
                {
                    dartShooters[i].Shoot(damage, dart);
                }
                StartCoroutine(Cooldown());
            }
        }

        IEnumerator Cooldown()
        {
            onCooldown = true;
            yield return new WaitForSeconds(cooldown);
            onCooldown = false;
            for (int i = 0; i < pressurePlates.Length; i++)
            {
                pressurePlates[i].ResetPlate();
            }
        }
    }
}
