using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public struct Occurrence : IOccurrence
    {
        private MonoBehaviour _target;
        private OccurrenceType _type;

        public MonoBehaviour Target => _target;
        public OccurrenceType Type => _type;

        public Occurrence(MonoBehaviour target, OccurrenceType type)
        {
            _target = target;
            _type = type;
        }
    }
}
