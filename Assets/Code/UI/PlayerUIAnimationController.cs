using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        private Image heartImage;

        [SerializeField]
        private float coinPickUpPopIntensity = 2f;
        [SerializeField]
        private float coinPickUpDuration = 1f;

        [SerializeField]
        private float takeDamageShakeIntensity = 1f;
        [SerializeField]
        private float takeDamageShakeDuration = 0.2f;
        [SerializeField]
        private float takeDamageShakeSpeed = 1f;

        void Start()
        {
            haveCoins = coins != null;
            haveCoinsText = coinsText != null;
            haveHeart = heart != null;
            haveHeartText = heartText != null;
            

            if (haveCoins) coins.SetRectTransform(coins.GetComponent<RectTransform>());
            if (haveHeart)
            {
                heart.SetRectTransform(heart.GetComponent<RectTransform>());
                heartImage = heart.gameObject.GetComponentInChildren<Image>();
                Debug.Log(heartImage);
            }
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
                heartImage.materialForRendering.SetFloat("_Color_Alpha", 0f);
                heart.ShakeSmooth(takeDamageShakeIntensity, 0f, takeDamageShakeDuration, takeDamageShakeSpeed);
            }
        }

        public void TakeDamageStart()
        {
            if (haveHeart)
            {
                heartImage.materialForRendering.SetFloat("_Color_Alpha", 1f);
            }
        }
    }
}
