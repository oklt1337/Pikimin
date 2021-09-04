using UnityEngine;
using static Pikimins.Pikimin;
using static Players.PikiminInventory;

namespace Items
{
    public class HiddenMachine : Item
    {
        [SerializeField] protected PikiminType[] hmPikiminType;

        protected bool CheckIfHasPikiminOfType()
        {
            for (int i = 0; i < playerPikiminInventory.OwnedPikimin.Length; i++)
            {
                for (int j = 0; j < hmPikiminType.Length; j++)
                {
                    if (playerPikiminInventory.OwnedPikimin[i] != null)
                    {
                        if (playerPikiminInventory.OwnedPikimin[i].PikiType == hmPikiminType[j])
                        {
                            return true;
                        } 
                    }
                }
            }
            return false;
        }
    }
}
