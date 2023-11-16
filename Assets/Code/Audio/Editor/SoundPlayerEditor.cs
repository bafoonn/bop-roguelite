using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pasta
{
    [CustomEditor(typeof(SoundPlayer))]
    public class SoundPlayerEditor : Editor
    {
        private SoundPlayer soundPlayer;

        protected virtual void OnEnable()
        {
            soundPlayer = (SoundPlayer)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Play Sound"))
            {
                soundPlayer.Play();
            }

            if (GUILayout.Button("Start"))
            {
                soundPlayer.StartRepeat();
            }

            if (GUILayout.Button("Stop"))
            {
                soundPlayer.StopRepeat();
            }
        }
    }
}
