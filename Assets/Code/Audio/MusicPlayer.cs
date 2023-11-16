using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pasta
{
    public class MusicPlayer : SoundPlayer
    {
        [Header("Music Fields")]
        public float fadeTime = 1.0f;
        private Coroutine musicRoutine = null;

        protected override void Awake()
        {
            base.Awake();
            Source.outputAudioMixerGroup = AudioManager.Current.MusicMixerGroup;
        }

        public void FadeIn()
        {
            if (musicRoutine != null)
            {
                StopCoroutine(musicRoutine);
            }

            musicRoutine = StartCoroutine(FadeInRoutine(Source.volume));
        }

        public void FadeOut()
        {
            if (musicRoutine != null)
            {
                StopCoroutine(musicRoutine);
            }

            musicRoutine = StartCoroutine(FadeOutRoutine(Source.volume));
        }

        private IEnumerator FadeInRoutine(float volume)
        {
            Play();
            float timer = fadeTime * volume;
            float t;
            while (timer < 1)
            {
                t = timer / fadeTime;
                Source.volume = t;
                timer += Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator FadeOutRoutine(float volume)
        {
            float timer = fadeTime * volume;
            float t;
            while (timer > 0)
            {
                t = timer / fadeTime;
                Source.volume = t;
                timer -= Time.deltaTime;
                yield return null;
            }
            Stop();
        }

    }
}
