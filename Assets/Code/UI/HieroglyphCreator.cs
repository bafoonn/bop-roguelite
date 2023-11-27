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

        [SerializeField] private float xOffset = 18f;
        [SerializeField] private float yOffset = 22f;
        

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

            foreach(Hieroglyph hg in hieroglyphs)
            {
                for (int i = 0; i < hg.sprites.Length; i++)
                {
                    GameObject hgNew;
                    hgNew = Instantiate(hieroglyphPrefab, parent.transform);
                    hgNew.GetComponent<Image>().sprite = hg.sprites[i];
                    hgNew.GetComponent<RectTransform>().anchoredPosition = new Vector2 (position.x + (xOffset * i), position.y);
                }
                
            }
        }
    }
}
