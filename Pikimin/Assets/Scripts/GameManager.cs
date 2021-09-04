using System.Collections;
using Audio;
using Camera;
using Items;
using Players;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using static SaveData.SaveManager;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public delegate void SaveGame();
    public SaveGame OnSave;

    public bool continueGame;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Start()
    {
        OnSave += SaveData;
        Screen.SetResolution(1024, 768, true);
    }

    private void SaveData()
    {
        SavePlayer(Player.CurrentPlayer);
        SavePlayerController(PlayerController.CurrentPlayerController);
        SavePikiminInventory(PikiminInventory.playerPikiminInventory);
        SaveItemInventory(ItemInventory.playerItems);
        SavePikiminBox(PikiminBox.PikiBox);
        SavePikidex(PikidexBehaviour.PikidexInstance);
        SaveFoundItem(ItemManager.ItemManagerInstance);
        SaveGifts(GiftPikiminAndItemManager.GiftPikiminAndItemManagerInstance);
        SaveAudioClip(AudioManager.AudioManagerInstance);
        SaveTrainer(NpcManager.NpcManagerInstance);
        SaveTime(Timer.TimerInstance);
    }
}
