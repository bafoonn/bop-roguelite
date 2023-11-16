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
            if (_waitRoutine != null)
            {
                Current.StopCoroutine(_waitRoutine);
            }
            _waitRoutine = Current.StartCoroutine(WaitRoutine(duration));
        }

        private static IEnumerator WaitRoutine(float duration)
        {
            float timer = 0f;
            Time.timeScale = 0f;
            while (timer < duration)
            {
                if (GameManager.Current.IsPaused) yield return null;
                timer += Time.unscaledDeltaTime;
                //float t = timer / duration;
                //Time.timeScale = Mathf.Lerp(0, 1, t);
                yield return null;
            }
            _waitRoutine = null;
            Time.timeScale = 1;
        }
    }
}
