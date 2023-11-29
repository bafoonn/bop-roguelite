using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pasta
{
    public class HieroglyphCreator : MonoBehaviour
    {
        [SerializeField] private GameObject hieroglyphPrefab;
        [SerializeField] private GameObject parent;

        private Vector3 position;

        [SerializeField] private float popUpStartDelay = 0.4f;
        [SerializeField] private float popUpDelay = 0.15f;
        [SerializeField] private float popUpSpeed = 6f;

        [SerializeField] private float closeDelay = 0.15f;
        [SerializeField] private float closeSpeed = 6f;

        [SerializeField] private float xOffset = 18f;
        [SerializeField] private float yOffset = 22f;

        private List<GameObject> hgList;

        void Start()
        {
            Setup(Vector3.zero);
        }

        void Update()
        {
        
        }

        public void Setup(Vector3 position)
        {
            this.position = position;
        }

        public void CreateFromItem(ItemBase item)
        {
            Hieroglyph[] hieroglyphs = item.Hieroglyphs;

            Vector2 originalParent = parent.GetComponent<RectTransform>().anchoredPosition;
            parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(originalParent.x, originalParent.y + ((yOffset * (hieroglyphs.Length - 1) / 2f)));

            hgList = new List<GameObject>();

            for (int i = 0; i < hieroglyphs.Length; i++)
            {
                for (int ii = 0; ii < hieroglyphs[i].sprites.Length; ii++)
                {
                    GameObject hgNew;
                    hgNew = Instantiate(hieroglyphPrefab, parent.transform);
                    hgNew.GetComponent<UIElementAnimations>().SetScale(0f);
                    hgNew.GetComponent<Image>().sprite = hieroglyphs[i].sprites[ii];
                    hgNew.GetComponent<Image>().enabled = false;
                    hgNew.GetComponent<RectTransform>().anchoredPosition = new Vector2 (position.x + (xOffset * ii), position.y + (-yOffset * i));
                    hgList.Add(hgNew);
                }
                
            }
            StartCoroutine(PopUpHieroglyphs(hgList));
        }

        private IEnumerator PopUpHieroglyphs(List<GameObject> list)
        {
            yield return new WaitForSeconds(popUpStartDelay);

            for (int i = 0; i < list.Count; i++)
            {
                list[i].GetComponent<Image>().enabled = true;
                list[i].GetComponent<UIElementAnimations>().SetScale(0f);
                list[i].gameObject.GetComponent<UIElementAnimations>().ScaleFromZero(popUpSpeed);
                yield return new WaitForSeconds(popUpDelay);
            }
        }

        public void CloseHieroglyphs()
        {
            StartCoroutine(Close(hgList));
        }

        private IEnumerator Close(List<GameObject> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].gameObject.GetComponent<UIElementAnimations>().ScaleToZero(closeSpeed);
                yield return new WaitForSeconds(closeDelay);
            }
        }
    }
}
