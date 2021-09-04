using System;
using Players;
using UnityEngine;

namespace SaveData
{
    [Serializable]
    public class ItemInventoryData
    {
        public string[] items;
        public int[] ownedCopies;
        public int money;

        public ItemInventoryData(ItemInventory itemInventory)
        {
            int amount = 0;
            for (int i = 0; i < itemInventory.OwnedItems.Length; i++)
            {
                if (itemInventory.OwnedItems[i] != null)
                {
                    amount++;
                }
            }
            items = new string[amount];
            ownedCopies = new int[amount];

            if (items.Length > 0)
            {
                for (int i = 0; i < amount; i++)
                {
                    items[i] = itemInventory.OwnedItems[i].ItemName;
                    ownedCopies[i] = itemInventory.OwnedItems[i].OwnedCopies;
                }
            }

            money = itemInventory.OwnedMoney;
        }
    }
}
