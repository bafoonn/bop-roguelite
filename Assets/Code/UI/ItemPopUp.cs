using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pasta
{
    public class ItemPopUp : MonoBehaviour
    {

        private UIElementAnimations nameObj;
        private UIElementAnimations descriptionObj;
        private UIElementAnimations imageObj;
        private UIElementAnimations flavorObj;
        private GameObject hieroglypsObj;
        private Image backgroundObj;
        private UIElementAnimations stampObj;
        private UIElementAnimations close;

        public ItemBase item;

        [SerializeField] private ItemBase testItem;

        [SerializeField] private float imagePopUpSpeed = 4f;
        [SerializeField] private float imageStartRotation = -160f;
        [SerializeField] private float namePopUpSpeed = 4f;
        [SerializeField] private float namePopUpDelay = 0.1f;
        [SerializeField] private float descPopUpSpeed = 4f;
        [SerializeField] private float descPopUpDelay = 0.2f;
        [SerializeField] private float flavorPopUpSpeed = 4f;
        [SerializeField] private float flavorPopUpDelay = 0.4f;
        [SerializeField] private float stampPopUpSpeed = 6f;
        [SerializeField] private float stampCloseSpeed = 6f;

        [SerializeField] private float closeSpeed = 2f;
        [SerializeField] private float imageCloseDelay = 0.2f;
        [SerializeField] private float imageCloseSpeed = 4f;
        [SerializeField] private float hieroglyphCloseDelay = 1f;

        [SerializeField] private float backgroundAlphaTarget = 0.85f;
        [SerializeField] private float backgroundAlphaSpeed = 4f;
        private float currentBackgroundAlphaTarget = 0f;
        private float currentBackgroundAlpha = 0f;
        private Color bgColor;

        private HieroglyphCreator hgCreator;

        public bool IsActive { get; private set; }
        private Coroutine _currentCoroutine = null;

        void Awake()
        {
            nameObj = transform.Find("Name").gameObject.GetComponent<UIElementAnimations>();
            descriptionObj = transform.Find("Description").gameObject.GetComponent<UIElementAnimations>();
            imageObj = transform.Find("Image").gameObject.GetComponent<UIElementAnimations>();
            flavorObj = transform.Find("FlavorText").gameObject.GetComponent<UIElementAnimations>();
            hieroglypsObj = transform.Find("Hieroglyphs").gameObject;
            backgroundObj = transform.Find("Background").gameObject.GetComponent<Image>();
            stampObj = transform.Find("Stamp").gameObject.GetComponent<UIElementAnimations>();
            close = transform.Find("Close").gameObject.GetComponent<UIElementAnimations>();
            bgColor = backgroundObj.color;
            bgColor.a = 0f;

            hgCreator = GetComponent<HieroglyphCreator>();

            //Testing stuff
            if (testItem != null)
                Activate(testItem);
        }

        void Update()
        {
            if (currentBackgroundAlpha != currentBackgroundAlphaTarget)
            {
                currentBackgroundAlpha = Mathf.MoveTowards(currentBackgroundAlpha, currentBackgroundAlphaTarget, backgroundAlphaSpeed * Time.unscaledDeltaTime);
                bgColor.a = currentBackgroundAlpha;
                backgroundObj.color = bgColor;
            }
        }

        public void Activate(ItemBase item)
        {
            this.gameObject.SetActive(true);
            this.item = item;

            nameObj.GetComponent<Text>().text = item.Name;
            descriptionObj.GetComponent<Text>().text = item.Description;
            imageObj.GetComponent<Image>().sprite = item.Sprite;
            flavorObj.GetComponent<Text>().text = item.Flavor;
            //shouldIncreaseBackgoundAlpha = true;
            close.GetComponent<FirstSelectUIObject>().Select();


            if (_currentCoroutine != null)
            {
                StopCoroutine(_currentCoroutine);
            }
            _currentCoroutine = StartCoroutine(ShowObjects());
            Time.timeScale = 0f;
        }

        private IEnumerator ShowObjects()
        {
            nameObj.HardReset();
            imageObj.HardReset();
            descriptionObj.HardReset();
            flavorObj.HardReset();
            stampObj.HardReset();

            backgroundObj.color = bgColor;
            nameObj.SetScale(0f);
            imageObj.SetScale(0f);
            descriptionObj.SetScale(0f);
            flavorObj.SetScale(0f);
            stampObj.SetScale(0f);
            close.SetScale(0f);

            //yield return new WaitForSeconds(1f);
            currentBackgroundAlphaTarget = backgroundAlphaTarget;
            CreateHieroglyphs(item);

            imageObj.ItemPopUp(imagePopUpSpeed, imageStartRotation);
            stampObj.ScaleFromZero(stampPopUpSpeed);
            close.ScaleFromZero(imagePopUpSpeed);
            yield return new WaitForSecondsRealtime(namePopUpDelay);
            nameObj.ScaleFromZero(namePopUpSpeed);
            yield return new WaitForSecondsRealtime(descPopUpDelay);
            descriptionObj.ScaleFromZero(descPopUpSpeed);
            yield return new WaitForSecondsRealtime(flavorPopUpDelay);
            flavorObj.ScaleFromZero(flavorPopUpSpeed);

            IsActive = true;
            _currentCoroutine = null;
            //yield return new WaitForSeconds(5f);
            //Close();
        }

        private void CreateHieroglyphs(ItemBase item)
        {
            hgCreator.CreateFromItem(item);
        }

        public void Close()
        {
            if (_currentCoroutine != null)
            {
                StopCoroutine(_currentCoroutine);
            }
            _currentCoroutine = StartCoroutine(ClosePopUp());
        }

        private IEnumerator ClosePopUp()
        {
            nameObj.ScaleToZero(closeSpeed);
            descriptionObj.ScaleToZero(closeSpeed);
            flavorObj.ScaleToZero(closeSpeed);
            close.ScaleToZero(closeSpeed);

            currentBackgroundAlphaTarget = 0f;

            yield return new WaitForSecondsRealtime(imageCloseDelay);

            imageObj.ScaleToZero(imageCloseSpeed);
            imageObj.RotateTo(-180f, imageCloseSpeed);
            stampObj.ScaleToZero(stampCloseSpeed);
            stampObj.RotateTo(-180f, imageCloseSpeed);

            yield return new WaitForSecondsRealtime(hieroglyphCloseDelay);

            hgCreator.CloseHieroglyphs();
            yield return new WaitForSecondsRealtime(hgCreator.TotalCloseDelay);
            Time.timeScale = 1f;
            IsActive = false;


            gameObject.Deactivate();
            _currentCoroutine = null;
        }
    }
}
