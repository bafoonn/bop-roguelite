using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    [CreateAssetMenu(menuName = "Items/EventItem", fileName = "New EventItem")]
    public class EventItem : ItemBase
    {
        public OccurrenceType EventType;

        public override bool CanStack => false;

        public override void Loot()
        {
            OccurrenceHandler.OnOccurrence += Trigger;
        }

        public override void Drop()
        {
            OccurrenceHandler.OnOccurrence -= Trigger;
        }

        private void Trigger(IOccurrence obj)
        {
            if (obj.Type != EventType)
            {
                return;
            }


        }
    }
}
