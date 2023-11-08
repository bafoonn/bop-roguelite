using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class EventContext
    {
        public EventActionType EventType;
        public EventContext(EventActionType eventType)
        {
            EventType = eventType;
        }
    }
}
