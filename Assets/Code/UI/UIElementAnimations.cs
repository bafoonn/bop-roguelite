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
        private float highlightShakeIntensityX = 0f;
        [SerializeField]
        private float highlightShakeIntensityY = 5f;
        [SerializeField]
        private float highlightShakeDuration = 0.6f;
        [SerializeField]
        private float highlightShakeSpeed = 20f;

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
        private float shakeSpeed = 1f;
        private float shakeTime = 0f;

        private float rotationShakeDuration = 0.5f;
        private float rotationShakeIntensity = 1f;
        private float rotationShakeSpeed = 1f;
        private float rotationShakeTime = 0f;

        private float popIntensity = 1.5f;
        private float popDuration = 1f;
        private float popTime = 1f;

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
        private bool shouldRotationShake = false;
        private bool shouldPop = false;


        void Start()
        {
            //rectTransform = gameObject.transform.GetChild(0).GetComponent<RectTransform>();
        }

        void Update()
        {
            if (shouldShake)
            {
                if (shakeTime >= shakeDuration)
                {
                    shouldShake = false;
                    shakeTime = 0f;
                } else
                {
                    UpdateShake();
                }
            }
            if (shouldRotationShake)
            {
                if (rotationShakeTime >= rotationShakeDuration)
                {
                    shouldRotationShake = false;
                    rotationShakeTime = 0f;
                }
                else
                {
                    UpdateRotationShake();
                }
            }
            if (shouldPop)
            {
                if (popTime >= popDuration)
                {
                    shouldPop = false;
                    popTime = 0f;
                    targetScale = 1f;
                }
                else
                {
                    UpdatePop();
                }
            }

            //TODO: Change certain effects to not use lerp

            currentRotation = Mathf.Lerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
            currentScale = Mathf.Lerp(currentScale, targetScale, scaleSpeed * Time.deltaTime);
            currentXOffset = Mathf.Lerp(currentXOffset, targetXOffset, xOffsetSpeed * Time.deltaTime);
            currentYOffset = Mathf.Lerp(currentYOffset, targetYOffset, yOffsetSpeed * Time.deltaTime);

            rectTransform.localPosition = new Vector3(currentXOffset, currentYOffset, 0f);
            rectTransform.localRotation = Quaternion.Euler(0f, 0f, currentRotation);
            rectTransform.localScale = new Vector3(currentScale, currentScale, 1f);
        }

        public void SetRectTransform(RectTransform rectTransform)
        {
            this.rectTransform = rectTransform;
        }

        public void Highlight()
        {
            //Debug.Log("Highlight");
            Highlight(highlightScale, highlightScaleSpeed);
        }

        public void Highlight(float scale, float scaleSpeed)
        {
            this.scaleSpeed = scaleSpeed;
            targetScale = scale;

            ShakeSmooth(highlightShakeIntensityX, highlightShakeIntensityY, highlightShakeDuration, highlightShakeSpeed);
        }

        public void Shake(float xIntensity)
        {

        }

        public void PickUp()
        {

        }


        private void ShakeSmooth(float xIntensity, float yIntensity, float duration, float shakeSpeed)
        {
            shouldShake = true;
            shakeDuration = duration;
            shakeTime = 0f;
            shakeXIntensity = xIntensity;
            shakeYIntensity = yIntensity;
            this.shakeSpeed = shakeSpeed;

            xOffsetSpeed = 1000f;
            yOffsetSpeed = 1000f;
        }

        private void RotationShake(float intensity, float duration, float shakeSpeed)
        {
            shouldRotationShake = true;
            rotationShakeDuration = duration;
            rotationShakeTime = 0f;
            rotationShakeSpeed = shakeSpeed;
            rotationShakeIntensity = intensity;

            rotationSpeed = 1000f;
        }

        public void Pop(float intensity, float duration)
        {
            shouldPop = true;
            popDuration = duration;
            popIntensity = intensity;
            popTime = 0f;

            scaleSpeed = 1000f;
        }

        private void UpdateShake()
        {
            shakeTime += Time.deltaTime;
            float timeIntensity = (1 + Mathf.Cos((shakeTime / shakeDuration) * Mathf.PI)) / 2;
            float sine = Mathf.Sin(shakeTime * shakeSpeed);
            float xValue = sine * shakeXIntensity * timeIntensity;
            float yValue = sine * shakeYIntensity * timeIntensity;

            targetXOffset = xValue;
            targetYOffset = yValue;
        }
        private void UpdateRotationShake()
        {
            rotationShakeTime += Time.deltaTime;
            float timeIntensity = (1 + Mathf.Cos((rotationShakeTime / rotationShakeDuration) * Mathf.PI)) / 2;
            float sine = Mathf.Sin(rotationShakeTime * rotationShakeSpeed);
            float rotation = sine * rotationShakeIntensity * timeIntensity;

            targetRotation = rotation;
        }

        private void UpdatePop()
        {
            popTime += Time.deltaTime;
            float sine = Mathf.Sin((popTime / popDuration) * (Mathf.PI));
            float scale = 1f + (sine * popIntensity);
            Debug.Log(sine);

            targetScale = scale;
        }

        public void SoftReset()
        {
            scaleSpeed = scaleResetSpeed;
            //Debug.Log("Soft Reset");
            targetRotation = 0f;
            targetScale = 1f;
            targetXOffset = 0f;
            targetYOffset = 0f;

            shouldShake = false;
            shouldRotationShake = false;
            shakeTime = 0f;
            rotationShakeTime = 0f;
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

            shouldShake = false;
            shouldRotationShake = false;
            shakeTime = 0f;
            rotationShakeTime = 0f;
        }
    }
}
