using System;
using UnityEngine;

namespace SaveData
{
    [Serializable]
    public class ProfAleiData
    {
        public string takenPikimin;
        public bool tookPikimin;

        public ProfAleiData(AleiBehaviour prof)
        {
            takenPikimin = prof.TakenPikimin;
            tookPikimin = prof.gotHisFirstPikiminAlready;
        }
    }
}
