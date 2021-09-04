using UnityEngine;
using Pikimins;
using SaveData;
using TMPro;
using static Players.Player;
using UnityEngine.UI;
using static Battle.BattleManager;
using static Audio.AudioManager;

namespace UI
{
    public class PikidexBehaviour : MonoBehaviour
    {
        public static PikidexBehaviour PikidexInstance;

        [Header ("Pikimin Info")]
        [SerializeField] private Pikimin[] pikimin;
        [SerializeField] private bool[] hasBeenSeen;
        [SerializeField] private bool[] hasBeenCaught;

        [Header("Menuing")]
        [SerializeField] private GameObject menuBox;
        [SerializeField] private TextMeshProUGUI pikiminNamesList;
        [SerializeField] private Image image;
        [SerializeField] private byte currentPosition;
        [SerializeField] private byte lowerBenchmark;
        [SerializeField] private byte upperBenchmark;
        [SerializeField] private bool didInput;
        [SerializeField] private float waitTillNextInput;

        [Header("Highlighted Pikimin UI")]
        [SerializeField] private Image pikiImage;
        [SerializeField] private Image pikiBackImage;
        [SerializeField] private Image pikiFootprintImage;
        [SerializeField] private TextMeshProUGUI pikiBodyInfo;
        [SerializeField] private TextMeshProUGUI pikiDescription;
        [SerializeField] private TextMeshProUGUI pikiType;

        public bool[] HasBeenSeen => hasBeenSeen;

        public bool[] HasBeenCaught => hasBeenCaught;

        private void Awake()
        {
            PikidexInstance = this;
        }

        private void Start()
        {
            LoadData.LoadDataInstance.OnLoad += LoadPikidexData;
            pikimin = BattleManagerInstance.AllPikimin.ToArray();
            image.enabled = true;
            gameObject.SetActive(false);
            pikiminNamesList.gameObject.SetActive(true);
            pikiImage.gameObject.SetActive(true);
            pikiBodyInfo.gameObject.SetActive(true);
            pikiDescription.gameObject.SetActive(true);
            pikiType.gameObject.SetActive(true);
            pikiFootprintImage.gameObject.SetActive(true);
            pikiBackImage.gameObject.SetActive(true);
        }

        void Update()
        {
            if (didInput)
            {
                waitTillNextInput -= Time.deltaTime;
                if(waitTillNextInput < 0)
                {
                    didInput = false;
                }
            }
            else
            {
                Inputs();
            }
        }

        /// <summary>
        /// The Players inputs.
        /// </summary>
        private void Inputs()
        {
            // UP and DOWN
            if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && (pikimin.Length-1) > currentPosition)
            {
                AudioManagerInstance.Sfx.PlayOneShot(AudioManagerInstance.ButtonOverSounds[0]);
                waitTillNextInput = 0.1f;
                didInput = true;
                currentPosition++;
                if (currentPosition > upperBenchmark)
                {
                    upperBenchmark++;
                    lowerBenchmark++;
                }
                
                TextManager();
                HightlightedPikiminUpdater();
            }
            else if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && 0 < currentPosition)
            {
                AudioManagerInstance.Sfx.PlayOneShot(AudioManagerInstance.ButtonOverSounds[0]);
                waitTillNextInput = 0.1f;
                didInput = true;
                currentPosition--;
                if (currentPosition < lowerBenchmark)
                {
                    upperBenchmark--;
                    lowerBenchmark--;
                }
                
                TextManager();
                HightlightedPikiminUpdater();
            }

