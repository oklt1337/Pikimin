using SaveData;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
    public static NpcManager NpcManagerInstance;
    public bool[] hasBeenBeaten = new bool[40];

    public delegate void NpcSetBool();
    public NpcSetBool OnLoadNpcManager;

    private void Awake()
    {
        NpcManagerInstance = this;
    }

    private void Start()
    {
        LoadData.LoadDataInstance.OnLoad += LoadTrainerData;
    }


    private void LoadTrainerData()
    {
        //Read Trainerdata
        TrainerData data = SaveManager.LoadTrainer();
            
        hasBeenBeaten = data.hasBeenBeaten;
        
        OnLoadNpcManager?.Invoke();
    }
}