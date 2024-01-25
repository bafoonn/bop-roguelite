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
        private SpriteRenderer abilitySpriteRend;
        private DamageArea damageArea;
        [SerializeField] private GameObject ActivateAbilityObj;
		// Start is called before the first frame update

		private void Start()
		{
            abilitySprite = GetComponent<SpriteRenderer>().color;
            abilitySpriteRend = GetComponent<SpriteRenderer>();
            damageArea = GetComponent<DamageArea>();
            if(damageArea != null)
			{
                damageArea.enabled = false;
			}
            if(ActivateAbilityObj != null)
            {
                ActivateAbilityObj.SetActive(false);
            }
            AbilityIndicator();
		}
		public void AbilityIndicator()
		{
            //abilitySprite.a = 0.25f;
            abilitySprite = new Color(1, 1, 1, 0.25f);
            abilitySpriteRend.color = abilitySprite;
            StartCoroutine("ActivateAbility", IndicatorAliveTimer);
        }

        private IEnumerator ActivateAbility(float time)
		{
            yield return new WaitForSeconds(time);
            abilitySprite = new Color(1, 1, 1, 1);
            abilitySpriteRend.color = abilitySprite;
            if(ActivateAbilityObj != null)
            {
                ActivateAbilityObj.SetActive(true);
            }
            
            if (damageArea != null)
            {
                damageArea.enabled = true;
            }
            

        }

      

    }
}
