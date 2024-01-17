using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pasta
{
    public class LootUI : MonoBehaviour
    {
        [SerializeField] private Button _gridItemTemplate = null;
        public class GridItem
        {
            public Button Button = null;
            public ItemBase Item = null;
            private Image _image = null;
            private Text _amount = null;
            private LootUI _parent = null;

            public GridItem(LootUI loot, GameObject parent, Button template)
            {
                Button = Instantiate(template, parent.transform);
                Button.onClick.AddListener(Select);
                _amount = Button.GetComponentInChildren<Text>();
                _image = Button.GetComponentInChildren<Image>();
                _parent = loot;
            }

            public void SetItem(ItemBase item)
            {
                Item = item;
                Button.gameObject.name = Item.Name;
                var offset = new Vector2(Random.Range(-3, 3), Random.Range(-3, 3));
                _amount.text = Item.Amount.ToString();
                _amount.rectTransform.anchoredPosition = offset;
                _image.sprite = item.Sprite;
                _image.rectTransform.anchoredPosition = offset;
                _image.color = Color.gray;
                Button.gameObject.Activate();
            }

            public void Select()
            {
                _parent.SelectItem(this);
                _image.color = Color.white;
            }

            public void Deselect()
            {
                _image.color = Color.gray;
            }

            public void Clear()
            {
                Button.gameObject.Deactivate();
            }
        }

        private class ItemInfo
        {
            private Text[] _texts = null;
            public HieroglyphCreator Creator = null;

            public ItemInfo(GameObject gameObject)
            {
                _texts = gameObject.GetComponentsInChildren<Text>();
                foreach (var text in _texts)
                {
                    text.text = string.Empty;
                }
                Creator = gameObject.GetComponent<HieroglyphCreator>();
            }

            public void SetInfo(ItemBase item)
            {
                _texts[0].text = item.Name;
                _texts[1].text = item.Flavor;
                _texts[2].text = item.Description;
                Creator.CreateFromItem(item);
            }
        }

        private GridLayoutGroup _grid = null;
        private ItemInfo _info = null;
        private List<GridItem> _gridItems = new();
        private Loot _loot = null;
        private GridItem _selected = null;

        private void Awake()
        {
            _grid = GetComponentInChildren<GridLayoutGroup>();
            _info = new ItemInfo(GetComponentInChildren<VerticalLayoutGroup>().gameObject);
            _gridItemTemplate.gameObject.Deactivate();
        }

        private void Start()
        {
            Setup(Player.Current.Loot);
        }

        public void Setup(Loot loot)
        {
            _loot = loot;
            CreateLayout();
        }

        private void OnEnable()
        {
            CreateLayout();
        }

        private void OnDisable()
        {
            foreach (var item in _gridItems)
            {
                item.Clear();
            }
        }
        private void CreateLayout()
        {
            if (_loot == null) return;
            List<ItemBase> distinctItems = new();
            foreach (var item in _loot)
            {
                if (distinctItems.Contains(item as ItemBase))
                {
                    continue;
                }
                distinctItems.Add(item as ItemBase);
            }

            while (distinctItems.Count > _gridItems.Count)
            {
                _gridItems.Add(new GridItem(this, _grid.gameObject, _gridItemTemplate));
            }

            for (int i = 0; i < _gridItems.Count; i++)
            {
                _gridItems[i].SetItem(distinctItems[i]);
            }

            if (_gridItems.Count > 0) _gridItems[0].Select();
        }

        public void SelectItem(GridItem gridItem)
        {
            if (_selected == gridItem) return;
            if (_selected != null)
            {
                _info.Creator.CloseHieroglyphs();
                _selected.Deselect();
            }
            _info.SetInfo(gridItem.Item);
            _selected = gridItem;
        }
    }
}
