using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class DartTrap : Trap
    {
        [SerializeField]
        private DartShooter[] dartShooters;
        [SerializeField]
        private PressurePlate[] pressurePlates;
        [SerializeField]
        private Dart dart;
        private Dart activeDart;
        [SerializeField]
        private int damage = 30;

        private void Start()
        {
            
        }


        public void ActivateTrap()
        {
            Debug.Log(Disabled);
            if (!Disabled)
            {
                for (int i = 0; i < dartShooters.Length; i++)
                {
                    dartShooters[i].Shoot(damage, dart);
                }
                for (int i = 0; i < pressurePlates.Length; i++)
                {
                    StartCoroutine(pressurePlates[i].OffTimer());
                }
            }
        }
    }
}
