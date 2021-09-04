using System;

namespace SaveData
{
    [Serializable]
    public class GiftPikiminAndItemData
    {
        public bool[] wasGiven;

        public GiftPikiminAndItemData(GiftPikiminAndItemManager manager)
        {
            wasGiven = new bool[manager.wasGiven.Length];

            wasGiven = manager.wasGiven;
        }
    }
}
