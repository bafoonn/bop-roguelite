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
            var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
            rootVisualElement.Add(splitView);

            var items = LoadItems();
            var leftPanel = new ListView(items);
            leftPanel.makeItem = () => new Label();
            leftPanel.bindItem = (element, i) => (element as Label).text = items[i].name;
            leftPanel.selectionChanged += OnSelect;

            splitView.Add(leftPanel);
            _rightPanel = new VisualElement();
            splitView.Add(_rightPanel);
        }

        private void OnSelect(IEnumerable<object> selectedItems)
        {
            _rightPanel.Clear();

            var item = selectedItems.First() as ItemBase;
            if (item == null)
            {
                return;
            }

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
