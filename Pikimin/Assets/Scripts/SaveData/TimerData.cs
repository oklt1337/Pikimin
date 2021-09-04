using System;
using System.Diagnostics.Contracts;
using Camera;
using UnityEngine;

namespace SaveData
{
    [Serializable]
    public class TimerData
    {
        public float time;

        public TimerData(Timer timer)
        {
            time = timer.Playtime;
        }
    }
}
