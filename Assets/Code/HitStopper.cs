using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class HitStopper : Singleton<HitStopper>
    {
        public override bool DoPersist => true;
        private static Coroutine _waitRoutine;

        public static void Stop(float duration)
        {
            if (_waitRoutine != null) return;
            _waitRoutine = Current.StartCoroutine(WaitRoutine(duration));
        }

        private static IEnumerator WaitRoutine(float duration)
        {
            Time.timeScale = 0;
            yield return new WaitForSecondsRealtime(duration);
            _waitRoutine = null;
            Time.timeScale = 1;
        }
    }
}
