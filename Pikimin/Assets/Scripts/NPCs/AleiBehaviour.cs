using Battle;
using Pikimins;
using Players;
using SaveData;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class AleiBehaviour : NpcBehaviour
{
    public static AleiBehaviour Alei;

    [Header("Alei Stuff")]
    [SerializeField] private Tilemap barrier;
    [SerializeField] private string takenPikimin;
    [SerializeField] private string[] afterFirstPikiminDialogue;
    [SerializeField] private string[] afterCompletionDialogue;
    [SerializeField] private GameObject garryManager;
    [SerializeField] private GameObject[] truthsEntries;

    [Header("Starters")]
    [SerializeField] private Pikimin[] startersPrefabs;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image pikimin;
    [SerializeField] private GameObject textBox;
    [SerializeField] private byte currentPosition;
    [SerializeField] private bool isChoosingStarter;
    [SerializeField] private bool tempBool;

    public bool gotHisFirstPikiminAlready;

    public string TakenPikimin
    {
        get => takenPikimin;
        private set => takenPikimin = value;
    }

    private void Awake()
    {
        Alei = this;
    }

    private void Start()
    {
        GameManager.Instance.OnSave += SaveProfAleiData;
        LoadData.LoadDataInstance.OnLoad += LoadProfAleiData;
    }

    private void Update()
    {
        if (gotHisFirstPikiminAlready && !tempBool)
        {
            SaidDialogue = afterFirstPikiminDialogue;
            isChoosingStarter = false;
            // Turns of the barrier, don't ask me why it works, just accept it.
            barrier.SetTile(new Vector3Int(-2, 1, 0), null);
            barrier.SetTile(new Vector3Int(-1, 1, 0), null);
            barrier.SetTile(new Vector3Int(0, 1, 0), null);
            barrier.SetTile(new Vector3Int(1, 1, 0), null);

            garryManager.SetActive(true);
            tempBool = true;
        }

        if (isChoosingStarter && DialogueManager.Dialogue.chattingHasEnded)
        {
            Player.CurrentPlayer.ChangeState(Players.PlayerState.Interact);
            textBox.SetActive(true);
            TextManager();
            Inputs();
        }

        if(TrainerPass.TrainerPassInstance.CaughtDifferentPikiminAmount == BattleManager.BattleManagerInstance.AllPikimin.Count)
        {
            for (int i = 0; i < truthsEntries.Length; i++)
            {
                truthsEntries[i].SetActive(true);
            }
        }
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        TrainerPass.TrainerPassInstance.UpdateInfo();
        if(TrainerPass.TrainerPassInstance.CaughtDifferentPikiminAmount == BattleManager.BattleManagerInstance.AllPikimin.Count)
        {
            SaidDialogue = afterCompletionDialogue;
        }
        base.OnTriggerEnter2D(collision);
        if(takenPikimin == "")
        {
            isChoosingStarter = true;
        }
    }

    /// <summary>
    /// Updates all the Profs values after the Player takes their first Pikimin.
    /// </summary>
    private void UpdateInfo()
    {
        BattleManager.BattleManagerInstance.CreatePlayersFirstPikimin(startersPrefabs[currentPosition], 5);
        isChoosingStarter = false;
        textBox.SetActive(false);
        TakenPikimin = PikiminInventory.playerPikiminInventory.OwnedPikimin[0].PikiminName;
        SaidDialogue = afterFirstPikiminDialogue;
        Player.CurrentPlayer.ChangeState(Players.PlayerState.Idle);

        // Turns of the barrier, don't ask me why it works, just accept it.
        barrier.SetTile(new Vector3Int(-2,1,0), null);
        barrier.SetTile(new Vector3Int(-1,1,0), null);
        barrier.SetTile(new Vector3Int(0,1,0), null);
        barrier.SetTile(new Vector3Int(1,1,0), null);

        garryManager.SetActive(true);
        gotHisFirstPikiminAlready = true;
    }

    /// <summary>
    /// The Players Inputs.
    /// </summary>
    private void Inputs()
    {
        if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && currentPosition < 2) 
        {
            currentPosition++;
        }

        if((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && currentPosition > 0)
        {
            currentPosition--;
        }

        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E))
        {
            UpdateInfo();
        }
    }

    // Manages the Text.
    private void TextManager()
    {
        if(currentPosition == 0)
        {
            text.text = "\n > Bulbasaur \n\n Charmander \n\n Squirtle";
            pikimin.sprite = BattleManager.BattleManagerInstance.AllPikimin[0].FrontSprite;
        }
        else if(currentPosition == 1)
        {
            text.text = "\n Bulbasaur \n\n > Charmander \n\n Squirtle";
            pikimin.sprite = BattleManager.BattleManagerInstance.AllPikimin[3].FrontSprite;
        }
        else if(currentPosition == 2)
        {
            text.text = "\n Bulbasaur \n\n Charmander \n\n > Squirtle";
            pikimin.sprite = BattleManager.BattleManagerInstance.AllPikimin[6].FrontSprite;
        }
    }

    private void LoadProfAleiData()
    {
        ProfAleiData data = SaveManager.LoadProfAlei();

        takenPikimin = data.takenPikimin;
        gotHisFirstPikiminAlready = data.tookPikimin;
    }

    private void SaveProfAleiData()
    {
        SaveManager.SaveProfAlei(this);
    }
}