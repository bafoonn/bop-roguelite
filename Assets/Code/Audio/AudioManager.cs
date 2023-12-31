using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace Pasta
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private AudioMixer mixer;
        [field: SerializeField] public AudioMixerGroup SFXMixerGroup { get; private set; }
        [field: SerializeField] public AudioMixerGroup MusicMixerGroup { get; private set; }

        public override bool PersistSceneLoad => true;

        private SFXPlayer sfxPlayerTemplate;
        private List<SFXPlayer> sfxPlayers = new();

        private SoundEffect[] soundEffects;
        private Music[] songs;

        [SerializeField] private bool usePlayerPrefs;

        protected override void Init()
        {
            base.Init();
            soundEffects = Resources.LoadAll<SoundEffect>("Audio/SoundEffects");
            songs = Resources.LoadAll<Music>("Audio/Music");
            sfxPlayerTemplate = GetComponentInChildren<SFXPlayer>();

            Debug.Assert(mixer != null, $"{name} has no AudioMixer set.");
            Debug.Assert(sfxPlayerTemplate != null, $"{name} couldn't find an sfxPlayer in children.");
            Debug.Assert(soundEffects != null, $"{name} couldn't load soundeffects");
            Debug.Assert(songs != null, $"{name} couldn't load music");
        }

        private void Start()
        {
            if (usePlayerPrefs)
            {
                float masterVol = PlayerPrefs.GetFloat(VolumeParameter.MasterVolume.ToString());
                float musicVol = PlayerPrefs.GetFloat(VolumeParameter.MusicVolume.ToString());
                float sfxVol = PlayerPrefs.GetFloat(VolumeParameter.SFXVolume.ToString());
                SetVolume(VolumeParameter.MasterVolume, masterVol);
                SetVolume(VolumeParameter.MusicVolume, masterVol * musicVol);
                SetVolume(VolumeParameter.SFXVolume, masterVol * sfxVol);
            }
        }

        /// <summary>
        /// Retreives a sound effect.
        /// </summary>
        /// <param name="effectName"></param>
        /// <param name="soundEffect"></param>
        /// <returns>True if sound effect found.</returns>
        public bool TryGetSoundEffect(string effectName, out SoundEffect soundEffect)
        {
            soundEffect = soundEffects.FirstOrDefault(sfx => sfx.Name.Equals(effectName.ToLower()));
            if (soundEffect == null)
                Debug.LogWarning($"AudioManager couldn't find a soundeffect with the name \"{effectName}\"");
            return soundEffect != null;
        }

        public void PlaySoundEffect(string effectName, float volume = 1f, bool addPitch = true, bool interrupt = true)
        {
            // Get soundeffect
            var soundEffect = soundEffects.FirstOrDefault(sfx => sfx.Name.Equals(effectName.ToLower()));

            // Check if there is a soundeffect to be played.
            if (soundEffect == null)
            {
                Debug.LogWarning($"{effectName} sound effect has no AudioClip.");
                return;
            }

            PlaySoundEffect(soundEffect, volume, addPitch, interrupt);
        }

        public void PlaySoundEffect(SoundEffect soundEffect, float volume = 1f, bool addPitch = true, bool interrupt = true)
        {
            volume = Mathf.Clamp01(volume);
            // Get player
            var player = sfxPlayers.FirstOrDefault(src => src.SoundEffect == soundEffect);

            // If no source is found, create a new one
            if (player == null)
            {
                player = Instantiate(sfxPlayerTemplate, transform);
                player.gameObject.name = soundEffect.Name;
                player.SoundEffect = soundEffect;
                sfxPlayers.Add(player);
            }

            player.AddPitch = addPitch;
            player.InterruptPlaying = interrupt;
            player.Volume = volume;
            player.Source.volume = volume;
            player.Play();
        }


        public static bool SetVolume(VolumeParameter param, float value)
        {
            value = Mathf.Clamp01(value);
            return Current.mixer.SetFloat(param.ToString(), value.FromLinearToDB());
        }

        public static bool TryGetVolume(VolumeParameter param, out float value)
        {
            if (Current.mixer.GetFloat(param.ToString(), out float volume))
            {
                value = volume;
                return true;
            }

            value = 0;
            return false;
        }

        public static float GetVolume(VolumeParameter param)
        {
            Current.mixer.GetFloat(param.ToString(), out float value);
            return value;
        }
    }
}
