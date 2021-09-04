using System;
using UnityEngine;

namespace SaveData
{
    [Serializable]
    public class TrainerData
    {
        public bool[] hasBeenBeaten;

        public TrainerData(NpcManager npc)
        {
            hasBeenBeaten = new bool[npc.hasBeenBeaten.Length];
            hasBeenBeaten = npc.hasBeenBeaten;
        }
    }
}
