using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    [CreateAssetMenu(menuName = "Abilities/Fireball")]
    public class Fireball : Ability
    {
        [SerializeField] private GameObject fireBall;
        private WeaponParent weapon;
        private GameObject spawnedFireball;

        public override void Activate(GameObject parent)
        {
            weapon = parent.GetComponent<WeaponParent>();
            if(weapon != null)
            {
                spawnedFireball = Instantiate(fireBall, weapon.ProjectileSpawnPoint.transform.position, weapon.ProjectileSpawnPoint.transform.rotation);
            }
            DeactivateAbility();
        }


        private IEnumerator DeactivateAbility()
        {
            yield return new WaitForSeconds(coolDown + 1);
            Destroy(spawnedFireball);
        }

        public override void Deactivate()
        {
            Destroy(spawnedFireball);
        }

    }
}
