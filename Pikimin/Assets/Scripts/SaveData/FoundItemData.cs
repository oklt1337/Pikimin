using System;
using Items;

namespace SaveData
{
    [Serializable]
    public class FoundItemData
    {
        public bool[] wasTaken;

        public FoundItemData(ItemManager item)
        {
            wasTaken = new bool[item.wasTaken.Length];

            wasTaken = item.wasTaken;
        }
    }
}
