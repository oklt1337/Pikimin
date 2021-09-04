using Pikimins;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Runtime.InteropServices;
using SaveData;
using static Audio.AudioManager;
using static Battle.BattleManager;
using static Players.Player;
using static Players.ItemInventory;
using static SaveData.SaveManager;
using static PikiminBox;
using static UI.PikidexBehaviour;

namespace Players
{
    public class PikiminInventory : MonoBehaviour
    {
        public static PikiminInventory playerPikiminInventory;
        public event Action<Pikimin> AddPikimin;

        [Header("Owned Pikimin Info")]
        [SerializeField] private Pikimin[] ownedPikimins = new Pikimin[4];
        private Pikimin swappingPikimin;
        public byte pikiminAmount;

        [Header("Healing Stuff")]
        public bool isHealing;
        public bool hasHealed;
        public bool canRevive;
        public int healStrength;

        [Header("Components")]
        public GameObject inventoryBox;
        [SerializeField] private GameObject[] currentPositionIndicators;
        [SerializeField] private GameObject menuBox;
        [SerializeField] private GameObject otherAuthorizingBox;
        [SerializeField] private OtherAuthorizeMenu otherAuthorize;
        [SerializeField] private CurrentPikiminStatus pikiminStatus;

        [Header("Pikimin UI")]
        [Space(20)]
        [SerializeField] private GameObject pikiminInventory1;
        [SerializeField] private Image pikiminInventoryImage1;
        [SerializeField] private Image pikimin1;
        [SerializeField] private TextMeshProUGUI pikiminText1;
        [SerializeField] private TextMeshProUGUI pikiminLevelText1;
        [SerializeField] private TextMeshProUGUI pikiminHpCount1;
        [SerializeField] private Slider pikiminHpBar1;

        [Space(20)]
        [SerializeField] private GameObject pikiminInventory2;
        [SerializeField] private Image pikiminInventoryImage2;
        [SerializeField] private Image pikimin2;
        [SerializeField] private TextMeshProUGUI pikiminText2;
        [SerializeField] private TextMeshProUGUI pikiminLevelText2;
        [SerializeField] private TextMeshProUGUI pikiminHpCount2;
        [SerializeField] private Slider pikiminHpBar2;

        [Space(20)]
        [SerializeField] private GameObject pikiminInventory3;
        [SerializeField] private Image pikiminInventoryImage3;
        [SerializeField] private Image pikimin3;
        [SerializeField] private TextMeshProUGUI pikiminText3;
        [SerializeField] private TextMeshProUGUI pikiminLevelText3;
        [SerializeField] private TextMeshProUGUI pikiminHpCount3;
        [SerializeField] private Slider pikiminHpBar3;

        [Space(20)]
        [SerializeField] private GameObject pikiminInventory4;
        [SerializeField] private Image pikiminInventoryImage4;
        [SerializeField] private Image pikimin4;
        [SerializeField] private TextMeshProUGUI pikiminText4;
        [SerializeField] private TextMeshProUGUI pikiminLevelText4;
        [SerializeField] private TextMeshProUGUI pikiminHpCount4;
        [SerializeField] private Slider pikiminHpBar4;

        [Header("Misc")]
        private byte currentPosition;
        private byte tempByte;
        private bool hasBeenSaved;

        public Pikimin[] OwnedPikimin
        {
            get => ownedPikimins;
            private set => ownedPikimins = value;
        }
        public byte CurrentPosition 
        {
            get => currentPosition; 
            private set => currentPosition = value;
        }

        private void Awake()
        {
            playerPikiminInventory = this;
        }

        private void Start()
        {
            pikiminAmount = 0;
            LoadData.LoadDataInstance.OnLoad += LoadPikiminInventoryData;
            
            AddPikimin += PikiBox.AddPikimin;
            AddPikimin += AddNewPikimin;
            AddPikimin += BattleManagerInstance.CaughtPikimin;
        }

        private void Update()
        {
            UpdatePikiminInfo();

            // Only allows the Inputs script to run, when the Inventory Box is active. 
            if (inventoryBox.activeSelf)
            {
                Inputs();
                pikiminStatus.currentPikimin = ownedPikimins[currentPosition];
            }

            if (otherAuthorize.wantsToSwap)
            {
                SwapPikiminPositions();
            }

            // Activates the Status screen and turns the inventory off.
            if (otherAuthorize.wantsToGetStatus)
            {
                otherAuthorize.wantsToGetStatus = false;
                pikiminStatus.gameObject.SetActive(true);
                inventoryBox.SetActive(false);
            }
        }
        
        public virtual void OnNewPikimin(Pikimin newPikimin)
        {
            PikidexInstance.CaughtPikiminUpdater(newPikimin);
            AddPikimin?.Invoke(newPikimin);
        }

