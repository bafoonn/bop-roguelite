using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class PlayerUIAnimationController : MonoBehaviour
    {

        [SerializeField]
        private UIElementAnimations coins;
        private bool haveCoins;

        [SerializeField]
        private UIElementAnimations coinsText;
        private bool haveCoinsText;

        [SerializeField]
        private UIElementAnimations heart;
        private bool haveHeart;

        [SerializeField]
        private UIElementAnimations heartText;
        private bool haveHeartText;

        [SerializeField]
        private float coinPickUpPopIntensity = 2f;
        [SerializeField]
        private float coinPickUpDuration = 1f;

        void Start()
        {
            haveCoins = coins != null;
            haveCoinsText = coinsText != null;
            haveHeart = heart != null;
            haveHeartText = heartText != null;

            if (haveCoins) coins.SetRectTransform(coins.GetComponent<RectTransform>());
            if (haveHeart) heart.SetRectTransform(heart.GetComponent<RectTransform>());
            if (haveCoinsText) coinsText.SetRectTransform(coinsText.GetComponent<RectTransform>());
            if (haveHeartText) heartText.SetRectTransform(heartText.GetComponent<RectTransform>());
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void CoinPickup()
        {
            if (haveCoins)
            {
                coins.Pop(coinPickUpPopIntensity, coinPickUpDuration);
            }
        }

        public void TakeDamage()
        {
            if (haveHeart)
            {

            }
        }
    }
}
