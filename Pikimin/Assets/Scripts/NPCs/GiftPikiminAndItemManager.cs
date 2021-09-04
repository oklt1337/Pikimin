using SaveData;
using UnityEngine;

public class GiftPikiminAndItemManager : MonoBehaviour
{
    public static GiftPikiminAndItemManager GiftPikiminAndItemManagerInstance;
    public bool[] wasGiven = new bool[11];


    private void Awake()
    {
        if (GiftPikiminAndItemManagerInstance != null)
        {
            Destroy(this);
        }
        else
        {
            GiftPikiminAndItemManagerInstance = this;
        }
    }

    private void Start()
    {
        LoadData.LoadDataInstance.OnLoad += LoadGiftPikiminAndItemManagerData;
    }


    private void LoadGiftPikiminAndItemManagerData()
    {
        //Read Data
        GiftPikiminAndItemData data = SaveManager.LoadGifts();

        wasGiven = data.wasGiven;
    }
}