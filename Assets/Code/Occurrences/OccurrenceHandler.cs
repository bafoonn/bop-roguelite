using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public static class OccurrenceHandler
    {
        public static event Action<IOccurrence> OnOccurrence;
        public static bool TriggerEvent(IOccurrence occurrence)
        {
            if (OnOccurrence != null)
            {
                OnOccurrence(occurrence);
                return true;
            }

            return false;
        }
    }
}
