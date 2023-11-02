using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Pasta
{
    public class SoundManager : MonoBehaviour
    {
        public AudioMixer audioMixer;
        private void Start()
        {
            if (PlayerPrefs.HasKey("MasterVol"))
            {
                audioMixer.SetFloat("MasterVol", PlayerPrefs.GetFloat("MasterVol"));
            }
            if (PlayerPrefs.HasKey("MusicVol"))
            {
                audioMixer.SetFloat("MusicVol", PlayerPrefs.GetFloat("MusicVol"));
            }
            if (PlayerPrefs.HasKey("SFXVol"))
            {
                audioMixer.SetFloat("SFXVol", PlayerPrefs.GetFloat("SFXVol"));
            }
        }






    }
}
