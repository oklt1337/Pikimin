using System;
using Pikimins;
using UI;
using UnityEngine;

namespace SaveData
{
    [Serializable]
    public class PikidexData
    {
        public bool[] hasBeenSeen;
        public bool[] hasBeenCaught;

        public PikidexData(PikidexBehaviour pikidex)
        {
            hasBeenSeen = new bool[pikidex.HasBeenSeen.Length];
            hasBeenCaught = new bool[pikidex.HasBeenCaught.Length];
            hasBeenSeen = pikidex.HasBeenSeen;
            hasBeenCaught = pikidex.HasBeenCaught;
        }
    }
}
