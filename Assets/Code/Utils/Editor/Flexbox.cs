using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Pasta
{
    public class FlexBox : Box
    {
        public FlexBox() : base()
        {
            style.display = DisplayStyle.Flex;
        }

        public FlexBox(FlexDirection dir) : base()
        {
            style.display = DisplayStyle.Flex;
            style.flexDirection = dir;
        }
    }
}
