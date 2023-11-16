using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Pasta
{
    public class SoundPlayer : MonoBehaviour
    {
        [Header("Player Fields")]
        public Sound SoundEffect;
        public AudioSource Source;
        [Range(0.0f, 1.0f)]
        public float Volume = 1.0f;
        //public float Distance = 1.0f;
        public bool InterruptPlaying = true;
        public bool AddPitch = false;
        public bool LoopSource = false;
        [Header("Repeater Fields")]
        public float Interval = 5;
        public float AddedIntervalRandomness = 0;
        private bool doPlay = false;
        private Coroutine repeatRoutine;


        protected virtual void Awake()
        {
            if (Source == null)
            {
                Source = this.AddOrGetComponent<AudioSource>();
            }
            Source.volume = Volume;
            Source.playOnAwake = false;
            Source.loop = LoopSource;
            //Source.minDistance = Distance;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Source)
            {
                Source.volume = Volume;
                Source.playOnAwake = false;
                //Source.minDistance = Distance;
            }

            if (AddedIntervalRandomness < 0)
                AddedIntervalRandomness = 0;

            if (Interval < 0)
                Interval = 0;
        }
#endif

        /// <summary>
        /// Plays a random sound clip from assigned sound effect.
        /// </summary>
        public void Play()
        {
            if (!Application.isPlaying)
            {
                Debug.LogWarning("Sounds can only be played in play mode!");
                return;
            }

            if (SoundEffect == null || gameObject.activeInHierarchy == false)
                return;

            if (!InterruptPlaying && Source.isPlaying)
                return;

            if (Source.isPlaying)
                Source.Stop();

            Source.clip = SoundEffect.RandomClip;

            if (AddPitch)
                Source.pitch = 1 + Random.Range(0, 0.2f);

            Source.Play();
        }

        public void Stop() => Source.Stop();

        public void StartRepeat()
        {
            if (doPlay || !Application.isPlaying) return;
            doPlay = true;
            repeatRoutine = StartCoroutine(PlayRepeat());
        }

        public void StopRepeat(bool stopPlayer = false)
        {
            doPlay = false;

            if (repeatRoutine != null)
            {
                StopCoroutine(repeatRoutine);
                repeatRoutine = null;
            }

            if (stopPlayer)
            {
                Source.Stop();
            }
        }

        private IEnumerator PlayRepeat()
        {
            while (doPlay)
            {
                float interval = Interval + Random.Range(0, AddedIntervalRandomness);
                yield return new WaitForSeconds(interval);
                Play();
            }
        }
    }
}
