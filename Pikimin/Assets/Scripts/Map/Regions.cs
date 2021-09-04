using UnityEngine;

namespace Map
{
    public class Regions : MonoBehaviour
    {
        public static Regions RegionsInstance;

        private void Awake()
        {
            RegionsInstance = this;
        }

        public enum BiomeList
        {
            Grass,
            Forest,
            Rock,
            Water,
            Special,
            Npc,
        }

        public enum PikiminRegions
        {
            Altair = 0,
            Deneb = 1,
            Unukal = 2,
            Vega = 3,
            Triverr = 4,
            Maeus = 5,
            Route1 = 6,
            Route2 = 7,
            Route3 = 8,
            Route4 = 9,
            Route5 = 10,
            Route6 = 11,
            Route7 = 12,
            Route8 = 13,
            Route9 = 14,
            Hai = 15,
            Ptole = 16,
            Delteros = 17,
            Alsham = 18,
            Reha = 19,
            ProfAlei = 20,
            None = 21,
            Npc = 22,
        }
        
        public void PikiminLvlAtRegion(in PikiminRegions regions, out int minLvl, out int maxLvl)
        {
            switch (regions)
            {
                case PikiminRegions.Route1:
                    minLvl = 3;
                    maxLvl = 6;
                    return;
                case PikiminRegions.Route2:
                    minLvl = 5;
                    maxLvl = 9;
                    return;
                case PikiminRegions.Route3:
                    minLvl = 12;
                    maxLvl = 15;
                    return;
                case PikiminRegions.Route4:
                    minLvl = 17;
                    maxLvl = 23;
                    return;
                case PikiminRegions.Route5:
                    minLvl = 29;
                    maxLvl = 33;
                    return;
                case PikiminRegions.Route6:
                    minLvl = 35;
                    maxLvl = 41;
                    return;
                case PikiminRegions.Route7:
                    minLvl = 41;
                    maxLvl = 43;
                    return;
                case PikiminRegions.Route8:
                    minLvl = 44;
                    maxLvl = 47;
                    return;
                case PikiminRegions.Route9:
                    minLvl = 46;
                    maxLvl = 49;
                    return;
                case PikiminRegions.Hai:
                    minLvl = 10;
                    maxLvl = 14;
                    return;
                case PikiminRegions.Ptole:
                    minLvl = 50;
                    maxLvl = 55;
                    return;
                case PikiminRegions.Delteros:
                    minLvl = 24;
                    maxLvl = 28;
                    return;
            }
            minLvl = 0;
            maxLvl = 0;
        }
    }
}
