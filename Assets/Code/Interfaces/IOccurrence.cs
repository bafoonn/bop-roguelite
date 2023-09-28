using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public interface IOccurrence
    {
        public MonoBehaviour Target { get; }
        public OccurrenceType Type { get; }
    }
}
