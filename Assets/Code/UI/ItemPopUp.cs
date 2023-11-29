using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pasta
{
    public class ItemPopUp : MonoBehaviour
    {

        private GameObject nameObj;
        private GameObject descriptionObj;
        private GameObject imageObj;
        private GameObject flavorObj;
        private GameObject hieroglypsObj;
        private GameObject backgroundObj;
        private GameObject stampObj;

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
        private float currentBackgroundAlpha = 0f;
        private bool shouldIncreaseBackgoundAlpha = false;
        private Color bgColor;

        private HieroglyphCreator hgCreator;


        void Start()
        {
            nameObj = transform.Find("Name").gameObject;
            descriptionObj = transform.Find("Description").gameObject;
            imageObj = transform.Find("Image").gameObject;
            flavorObj = transform.Find("FlavorText").gameObject;
            hieroglypsObj = transform.Find("Hieroglyphs").gameObject;
            backgroundObj = transform.Find("Background").gameObject;
            stampObj = transform.Find("Stamp").gameObject;
            bgColor = backgroundObj.GetComponent<Image>().color;
            bgColor.a = 0f;

            hgCreator = GetComponent<HieroglyphCreator>();

            //Testing stuff
            Activate(testItem);
        }

        void Update()
        {
            if (shouldIncreaseBackgoundAlpha && currentBackgroundAlpha != backgroundAlphaTarget)
            {
                currentBackgroundAlpha = Mathf.MoveTowards(currentBackgroundAlpha, backgroundAlphaTarget, backgroundAlphaSpeed * Time.deltaTime);
                bgColor.a = currentBackgroundAlpha;
                backgroundObj.GetComponent<Image>().color = bgColor;
            }
            else if (!shouldIncreaseBackgoundAlpha && currentBackgroundAlpha != 0f)
            {
                currentBackgroundAlpha = Mathf.MoveTowards(currentBackgroundAlpha, 0f, backgroundAlphaSpeed * Time.deltaTime);
                bgColor.a = currentBackgroundAlpha;
                backgroundObj.GetComponent<Image>().color = bgColor;
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

            

            StartCoroutine(ShowObjects());
        }

        private IEnumerator ShowObjects()
        {

            backgroundObj.GetComponent<Image>().color = bgColor;
            nameObj.GetComponent<UIElementAnimations>().SetScale(0f);
            imageObj.GetComponent<UIElementAnimations>().SetScale(0f);
            descriptionObj.GetComponent<UIElementAnimations>().SetScale(0f);
            flavorObj.GetComponent<UIElementAnimations>().SetScale(0f);
            stampObj.GetComponent<UIElementAnimations>().SetScale(0f);

            yield return new WaitForSeconds(1f);
            shouldIncreaseBackgoundAlpha = true;
            CreateHieroglyphs(item);

            imageObj.GetComponent<UIElementAnimations>().ItemPopUp(imagePopUpSpeed, imageStartRotation);
            stampObj.GetComponent<UIElementAnimations>().ScaleFromZero(stampPopUpSpeed);
            yield return new WaitForSeconds(namePopUpDelay);
            nameObj.GetComponent<UIElementAnimations>().ScaleFromZero(namePopUpSpeed);
            yield return new WaitForSeconds(descPopUpDelay);
            descriptionObj.GetComponent<UIElementAnimations>().ScaleFromZero(descPopUpSpeed);
            yield return new WaitForSeconds(flavorPopUpDelay);
            flavorObj.GetComponent<UIElementAnimations>().ScaleFromZero(flavorPopUpSpeed);

            yield return new WaitForSeconds(5f);
            Close();
        }

        private void CreateHieroglyphs(ItemBase item)
        {
            hgCreator.CreateFromItem(item);
        }

        public bool Close()
        {
            StartCoroutine(ClosePopUp());
            return true;
        }

        private IEnumerator ClosePopUp()
        {
            nameObj.GetComponent<UIElementAnimations>().ScaleToZero(closeSpeed);
            descriptionObj.GetComponent<UIElementAnimations>().ScaleToZero(closeSpeed);
            flavorObj.GetComponent<UIElementAnimations>().ScaleToZero(closeSpeed);
            

            backgroundAlphaTarget = 0f;
            shouldIncreaseBackgoundAlpha = false;

            yield return new WaitForSeconds(imageCloseDelay);

            imageObj.GetComponent<UIElementAnimations>().ScaleToZero(imageCloseSpeed);
            imageObj.GetComponent<UIElementAnimations>().RotateTo(-180f, imageCloseSpeed);
            stampObj.GetComponent<UIElementAnimations>().ScaleToZero(stampCloseSpeed);
            stampObj.GetComponent<UIElementAnimations>().RotateTo(-180f, imageCloseSpeed);

            yield return new WaitForSeconds(hieroglyphCloseDelay);

            hgCreator.CloseHieroglyphs();

            yield return new WaitForSeconds((1f / imageCloseSpeed * 4f) + (4f));

            gameObject.Deactivate();
        }
    }
}
