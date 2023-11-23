using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Pasta
{
    public class SoundPlayer : MonoBehaviour
    {
        public Sound SoundEffect;
        public AudioSource Source;
        [Range(0.0f, 1.0f)]
        public float Volume = 1.0f;
        //public float Distance = 1.0f;
        [Tooltip("Stops previous playback if Play is called during it.")]
        public bool InterruptPlaying = true;
        [Header("Pitch")]
        public bool AddPitch = false;
        [Range(-3f, 3f)] public float MinPitch = 1f;
        [Range(-3f, 3f)] public float MaxPitch = 1.02f;
        [Header("Loop")]
        //public bool LoopSource = false;
        public float MinInterval = 5;
        public float MaxInterval = 0;
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
            //Source.loop = LoopSource;
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

            if (MaxPitch < MinPitch)
                MaxPitch = MinPitch;

            if (MaxInterval < MinInterval)
                MaxInterval = MinInterval;

            if (MinInterval < 0)
                MinInterval = 0;
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
                Source.pitch = Random.Range(MinPitch, MaxPitch);

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
                float interval = Random.Range(MinInterval, MaxInterval);
                yield return new WaitForSeconds(interval);
                Play();
            }
        }
    }
}
