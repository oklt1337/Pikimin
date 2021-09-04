using System;
using Players;
using UnityEngine;

namespace SaveData
{
    [Serializable]
    public class PikiminInventoryData
    {
        public int[] pikimin;
        public int[] exp;
        public int[] currentHp;
        
        [Header("Ivs")]
        public int[] hp;
        public int[] atk;
        public int[] def;
        public int[] speed;
        
        [Header("Moves")]
        public byte[] pikimin0;
        public byte[] pikimin1;
        public byte[] pikimin2;
        public byte[] pikimin3;

        public PikiminInventoryData(PikiminInventory pikiminInventory)
        {
            var amount = pikiminInventory.pikiminAmount;
            pikimin = new int[amount];
            exp = new int[amount];
            currentHp = new int[amount];

            //Ivs
            hp = new int[amount];
            atk = new int[amount];
            def = new int[amount];
            speed = new int[amount];

            pikimin0 = new byte[4];
            pikimin1 = new byte[4];
            pikimin2 = new byte[4];
            pikimin3 = new byte[4];
            
            if (pikimin.Length > 0)
            {
                var newPikimin = pikiminInventory.OwnedPikimin;
                for (int i = 0; i < pikimin.Length; i++)
                {
                    if (newPikimin[i] != null)
                    {
                        pikimin[i] = (int) newPikimin[i].PikidexID;
                        exp[i] = newPikimin[i].Exp;
                        currentHp[i] = newPikimin[i].PikiminStats.CurrentHp;
                    
                        //Ivs
                        hp[i] = newPikimin[i].PikiminIVs.HpIV;
                        atk[i] = newPikimin[i].PikiminIVs.AtkIV;
                        def[i] = newPikimin[i].PikiminIVs.DefIV;
                        speed[i] = newPikimin[i].PikiminIVs.SpeedIV;

                        //Move pp
                        if (i == 0)
                        {
                            for (int j = 0; j < newPikimin[i].CurrentMoves.Length; j++)
                            {
                                if (newPikimin[i].CurrentMoves[j] != null)
                                {
                                    pikimin0[j] = newPikimin[i].CurrentMoves[j].CurrentPp;
                                }
                            }
                        }
                        else if (i == 1)
                        {
                            for (int j = 0; j < newPikimin[i].CurrentMoves.Length; j++)
                            {
                                if (newPikimin[i].CurrentMoves[j] != null)
                                {
                                    pikimin1[j] = newPikimin[i].CurrentMoves[j].CurrentPp;
                                }
                            }
                        }
                        else if (i == 2)
                        {
                            for (int j = 0; j < newPikimin[i].CurrentMoves.Length; j++)
                            {
                                if (newPikimin[i].CurrentMoves[j] != null)
                                {
                                    pikimin2[j] = newPikimin[i].CurrentMoves[j].CurrentPp;
                                }
                            }
                        }
                        else if (i == 3)
                        {
                            for (int j = 0; j < newPikimin[i].CurrentMoves.Length; j++)
                            {
                                if (newPikimin[i].CurrentMoves[j] != null)
                                {
                                    pikimin3[j] = newPikimin[i].CurrentMoves[j].CurrentPp;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
