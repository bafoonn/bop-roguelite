using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Pasta
{
    public class UIElementAnimations : MonoBehaviour
    {
        [SerializeField]
        private float highlightScale = 1.4f;
        [SerializeField]
        private float highlightScaleSpeed = 24f;

        [SerializeField]
        private float scaleResetSpeed = 8f;

        private RectTransform rectTransform;

        private float rotationSpeed = 5f;
        private float scaleSpeed = 24f;
        private float xOffsetSpeed = 20f;
        private float yOffsetSpeed = 20f;

        private float shakeDuration = 0.5f;
        private float shakeXIntensity = 1f;
        private float shakeYIntensity = 1f;
        private float shakeCount = 1f;
        private float shakeTime = 0f;

        private float currentRotation;
        private float currentScale = 1f;
        private float currentXOffset;
        private float currentYOffset;

        private float targetRotation;
        private float targetScale = 1f;
        private float targetXOffset;
        private float targetYOffset;

        private bool shouldUpdate = true;
        private bool shouldShake = false;


        void Start()
        {
            rectTransform = gameObject.transform.GetChild(0).GetComponent<RectTransform>();
        }

        void Update()
        {
            if (shouldShake)
            {
                if (shakeDuration <= 0f)
                {
                    shouldShake = false;
                } else
                {
                    UpdateShake(shakeTime * Time.deltaTime);
                }
            }

            currentRotation = Mathf.Lerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
            currentScale = Mathf.Lerp(currentScale, targetScale, scaleSpeed * Time.deltaTime);
            currentXOffset = Mathf.Lerp(currentXOffset, targetXOffset, xOffsetSpeed * Time.deltaTime);
            currentYOffset = Mathf.Lerp(currentYOffset, targetYOffset, yOffsetSpeed * Time.deltaTime);

            rectTransform.localPosition = new Vector3(currentXOffset, currentYOffset, 0f);
            rectTransform.localRotation = Quaternion.Euler(0f, 0f, currentRotation);
            rectTransform.localScale = new Vector3(currentScale, currentScale, 1f);
        }

        public void Highlight()
        {
            Debug.Log("Highlight");
            Highlight(highlightScale, highlightScaleSpeed);
        }

        public void Highlight(float scale, float scaleSpeed)
        {
            this.scaleSpeed = scaleSpeed;
            targetScale = scale;
        }

        private void ShakeSmooth(float xIntensity, float yIntensity, float duration, float shakeCount)
        {
            shouldShake = true;
            shakeTime = 0f;
            shakeDuration = duration;
            shakeXIntensity = xIntensity;
            shakeYIntensity = yIntensity;
            this.shakeCount = shakeCount;
        }

        private void UpdateShake(float time)
        {
            float timeIntensity = (1 + Mathf.Cos(time)) / 2f;
            float sine = Mathf.Sin(time * timeIntensity * shakeCount);
            float xValue = sine * shakeXIntensity;
            float yValue = sine * shakeYIntensity;

            targetXOffset = xValue;
            targetYOffset = yValue;
        }

        public void SoftReset()
        {
            scaleSpeed = scaleResetSpeed;
            Debug.Log("Soft Reset");
            targetRotation = 0f;
            targetScale = 1f;
            targetXOffset = 0f;
            targetYOffset = 0f;
        }

        public void HardReset()
        {
            targetRotation = 0f;
            targetScale = 1f;
            targetXOffset = 0f;
            targetYOffset = 0f;

            currentRotation = 0f;
            currentScale = 1f;
            currentXOffset = 0f;
            currentYOffset = 0f;
        }
    }
}
