using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Pasta
{
    public class SFXPlayer : SoundPlayer
    {
        protected override void Awake()
        {
            base.Awake();
            Source.outputAudioMixerGroup = AudioManager.Current.SFXMixerGroup;
        }
    }
}
