using System;
using UI;
using UnityEngine;

namespace SaveData
{
    [Serializable]
    public class SettingsData
    {
        public float volume;

        public SettingsData(Settings settings)
        { 
            volume = settings.Volume.value;
        }
    }
}
