using SaveData;
using UnityEngine;

namespace Items
{
    public class ItemManager : MonoBehaviour
    {
        public static ItemManager ItemManagerInstance;
        public bool[] wasTaken = new bool[14];

        public delegate void OnLoad();

        public OnLoad OnLegendaryLoad;


        private void Awake()
        {
            if (ItemManagerInstance != null)
            {
                Destroy(this);
            }
            else
            {
                ItemManagerInstance = this;
            }
        }

        private void Start()
        {
            LoadData.LoadDataInstance.OnLoad += LoadFoundItemData;
        }


        private void LoadFoundItemData()
        {
            //Read Data
            FoundItemData data = SaveManager.LoadFoundItem();
            
            wasTaken = data.wasTaken;
            OnLegendaryLoad?.Invoke();
        }
    }
}
