using System;
using Audio;
using UnityEngine;

namespace SaveData
{
    [Serializable]
    public class AudioData
    {
        public string backgroundSound;

        public AudioData(AudioManager audioManager)
        {
            if (audioManager.BackgroundSound.clip != null)
            {
                backgroundSound = audioManager.BackgroundSound.clip.name;
            }
        }
    }
}