        /// <summary>
        /// Adds the new Pikimin to the owned ones. 
        /// </summary>
        private void AddNewPikimin(Pikimin newPikimin)
        {
            for (int i = 0; i < 4; i++)
            {
                if (ownedPikimins[i] == null)
                {
                    OwnedPikimin[i] = BattleManagerInstance.InstantiatePikimin(newPikimin, playerPikiminInventory.transform);
                    OwnedPikimin[i].SetIvs(newPikimin.PikiminIVs.HpIV, newPikimin.PikiminIVs.AtkIV, newPikimin.PikiminIVs.DefIV, newPikimin.PikiminIVs.SpeedIV);
                    OwnedPikimin[i].CalculateStats();
                    OwnedPikimin[i].PikiminStats.CurrentHp = newPikimin.PikiminStats.CurrentHp;
                    OwnedPikimin[i].gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    OwnedPikimin[i].IsPlayerPikimin = true;
                    OwnedPikimin[i].IsWild = false;
                    pikiminAmount++;
                    PikidexInstance.CaughtPikiminUpdater(playerPikiminInventory.OwnedPikimin[0]);
                    break;
                }
            }
        }

        /// <summary>
        /// Swaps the Position of two Pikimin.
        /// </summary>
        private void SwapPikiminPositions()
        {
            if (!hasBeenSaved)
            {
                // Saves the choosen Pikimin in temporary values.
                swappingPikimin = ownedPikimins[currentPosition];
                tempByte = currentPosition;
                hasBeenSaved = true;
            }

            // Choosing the second Pikimin and then swapping their values.
            if((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E)) && swappingPikimin != null && swappingPikimin != ownedPikimins[currentPosition] && ownedPikimins[currentPosition] != null)
            {
                ownedPikimins[tempByte] = ownedPikimins[currentPosition];
                ownedPikimins[currentPosition] = swappingPikimin;
                swappingPikimin = null;
                tempByte = 5;
                hasBeenSaved = false;
                otherAuthorize.wantsToSwap = false;
                otherAuthorizingBox.SetActive(false);
            }
        }

