using UnityEngine;

namespace Pasta
{
    [CreateAssetMenu(menuName = "Abilities/Whirlwind")]
    public class Whirlwind : Ability
    {
        public float radius;

        [SerializeField] private GameObject whirlWindObj;
        private GameObject spawnedWhirlwind;
        public override void Activate(GameObject p)
        {
            // ????


            //GameObject weapon = p.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject;
            //Debug.Log(weapon.name);
            //weapon.transform.RotateAround(p.transform.position, new Vector3(0, 1, 0), 2f * Time.deltaTime); // TODO: TEST
            spawnedWhirlwind = Instantiate(whirlWindObj, p.transform);
            spawnedWhirlwind.GetComponent<WhirlwindObj>().damage = damage;
            spawnedWhirlwind.GetComponent<WhirlwindObj>().radius = radius;
            spawnedWhirlwind.GetComponent<WhirlwindObj>().activetime = ActiveTime;

        }



        public override void Deactivate()
        {
            Destroy(spawnedWhirlwind);
        }

    }
}