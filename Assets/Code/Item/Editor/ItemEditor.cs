using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Properties;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Pasta
{
    public class ItemEditor : EditorWindow
    {
        private const string PATH = "Assets/Resources/Items/";
        private const int LIST_FONT_SIZE = 16;
        private const int LIST_IMAGE_DIMENSIONS = 48;

        private ListView _leftPanel;
        private VisualElement _rightPanel;

        [MenuItem("Tools/Item Editor")]
        public static void OpenWindow()
        {
            EditorWindow window = GetWindow<ItemEditor>();
            window.titleContent = new GUIContent("Item Editor");
        }

        private ItemBase[] LoadItems()
        {
            return Resources.LoadAll<ItemBase>("Items");
        }

        private void CreateGUI()
        {
            Draw();
        }

        private void Draw()
        {
            rootVisualElement.Clear();
            var header = new VisualElement();

            var inputField = new TextField();
            inputField.style.minWidth = 150;
            var addButton = new Button(() =>
            {
                CreateItem(inputField.text);
                inputField.SetValueWithoutNotify("");
            })
            { text = "Add" };
            var itemAdder = new Box();
            itemAdder.Add(inputField);
            itemAdder.Add(addButton);
            itemAdder.style.display = DisplayStyle.Flex;
            itemAdder.style.flexDirection = FlexDirection.Row;

            var deleteButton = new Button(DeleteSelected) { text = "Delete Selected" };
            var refreshButton = new Button(() =>
            {
                int selected = _leftPanel.selectedIndex;
                Draw();
                _leftPanel.SetSelection(selected);
            })
            { text = "Refresh" };
            var buttons = new Box();
            buttons.Add(deleteButton);
            buttons.Add(refreshButton);
            buttons.style.display = DisplayStyle.Flex;
            buttons.style.flexDirection = FlexDirection.Row;
            buttons.style.paddingBottom = 5;

            header.Add(itemAdder);
            header.Add(buttons);
            rootVisualElement.Add(header);

            var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
            rootVisualElement.Add(splitView);

            var items = LoadItems();
            _leftPanel = new ListView(items)
            {
                fixedItemHeight = LIST_IMAGE_DIMENSIONS,
                makeItem = () =>
                {
                    var box = new Box();
                    var image = new Image();
                    image.style.width = LIST_IMAGE_DIMENSIONS;
                    image.style.height = LIST_IMAGE_DIMENSIONS;
                    box.Add(image);

                    var label = new Label();
                    label.style.unityFontStyleAndWeight = FontStyle.Bold;
                    label.style.fontSize = LIST_FONT_SIZE;
                    label.style.alignSelf = Align.Center;
                    label.style.paddingLeft = 5;
                    box.Add(label);
                    box.style.flexDirection = FlexDirection.Row;

                    return box;
                },
                bindItem = (element, i) =>
                {
                    var box = element as Box;
                    box.Q<Label>().text = items[i].Name;
                    box.Q<Image>().sprite = items[i].Sprite;
                },
            };
            _leftPanel.selectionChanged += (items) =>
            {
                _rightPanel.Clear();
                var item = items.First() as ItemBase;
                if (item == null)
                {
                    return;
                }
                SetSelected(item);
            };

            splitView.Add(_leftPanel);
            _rightPanel = new ScrollView();
            splitView.Add(_rightPanel);
        }

        private void CreateItem(string name)
        {
            if (name == null || name.Length == 0) return;
            var newItem = CreateInstance<ItemBase>();
            newItem.Name = name;
            var path = PATH + name + ".asset";
            AssetDatabase.CreateAsset(newItem, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Draw();
            var items = LoadItems();
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].name == name)
                {
                    _leftPanel.SetSelection(i);
                    break;
                }
            }
        }

        private void SetSelected(ItemBase item)
        {
            var serializedItem = new SerializedObject(item);
            var itemProp = serializedItem.GetIterator();
            itemProp.Next(true);

            while (itemProp.NextVisible(false))
            {
                var prop = new PropertyField(itemProp);
                prop.SetEnabled(true);
                prop.Bind(serializedItem);
                _rightPanel.Add(prop);
            }
        }

        public void DeleteSelected()
        {
            var item = LoadItems()[_leftPanel.selectedIndex];
            if (!EditorUtility.DisplayDialog("Delete item", "Are you sure you want to delete " + item.Name + "?", "Yes", "No")) return;
            var path = PATH + item.name + ".asset";
            AssetDatabase.DeleteAsset(path);
            Draw();
        }
    }
}
