using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Pasta
{
    public class ItemEditor : EditorWindow
    {
        private const string PATH = "Assets/Resources/Items/";
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
            var addButton = new Button(() =>
            {
                CreateItem(inputField.text);
                inputField.SetValueWithoutNotify("");
            });
            addButton.text = "Add";
            var deleteButton = new Button(() =>
            {
                var items = LoadItems();
                var item = items[_leftPanel.selectedIndex];
                var path = PATH + item.name + ".asset";
                AssetDatabase.DeleteAsset(path);
                Draw();
            });
            deleteButton.text = "Delete Selected";
            var refreshButton = new Button(() =>
            {
                Draw();
            });
            refreshButton.text = "Refresh";

            header.Add(inputField);
            header.Add(addButton);
            header.Add(deleteButton);
            header.Add(refreshButton);
            rootVisualElement.Add(header);

            var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
            rootVisualElement.Add(splitView);

            var items = LoadItems();
            _leftPanel = new ListView(items);
            _leftPanel.makeItem = () => new Label();
            _leftPanel.bindItem = (element, i) => (element as Label).text = items[i].name;
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
    }
}