            // Closes Pikidex and open Menu again.
            if(Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Backspace))
            {
                lowerBenchmark = 0;
                upperBenchmark = 13;
                currentPosition = 0;
                gameObject.SetActive(false);
                menuBox.SetActive(true);
            }

            // Closes Pikidex and go back to Idle.
            if(Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Tab))
            {
                lowerBenchmark = 0;
                upperBenchmark = 13;
                currentPosition = 0;
                gameObject.SetActive(false);
                CurrentPlayer.ChangeState(Players.PlayerState.Idle);
            }
        }

        /// <summary>
        /// Updates the text when the pikidex is opened or an Input is done.
        /// </summary>
        public void TextManager()
        {
            pikiminNamesList.text = "";
            for (int i = 0; i < pikimin.Length; i++)
            {
                if (lowerBenchmark <= i && upperBenchmark >= i)
                {
                    if (i == currentPosition && hasBeenSeen[i])
                    {
                        pikiminNamesList.text += " > Nr." + (i+1) + " " + pikimin[i].PikiminName + "\n\n";
                    }
                    else if (i != currentPosition && hasBeenSeen[i])
                    {
                        pikiminNamesList.text += " Nr." + (i+1) + " " + pikimin[i].PikiminName + "\n\n";
                    }
                    else if (i == currentPosition && !hasBeenSeen[i])
                    {
                        pikiminNamesList.text += " > Nr." + (i+1) + " ???" + "\n\n";
                    }
                    else if (!hasBeenSeen[i])
                    {
                        pikiminNamesList.text += " Nr." + (i+1) + " ???" + "\n\n";
                    }
                }
            }
            HightlightedPikiminUpdater();
        }

        /// <summary>
        /// Adds a Pikimin to the caught List.
        /// </summary>
        /// <param name="caughtPikimin"> The caught Pikimin. </param>
        public void CaughtPikiminUpdater(Pikimin caughtPikimin)
        {
            for (int i = 0; i < pikimin.Length; i++)
            {
                if (caughtPikimin.PikidexID == pikimin[i].PikidexID)
                {
                    hasBeenSeen[i] = true;
                    hasBeenCaught[i] = true;
                    TextManager();
                    HightlightedPikiminUpdater();
                    break;
                }
            }
        }

        /// <summary>
        /// Adds a Pikimin to the seen List.
        /// </summary>
        /// <param name="seenPikimin"> The enemies Pikimin, that has been seen. </param>
        public void SeenPikiminUpdater(Pikimin seenPikimin)
        {
            for (int i = 0; i < pikimin.Length; i++)
            {
                if (seenPikimin.PikidexID == pikimin[i].PikidexID)
                {
                    hasBeenSeen[i] = true;
                    TextManager();
                    HightlightedPikiminUpdater();
                    break;
                }
            }
        }

        /// <summary>
        /// Updates the info on the highlighted Pikimin.
        /// </summary>
        private void HightlightedPikiminUpdater()
        {
            if (hasBeenSeen[currentPosition])
            {
                pikiImage.enabled = true;
                pikiImage.sprite = pikimin[currentPosition].FrontSprite;

                if (hasBeenCaught[currentPosition])
                {
                    pikiDescription.text = pikimin[currentPosition].Description + "\n\nCan be caught in:\n\n";

                    // Adding regions it can appear in.
                    for (int i = 0; i < pikimin[currentPosition].Regions.Length; i++)
                    {
                        // Checks if that is the last route it appears in.
                        if (pikimin[currentPosition].Regions.Length > i+1)
                        {
                            pikiDescription.text += pikimin[currentPosition].Regions[i] + ", ";
                        }
                        else
                        {
                            pikiDescription.text += pikimin[currentPosition].Regions[i];
                        }
                    }
                    pikiType.text = "Type: " + pikimin[currentPosition].PikiType;
                    pikiBodyInfo.text = "Avg. Weight:    " + pikimin[currentPosition].Weight + "kg."
                                        + "\n\nAvg. Height: " + pikimin[currentPosition].Height + "m."
                                        + "\n\nRarity: " + pikimin[currentPosition].MyRarity +
                                        "\n\nGrowth: " + pikimin[currentPosition].Lvl100Exp;
                    pikiFootprintImage.enabled = true;
                    pikiFootprintImage.sprite = pikimin[currentPosition].Footprint;
                    pikiBackImage.enabled = true;
                    pikiBackImage.sprite = pikimin[currentPosition].BackSprite;
                }
                else
                {
                    pikiDescription.text = "";
                    pikiType.text = "Type: ?";
                    pikiBodyInfo.text = "";
                    pikiFootprintImage.enabled = false;
                    pikiBackImage.enabled = false;
                }
            }
            else
            {
                pikiImage.enabled = false;
                pikiBackImage.enabled = false;
                pikiFootprintImage.enabled = false;
                pikiDescription.text = "";
                pikiType.text = "Type: ?";
                pikiBodyInfo.text = "";
            }
        }

        private void LoadPikidexData()
        {
            PikidexData data = SaveManager.LoadPikidex();

            hasBeenCaught = data.hasBeenCaught;
            hasBeenSeen = data.hasBeenSeen;
        }
    }
}
