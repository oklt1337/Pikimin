using System;
using UI;
using UnityEngine;

namespace SaveData
{
    [Serializable]
    public class PikiminBoxData
    {
        public int[] pikimin;
        public int[] exp;
        public int pikiminBoxAmount;
        
        [Header("Ivs")]
        public int[] hp;
        public int[] atk;
        public int[] def;
        public int[] speed;

        public PikiminBoxData(PikiminBox pikiminBox)
        {
            pikiminBoxAmount = pikiminBox.BoxPikiminAmount;
            if (pikiminBoxAmount > 0)
            {
                var amount = pikiminBox.BoxPikimin1.Length;
                var newPikimin = pikiminBox.BoxPikimin1;
                
                pikimin = new int[amount];
                exp = new int[amount];

                //Ivs
                hp = new int[amount];
                atk = new int[amount];
                def = new int[amount];
                speed = new int[amount];
            
            
                if (newPikimin.Length > 0)
                {
                    for (int i = 0; i < newPikimin.Length; i++)
                    {
                        if (newPikimin[i] != null)
                        {
                            pikimin[i] = (int) newPikimin[i].PikidexID - 1;
                            exp[i] = newPikimin[i].Exp;

                            //Ivs
                            hp[i] = newPikimin[i].PikiminIVs.HpIV;
                            atk[i] = newPikimin[i].PikiminIVs.AtkIV;
                            def[i] = newPikimin[i].PikiminIVs.DefIV;
                            speed[i] = newPikimin[i].PikiminIVs.SpeedIV;
                        }
                        else
                        {
                            pikimin[i] = 999;
                        }
                    }
                }
            }
        }
    }
}
