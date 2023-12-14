using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Pasta
{
    public static class EditorExtensions
    {
        public static void SetPadding(this VisualElement element, int padding)
        {
            element.style.paddingBottom = padding;
            element.style.paddingLeft = padding;
            element.style.paddingRight = padding;
            element.style.paddingTop = padding;
        }

        public static void SetRadius(this VisualElement element, int radius)
        {
            element.style.borderBottomLeftRadius = radius;
            element.style.borderBottomRightRadius = radius;
            element.style.borderTopLeftRadius = radius;
            element.style.borderTopRightRadius = radius;
        }
    }
}
