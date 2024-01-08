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

        private Vector2 originalParent;
        private Vector3 position;

        [SerializeField] private float popUpStartDelay = 0.4f;
        [SerializeField] private float popUpDelay = 0.15f;
        [SerializeField] private float popUpSpeed = 6f;

        [SerializeField] private float closeDelay = 0.15f;
        [SerializeField] private float closeSpeed = 6f;

        [SerializeField] private float xOffset = 18f;
        [SerializeField] private float yOffset = 22f;

        private List<GameObject> hgList = new();
        public float TotalCloseDelay => closeDelay * hgList.Count + (1f / closeSpeed);

        void Awake()
        {
            Setup(Vector3.zero);
        }

        public void Setup(Vector3 position)
        {
            this.position = position;
            originalParent = parent.GetComponent<RectTransform>().anchoredPosition;
            Debug.Log(originalParent.x + ", " + originalParent.y);
        }

        public void CreateFromItem(ItemBase item)
        {
            if (item.Hieroglyphs == null) return;
            Hieroglyph[] hieroglyphs = item.Hieroglyphs;

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
                    hgNew.GetComponent<RectTransform>().anchoredPosition = new Vector2(position.x + (xOffset * ii), position.y + (-yOffset * i));
                    hgList.Add(hgNew);
                }

            }
            StartCoroutine(PopUpHieroglyphs(hgList));
        }

        private IEnumerator PopUpHieroglyphs(List<GameObject> list)
        {
            yield return new WaitForSecondsRealtime(popUpStartDelay);

            for (int i = 0; i < list.Count; i++)
            {
                list[i].GetComponent<Image>().enabled = true;
                list[i].GetComponent<UIElementAnimations>().SetScale(0f);
                list[i].gameObject.GetComponent<UIElementAnimations>().ScaleFromZero(popUpSpeed);
                yield return new WaitForSecondsRealtime(popUpDelay);
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
                yield return new WaitForSecondsRealtime(closeDelay);
            }
            while (list.Count > 0)
            {
                Destroy(list[0]);
                list.RemoveAt(0);
            }
        }
    }
}