        /// <summary>
        /// The Players Inputs.
        /// </summary>
        private void Inputs()
        {
            // Only allows chosing when the other Authorize Box is not active.
            if (!otherAuthorizingBox.activeSelf || otherAuthorize.wantsToSwap)
            {
                // Chosing the Pikimin to do stuff with.
                if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && currentPosition < 3 && ownedPikimins[currentPosition + 1] != null)
                {
                    AudioManagerInstance.Sfx.PlayOneShot(AudioManagerInstance.ButtonOverSounds[0]);
                    currentPositionIndicators[currentPosition].SetActive(false);
                    currentPosition++;
                    currentPositionIndicators[currentPosition].SetActive(true);
                }
                else if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && currentPosition > 0)
                {
                    AudioManagerInstance.Sfx.PlayOneShot(AudioManagerInstance.ButtonOverSounds[0]);
                    currentPositionIndicators[currentPosition].SetActive(false);
                    currentPosition--;
                    currentPositionIndicators[currentPosition].SetActive(true);
                }
            }

            // Closing Inventory.
            if ((Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.P)) && CurrentPlayer.CurrentState != PlayerState.Fight)
            {
                // Resetting temporary values. 
                tempByte = 5;
                swappingPikimin = null;
                otherAuthorize.wantsToSwap = false;
                hasBeenSaved = false;
                isHealing = false;

                // Closing the Inventory Box.
                otherAuthorizingBox.SetActive(false);
                Player.CurrentPlayer.ChangeState(PlayerState.Idle);
                inventoryBox.SetActive(false);
            }
            else if((Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Q)) && CurrentPlayer.CurrentState != PlayerState.Fight)
            {
                // Resetting temporary values. 
                tempByte = 5;
                swappingPikimin = null;
                otherAuthorize.wantsToSwap = false;

                // Opens the iteminventory when player wanted to heal.
                if (isHealing)
                {
                    playerItems.itemBox.SetActive(true);
                    isHealing = false;
                }

                // Setting the menu active and closing the Inventory Box.
                otherAuthorizingBox.SetActive(false);
                inventoryBox.SetActive(false);

                if(CurrentPlayer.CurrentState != PlayerState.Fight)
                {
                    menuBox.SetActive(true);
                }
            }

            // Activating the other Authorize Box when Player chooses a Pikimin.
            if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E)) && !otherAuthorizingBox.activeSelf && !otherAuthorize.wantsToSwap && ownedPikimins[currentPosition] != null && !isHealing)
            {
                AudioManagerInstance.Sfx.PlayOneShot(AudioManagerInstance.ButtonOverSounds[0]);
                otherAuthorizingBox.SetActive(true);
            }
            else if((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E)) && isHealing && !otherAuthorizingBox.activeSelf && !otherAuthorize.wantsToSwap && ownedPikimins[currentPosition] != null)
            {
                if (canRevive && ownedPikimins[currentPosition].CurrentState == Pikimin.PikiminState.Dead)
                {
                    ownedPikimins[currentPosition].Heal(healStrength, canRevive, false);
                    hasHealed = true;
                    playerItems.OwnedItems[playerItems.CurrentPosition].OnUse();
                    inventoryBox.SetActive(false);
                    if (CurrentPlayer.CurrentState != PlayerState.Fight)
                    {
                        playerItems.itemBox.SetActive(true);
                    }
                }
                else if(!canRevive && ownedPikimins[currentPosition].CurrentState == Pikimin.PikiminState.Alive)
                {
                    ownedPikimins[currentPosition].Heal(healStrength, canRevive, false);
                    hasHealed = true;
                    playerItems.OwnedItems[playerItems.CurrentPosition].OnUse();
                    inventoryBox.SetActive(false);
                    if (CurrentPlayer.CurrentState != PlayerState.Fight)
                    {
                        playerItems.itemBox.SetActive(true);
                    }
                    playerItems.DeleteMissingItem();
                }
            }
        }

        /// <summary>
        /// Updates the UI of the Pikimin Inventory with the correct values.
        /// </summary>
        private void UpdatePikiminInfo()
        {
            if(ownedPikimins[0] != null)
            {
                pikiminInventory1.SetActive(true);
                pikiminText1.text = ownedPikimins[0].PikiminName;
                pikiminLevelText1.text = "Lv. " + ownedPikimins[0].Level;
                pikiminHpCount1.text = ownedPikimins[0].PikiminStats.CurrentHp + "/" + ownedPikimins[0].PikiminStats.FullHp;
                pikiminHpBar1.maxValue = ownedPikimins[0].PikiminStats.FullHp;
                pikiminHpBar1.minValue = 0;
                pikiminHpBar1.value = ownedPikimins[0].PikiminStats.CurrentHp;
                pikimin1.sprite = ownedPikimins[0].FrontSprite;
            } 
            else if(ownedPikimins[0] == null)
            {
                pikiminInventory1.SetActive(false);
            }

            if (ownedPikimins[1] != null)
            {
                pikiminInventory2.SetActive(true);
                pikiminText2.text = ownedPikimins[1].PikiminName;
                pikiminLevelText2.text = "Lv. " + ownedPikimins[1].Level;
                pikiminHpCount2.text = ownedPikimins[1].PikiminStats.CurrentHp + "/" + ownedPikimins[1].PikiminStats.FullHp;
                pikiminHpBar2.maxValue = ownedPikimins[1].PikiminStats.FullHp;
                pikiminHpBar2.minValue = 0;
                pikiminHpBar2.value = ownedPikimins[1].PikiminStats.CurrentHp;
                pikimin2.sprite = ownedPikimins[1].FrontSprite;
            }
            else if (ownedPikimins[1] == null)
            {
                pikiminInventory2.SetActive(false);
            }

            if (ownedPikimins[2] != null)
            {
                pikiminInventory3.SetActive(true);
                pikiminText3.text = ownedPikimins[2].PikiminName;
                pikiminLevelText3.text = "Lv. " + ownedPikimins[2].Level;
                pikiminHpCount3.text = ownedPikimins[2].PikiminStats.CurrentHp + "/" + ownedPikimins[2].PikiminStats.FullHp;
                pikiminHpBar3.maxValue = ownedPikimins[2].PikiminStats.FullHp;
                pikiminHpBar3.minValue = 0;
                pikiminHpBar3.value = ownedPikimins[2].PikiminStats.CurrentHp;
                pikimin3.sprite = ownedPikimins[2].FrontSprite;
            }
            else if (ownedPikimins[2] == null)
            {
                pikiminInventory3.SetActive(false);
            }

            if (ownedPikimins[3] != null)
            {
                pikiminInventory4.SetActive(true);
                pikiminText4.text = ownedPikimins[3].PikiminName;
                pikiminLevelText4.text = "Lv. " + ownedPikimins[3].Level;
                pikiminHpCount4.text = ownedPikimins[3].PikiminStats.CurrentHp + "/" + ownedPikimins[3].PikiminStats.FullHp;
                pikiminHpBar4.maxValue = ownedPikimins[3].PikiminStats.FullHp;
                pikiminHpBar4.minValue = 0;
                pikiminHpBar4.value = ownedPikimins[3].PikiminStats.CurrentHp;
                pikimin4.sprite = ownedPikimins[3].FrontSprite;
            }
            else if (ownedPikimins[3] == null)
            {
                pikiminInventory4.SetActive(false);
            }
        }

        /// <summary>
        /// Gives the Box one of the Players Pikimin.
        /// </summary>
        /// <param name="Indicator"> Which Pikimin the Box shall get. </param>
        public void GivePikiminToBox(byte Indicator)
        {
            PikiBox.AddManualPikimin(OwnedPikimin[Indicator]);
            OwnedPikimin[Indicator] = null;
            pikiminAmount--;
            PikiminSorter();
        }

        /// <summary>
        /// Sorts the position of the owned Pikimin.
        /// </summary>
        private void PikiminSorter()
        {
            for (int i = 0; i < OwnedPikimin.Length; i++)
            {
                if (OwnedPikimin[i] == null && i < OwnedPikimin.Length - 1)
                {
                    OwnedPikimin[i] = OwnedPikimin[i + 1];
                    OwnedPikimin[i + 1] = null;
                }
            }
        }

        /// <summary>
        /// Temp.
        /// </summary>
        public void FixPikiminAmount()
        {
            pikiminAmount = 0;
            
            for (int i = 0; i < ownedPikimins.Length; i++)
            {
                if (ownedPikimins[i] != null)
                {
                    pikiminAmount++;
                }
            }
        }
        
        private void LoadPikiminInventoryData()
        {
            //Read SaveData
            PikiminInventoryData data = LoadPikiminInventory();

            if (data != null)
                for (int i = 0; i < data.pikimin.Length; i++)
                {
                    playerPikiminInventory.OwnedPikimin[i] = BattleManagerInstance.InstantiatePikimin(
                        BattleManagerInstance.AllPikimin[data.pikimin[i] - 1], playerPikiminInventory.transform);

                    playerPikiminInventory.OwnedPikimin[i].SetIvs(data.hp[i], data.atk[i], data.def[i], data.speed[i]);
                    playerPikiminInventory.OwnedPikimin[i].CalculateStats();
                    playerPikiminInventory.OwnedPikimin[i].AddExp(data.exp[i]);

                    int temp = playerPikiminInventory.OwnedPikimin[i].PikiminStats.FullHp -
                               (playerPikiminInventory.OwnedPikimin[i].PikiminStats.FullHp - data.currentHp[i]);

                    playerPikiminInventory.OwnedPikimin[i].PikiminStats.CurrentHp = temp;

                    // When Pikimin has 0 HP.
                    if (playerPikiminInventory.OwnedPikimin[i].PikiminStats.CurrentHp < 1)
                    {
                        playerPikiminInventory.OwnedPikimin[i].Dead();
                    }

                    if (i == 0)
                    {
                        for (int j = 0; j < data.pikimin0.Length; j++)
                        {
                            if (data.pikimin0 != null)
                            {
                                if (playerPikiminInventory.OwnedPikimin[i].CurrentMoves[j] != null)
                                {
                                    playerPikiminInventory.OwnedPikimin[i].CurrentMoves[j].CurrentPp = data.pikimin0[j];
                                }
                            }
                        }
                    }
                    else if (i == 1)
                    {
                        for (int j = 0; j < data.pikimin1.Length; j++)
                        {
                            if (data.pikimin1 != null)
                            {
                                if (playerPikiminInventory.OwnedPikimin[i].CurrentMoves[j] != null)
                                {
                                    playerPikiminInventory.OwnedPikimin[i].CurrentMoves[j].CurrentPp = data.pikimin1[j];
                                }
                            }
                        }
                    }
                    else if (i == 2)
                    {
                        for (int j = 0; j < data.pikimin2.Length; j++)
                        {
                            if (data.pikimin2 != null)
                            {
                                if (playerPikiminInventory.OwnedPikimin[i].CurrentMoves[j] != null)
                                {
                                    playerPikiminInventory.OwnedPikimin[i].CurrentMoves[j].CurrentPp = data.pikimin2[j];
                                }
                            }
                        }
                    }
                    else if (i == 3)
                    {
                        for (int j = 0; j < data.pikimin3.Length; j++)
                        {
                            if (data.pikimin3 != null)
                            {
                                if (playerPikiminInventory.OwnedPikimin[i].CurrentMoves[j] != null)
                                {
                                    playerPikiminInventory.OwnedPikimin[i].CurrentMoves[j].CurrentPp = data.pikimin3[j];
                                }
                            }
                        }
                    }

                    playerPikiminInventory.OwnedPikimin[i].gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    playerPikiminInventory.OwnedPikimin[i].IsPlayerPikimin = true;
                    playerPikiminInventory.pikiminAmount++;
                }
        }
    }
}
