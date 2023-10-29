using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class IndicatorForAbility : MonoBehaviour
    {
        //public Ability currentAbility;
        public float IndicatorAliveTimer = 2f;
        //public GameObject abilityGameobject;
        private Color abilitySprite;
        private DamageArea damageArea;
		// Start is called before the first frame update

		private void Start()
		{
            abilitySprite = GetComponent<SpriteRenderer>().color;
            damageArea = GetComponent<DamageArea>();
            if(damageArea != null)
			{
                damageArea.enabled = false;
			}
            AbilityIndicator();
		}
		public void AbilityIndicator()
		{
            abilitySprite.a = 55f;
            StartCoroutine("DestroyObject", IndicatorAliveTimer);
        }

        private IEnumerator ActivateAbility(float time)
		{
            yield return new WaitForSeconds(time);
            abilitySprite.a = 255f;
            if (damageArea != null)
            {
                damageArea.enabled = true;
            }
            

        }

    }
}
