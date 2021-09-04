using System;
using Pikimins;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Battle.BattleManager;

namespace UI
{
    public class BattleUIManager : MonoBehaviour
    {
        public static BattleUIManager CurrentBattleUIManager;

        public BattleMenu currentMenu;
        
        [Header("Fight-Selection Menu")] 
        [SerializeField]private GameObject fightSelectionMenu;
        [SerializeField]private GameObject fightSelectionInfo;
        [SerializeField]private TextMeshProUGUI fightSelectionInfoText;

        [Space(20)]
        [SerializeField]private Button attack;
        [SerializeField]private TextMeshProUGUI attackT;
        [SerializeField]private Button bag;
        [SerializeField]private TextMeshProUGUI bagT;
        [SerializeField]private Button pikimin;
        [SerializeField]private TextMeshProUGUI pikiminT;
        [SerializeField]private Button run;
        [SerializeField]private TextMeshProUGUI runT;
        
        [Header("Moves")] 
        [SerializeField]private GameObject movesMenu;
        [SerializeField]private GameObject movesDetails;
        [SerializeField]private TextMeshProUGUI pp;
        [SerializeField]private TextMeshProUGUI pType;
        
        [Space(20)]
        [SerializeField]private Button move1;
        [SerializeField]private TextMeshProUGUI move1T;
        [SerializeField]private Button move2;
        [SerializeField]private TextMeshProUGUI move2T;
        [SerializeField]private Button move3;
        [SerializeField]private TextMeshProUGUI move3T;
        [SerializeField]private Button move4;
        [SerializeField]private TextMeshProUGUI move4T;
        
        [Header("PlayerBar")]
        [SerializeField]private GameObject pikiminPlayerBar;
        [SerializeField]private TextMeshProUGUI pikiminPlayerText;
        [SerializeField]private TextMeshProUGUI pikiminPlayerLevelText;
        [SerializeField]private TextMeshProUGUI pikiminPlayerHpCount;
        [SerializeField]private Slider pikiminPlayerHpBar;

        [Header("EnemyBar")]
        [SerializeField]private GameObject pikiminEnemyBar;
        [SerializeField]private TextMeshProUGUI pikiminEnemyText;
        [SerializeField]private TextMeshProUGUI pikiminEnemyLevelText;
        [SerializeField]private Slider pikiminEnemyHpBar;
        
        [Header("Pikimin")]
        [SerializeField]public GameObject pikiminMenu;

        [Header("PikiminStatus")]
        [SerializeField]public GameObject pikiminStatusCheckbox;
        [SerializeField]private TextMeshProUGUI pikiminStatusCheckboxText;

        [Space(20)] 
        [SerializeField]private GameObject pikiminStatus;
        [SerializeField]private CurrentPikiminStatus selectedPikimin;

        [Header("Bag")] 
        [SerializeField]private GameObject bagMenu;

        [Header("Dialog")] 
        [SerializeField]public GameObject dialogMenu;

        [Header("Misc")] 
        [SerializeField]private byte currentSelection;
        [SerializeField]private Sprite onActive;
        [SerializeField]private Sprite onDeActive;
        
        public Action<bool> ChangePikimin;
        
        public CurrentPikiminStatus SelectedPikimin => selectedPikimin;
        
        public TextMeshProUGUI SelectionInfoText => fightSelectionInfoText;

        private void Awake()
        {
            CurrentBattleUIManager = this;
        }

        // Start is called before the first frame update
        private void Start()
        {
            ChangePikimin += PlayerChangePikimin;
            run.onClick.AddListener(BattleManagerInstance.RunFromPikimin);
            StartDefaultText();
            
            if (BattleManagerInstance.BattleState == BattleState.Idle)
            {
                BattleManagerInstance.BattleCamera.SetActive(false);
            }
        }

        // Update is called once per frame
        private void Update()
        {
            if (currentMenu == BattleMenu.Attack || currentMenu == BattleMenu.FightSelectionMenu)
            {
                PlayerInput();
                FixSelection();
            }
            MenuSelection();

            if (BattleManagerInstance.BattleState == BattleState.Start)
            {
                currentSelection = 0;
            }
        }
        
        /// <summary>
        /// Setting up in game overlay.
        /// </summary>
        public void IsFighting(Pikimin playerPikimin, Pikimin enemyPikimin)
        {
            SelectionInfoText.text = "Choose an action.";
            ChangeMoveText(playerPikimin.CurrentMoves);
            UpdateUI(playerPikimin, enemyPikimin);
        }
        
        /// <summary>
        /// Setting text on start of the fight.
        /// </summary>
        private void StartDefaultText()
        {
            attackT.text = "attack";
            bagT.text = "bag";
            pikiminT.text = "pikimin";
            runT.text = "run";

            move1T.text = "move1";
            move2T.text = "move2";
            move3T.text = "move3";
            move4T.text = "move4";
            
            pikiminStatusCheckboxText.text = " > Status \n Swap \n Exit";
        }
        
        /// <summary>
        /// Updates PlayerPikiminUI and enemyPikiminUI
        /// </summary>
        /// <param name="playerPikimin">Pikimin</param>
        /// <param name="enemyPikimin">Pikimin</param>
        public void UpdateUI(Pikimin playerPikimin, Pikimin enemyPikimin)
        {
            pikiminPlayerText.text = playerPikimin.PikiminName;
            pikiminPlayerLevelText.text = "Lv" + playerPikimin.Level;
            pikiminPlayerHpCount.text = playerPikimin.PikiminStats.CurrentHp + "/" + playerPikimin.PikiminStats.FullHp;
            pikiminPlayerHpBar.maxValue = playerPikimin.PikiminStats.FullHp;
            pikiminPlayerHpBar.minValue = 0;
            pikiminPlayerHpBar.value = playerPikimin.PikiminStats.CurrentHp;

            pikiminEnemyText.text = enemyPikimin.PikiminName;
            pikiminEnemyLevelText.text = "Lv" + enemyPikimin.Level;
            pikiminEnemyHpBar.maxValue = enemyPikimin.PikiminStats.FullHp;
            pikiminEnemyHpBar.minValue = 0;
            pikiminEnemyHpBar.value = enemyPikimin.PikiminStats.CurrentHp;
        }
        
        /// <summary>
        /// Get player input and change selection value.
        /// </summary>
        private void PlayerInput()
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                switch (currentSelection)
                {
                    case 0:
                        currentSelection = 2;
                        break;
                    case 1:
                        currentSelection = 3;
                        break;
                    case 2:
                        currentSelection = 0;
                        break;
                    case 3:
                        currentSelection = 1;
                        break;
                }
            }

            if (!Input.GetKeyDown(KeyCode.LeftArrow) && !Input.GetKeyDown(KeyCode.A) && !Input.GetKeyDown(KeyCode.RightArrow) && !Input.GetKeyDown(KeyCode.D)) return;
            
            switch (currentSelection)
            {
                case 0:
                    currentSelection = 1;
                    break;
                case 1:
                    currentSelection = 0;
                    break;
                case 2:
                    currentSelection = 3;
                    break;
                case 3:
                    currentSelection = 2;
                    break;
            }
        }
        
        /// <summary>
        /// Provokes selection value to be out of range.
        /// </summary>
        private void FixSelection()
        {
            if (currentSelection < 1)
            {
                currentSelection = 0;
            }
            else if (currentSelection > 3)
            {
                currentSelection = 0;
            }
        }
        
        /// <summary>
        /// Changes menu.
        /// </summary>
        /// <param name="bm"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void ChangeMenu(BattleMenu bm)
        {
            currentMenu = bm;

            switch (bm)
            {
                case BattleMenu.FightSelectionMenu:
                    currentSelection = 0;
                    fightSelectionMenu.SetActive(true);
                    fightSelectionInfo.SetActive(true);
                    pikiminPlayerBar.SetActive(true);
                    pikiminEnemyBar.SetActive(true);
                    movesMenu.SetActive(false);
                    movesDetails.SetActive(false);
                    pikiminMenu.SetActive(false);
                    pikiminStatusCheckbox.SetActive(false);
                    pikiminStatus.SetActive(false);
                    bagMenu.SetActive(false);
                    dialogMenu.SetActive(false);
                    break;
                case BattleMenu.Attack:
                    currentSelection = 0;
                    fightSelectionMenu.SetActive(false);
                    fightSelectionInfo.SetActive(false);
                    pikiminPlayerBar.SetActive(true);
                    pikiminEnemyBar.SetActive(true);
                    movesMenu.SetActive(true);
                    movesDetails.SetActive(true);
                    pikiminMenu.SetActive(false);
                    pikiminStatusCheckbox.SetActive(false);
                    pikiminStatus.SetActive(false);
                    bagMenu.SetActive(false);
                    dialogMenu.SetActive(false);
                    break;
                case BattleMenu.Pikimin:
                    currentSelection = 0;
                    fightSelectionMenu.SetActive(false);
                    fightSelectionInfo.SetActive(false);
                    pikiminPlayerBar.SetActive(false);
                    pikiminEnemyBar.SetActive(false);
                    movesMenu.SetActive(false);
                    movesDetails.SetActive(false);
                    pikiminMenu.SetActive(true);
                    pikiminStatusCheckbox.SetActive(false);
                    pikiminStatus.SetActive(false);
                    bagMenu.SetActive(false);
                    dialogMenu.SetActive(false);
                    break;
                case BattleMenu.Bag:
                    currentSelection = 0;
                    fightSelectionMenu.SetActive(false);
                    fightSelectionInfo.SetActive(false);
                    pikiminPlayerBar.SetActive(false);
                    pikiminEnemyBar.SetActive(false);
                    movesMenu.SetActive(false);
                    movesDetails.SetActive(false);
                    pikiminMenu.SetActive(false);
                    pikiminStatusCheckbox.SetActive(false);
                    pikiminStatus.SetActive(false);
                    bagMenu.SetActive(true);
                    dialogMenu.SetActive(false);
                    break;
                case BattleMenu.CloseMenu:
                    currentSelection = 0;
                    fightSelectionMenu.SetActive(false);
                    fightSelectionInfo.SetActive(false);
                    pikiminPlayerBar.SetActive(false);
                    pikiminEnemyBar.SetActive(false);
                    movesMenu.SetActive(false);
                    movesDetails.SetActive(false);
                    pikiminMenu.SetActive(false);
                    pikiminStatusCheckbox.SetActive(false);
                    pikiminStatus.SetActive(false);
                    bagMenu.SetActive(false);
                    dialogMenu.SetActive(false);
                    break;
                case BattleMenu.Dialog:
                    currentSelection = 0;
                    fightSelectionMenu.SetActive(false);
                    fightSelectionInfo.SetActive(false);
                    pikiminPlayerBar.SetActive(false);
                    pikiminEnemyBar.SetActive(false);
                    movesMenu.SetActive(false);
                    movesDetails.SetActive(false);
                    pikiminMenu.SetActive(false);
                    pikiminStatusCheckbox.SetActive(false);
                    pikiminStatus.SetActive(false);
                    bagMenu.SetActive(false);
                    dialogMenu.SetActive(true);
                    break;
                case BattleMenu.FightDialog:
                    currentSelection = 0;
                    fightSelectionMenu.SetActive(false);
                    fightSelectionInfo.SetActive(false);
                    pikiminPlayerBar.SetActive(true);
                    pikiminEnemyBar.SetActive(true);
                    movesMenu.SetActive(false);
                    movesDetails.SetActive(false);
                    pikiminMenu.SetActive(false);
                    pikiminStatusCheckbox.SetActive(false);
                    pikiminStatus.SetActive(false);
                    bagMenu.SetActive(false);
                    dialogMenu.SetActive(true);
                    break;
                case BattleMenu.PikiminStatusCheckbox:
                    currentSelection = 0;
                    fightSelectionMenu.SetActive(false);
                    fightSelectionInfo.SetActive(false);
                    pikiminPlayerBar.SetActive(false);
                    pikiminEnemyBar.SetActive(false);
                    movesMenu.SetActive(false);
                    movesDetails.SetActive(false);
                    pikiminMenu.SetActive(true);
                    pikiminStatusCheckbox.SetActive(true);
                    pikiminStatus.SetActive(false);
                    bagMenu.SetActive(false);
                    dialogMenu.SetActive(false);
                    break;
                case BattleMenu.PikiminStatus:
                    currentSelection = 0;
                    fightSelectionMenu.SetActive(false);
                    fightSelectionInfo.SetActive(false);
                    pikiminPlayerBar.SetActive(false);
                    pikiminEnemyBar.SetActive(false);
                    movesMenu.SetActive(false);
                    movesDetails.SetActive(false);
                    pikiminMenu.SetActive(false);
                    pikiminStatusCheckbox.SetActive(false);
                    pikiminStatus.SetActive(true);
                    bagMenu.SetActive(false);
                    dialogMenu.SetActive(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(bm), bm, null);
            }
        }

        /// <summary>
        /// Changes PP text.
        /// </summary>
        /// <param name="value">byte currentSelection</param>
        private void BattlePikiminMoveUI(byte value)
        {
            if (BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[value] != null)
            {
                pp.text = BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[value].CurrentPp + "/" + BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[value].MAXPp;
                pType.text = "TYPE/" + BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[value].MoveType;
            }
            else if (BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[value] == null)
            {
                pp.text = "0/0";
                pType.text = "TYPE/TYPE";
            }
        }
        
         /// <summary>
        /// Changes selection and changes on player input the menu.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void MenuSelection()
        {
            switch (currentMenu)
            {
                case BattleMenu.Attack:
                    switch (currentSelection)
                    {
                        case 0:
                            move1.image.sprite = onActive;
                            move2.image.sprite = onDeActive;
                            move3.image.sprite = onDeActive;
                            move4.image.sprite = onDeActive;

                            BattlePikiminMoveUI(currentSelection);
                            
                            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
                            {
                                if (BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[0] != null)
                                {
                                    if (BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[0].CurrentPp > 0)
                                    {
                                        BattleManagerInstance.UsePikiminMove.Invoke(currentSelection);
                                    }
                                    else if   ((BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[0] == null || BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[0].CurrentPp == 0) 
                                             &&(BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[1] == null || BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[1].CurrentPp == 0) 
                                             &&(BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[2] == null || BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[2].CurrentPp == 0) 
                                             &&(BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[3] == null || BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[3].CurrentPp == 0))
                                    {
                                        BattleManagerInstance.UsePikiminMove.Invoke(currentSelection);
                                    }
                                }
                            }
                            else if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Backspace))
                            {
                                ChangeMenu(BattleMenu.FightSelectionMenu); 
                            }
                            
                            break;
                        case 1:
                            move1.image.sprite = onDeActive;
                            move2.image.sprite = onActive;
                            move3.image.sprite = onDeActive;
                            move4.image.sprite = onDeActive;
                            
                            BattlePikiminMoveUI(currentSelection);
                            
                            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
                            {
                                if (BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[1] != null)
                                {
                                    if (BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[1].CurrentPp > 0)
                                    {
                                        BattleManagerInstance.UsePikiminMove.Invoke(currentSelection);
                                    }
                                    else if   ((BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[0] == null || BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[0].CurrentPp == 0) 
                                               &&(BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[1] == null || BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[1].CurrentPp == 0) 
                                               &&(BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[2] == null || BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[2].CurrentPp == 0) 
                                               &&(BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[3] == null || BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[3].CurrentPp == 0))
                                    {
                                        BattleManagerInstance.UsePikiminMove.Invoke(currentSelection);
                                    }
                                }
                            }
                            else if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Backspace))
                            {
                                ChangeMenu(BattleMenu.FightSelectionMenu); 
                            }
                            break;
                        case 2:
                            move1.image.sprite = onDeActive;
                            move2.image.sprite = onDeActive;
                            move3.image.sprite = onActive;
                            move4.image.sprite = onDeActive;
                            
                            BattlePikiminMoveUI(currentSelection);
                            
                            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
                            {
                                if (BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[2] != null)
                                {
                                    if (BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[2].CurrentPp > 0)
                                    {
                                        BattleManagerInstance.UsePikiminMove.Invoke(currentSelection);
                                    }
                                    else if   ((BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[0] == null || BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[0].CurrentPp == 0) 
                                               &&(BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[1] == null || BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[1].CurrentPp == 0) 
                                               &&(BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[2] == null || BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[2].CurrentPp == 0) 
                                               &&(BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[3] == null || BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[3].CurrentPp == 0))
                                    {
                                        BattleManagerInstance.UsePikiminMove.Invoke(currentSelection);
                                    }
                                }
                            }
                            else if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Backspace))
                            {
                                ChangeMenu(BattleMenu.FightSelectionMenu); 
                            }
                            break;
                        case 3:
                            move1.image.sprite = onDeActive;
                            move2.image.sprite = onDeActive;
                            move3.image.sprite = onDeActive;
                            move4.image.sprite = onActive;

                            BattlePikiminMoveUI(currentSelection);
                            
                            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
                            {
                                if (BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[3] != null)
                                {
                                    if (BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[3].CurrentPp > 0)
                                    {
                                        BattleManagerInstance.UsePikiminMove.Invoke(currentSelection);
                                    }
                                    else if   ((BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[0] == null || BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[0].CurrentPp == 0) 
                                               &&(BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[1] == null || BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[1].CurrentPp == 0) 
                                               &&(BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[2] == null || BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[2].CurrentPp == 0) 
                                               &&(BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[3] == null || BattleManagerInstance.ActivePlayerPikimin.CurrentMoves[3].CurrentPp == 0))
                                    {
                                        BattleManagerInstance.UsePikiminMove.Invoke(currentSelection);
                                    }
                                }
                            }
                            else if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Backspace))
                            {
                                ChangeMenu(BattleMenu.FightSelectionMenu); 
                            }
                            break;
                    }
                    break;
                case BattleMenu.FightSelectionMenu:
                    switch (currentSelection)
                    {
                        case 0:
                            attack.image.sprite = onActive;
                            bag.image.sprite = onDeActive;
                            pikimin.image.sprite = onDeActive;
                            run.image.sprite = onDeActive;
                            
                            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
                            {
                                ChangeMenu(BattleMenu.Attack);
                            }
                            break;
                        case 1:
                            attack.image.sprite = onDeActive;
                            bag.image.sprite = onActive;
                            pikimin.image.sprite = onDeActive;
                            run.image.sprite = onDeActive;
                            
                            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
                            {
                                ChangeMenu(BattleMenu.Bag);
                            }
                            break;
                        case 2:
                            attack.image.sprite = onDeActive;
                            bag.image.sprite = onDeActive;
                            pikimin.image.sprite = onActive;
                            run.image.sprite = onDeActive;
                            
                            if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return)) && currentMenu != BattleMenu.FightDialog)
                            {
                                ChangeMenu(BattleMenu.Pikimin);
                            }
                            break;
                        case 3:
                            attack.image.sprite = onDeActive;
                            bag.image.sprite = onDeActive;
                            pikimin.image.sprite = onDeActive;
                            run.image.sprite = onActive;
                            
                            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
                            {
                                run.onClick.Invoke();
                            }
                            break;
                    }
                    break;
                case BattleMenu.Pikimin:
                    switch (currentSelection)
                    {
                        case 0:
                            
                            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
                            { 
                                ChangeMenu(BattleMenu.PikiminStatusCheckbox);
                            }
                            else if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Backspace))
                            {
                                if (BattleManagerInstance.ActivePlayerPikimin.CurrentState == Pikimin.PikiminState.Alive)
                                {
                                    ChangeMenu(BattleMenu.FightSelectionMenu);
                                }
                            }
                            break;
                        case 1:
                            
                            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
                            {
                                ChangeMenu(BattleMenu.PikiminStatusCheckbox);
                            }
                            else if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Backspace))
                            {
                                if (BattleManagerInstance.ActivePlayerPikimin.CurrentState == Pikimin.PikiminState.Alive)
                                {
                                    ChangeMenu(BattleMenu.FightSelectionMenu);
                                }
                            }
                            break;
                        case 2:
                            
                            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return)) 
                            {
                                ChangeMenu(BattleMenu.PikiminStatusCheckbox);
                            }
                            else if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Backspace))
                            {
                                if (BattleManagerInstance.ActivePlayerPikimin.CurrentState == Pikimin.PikiminState.Alive)
                                {
                                    ChangeMenu(BattleMenu.FightSelectionMenu);
                                }
                            }
                            break;
                        case 3:
                            
                            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
                            {
                                ChangeMenu(BattleMenu.PikiminStatusCheckbox);
                            }
                            else if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Backspace))
                            {
                                if (BattleManagerInstance.ActivePlayerPikimin.CurrentState == Pikimin.PikiminState.Alive)
                                {
                                    ChangeMenu(BattleMenu.FightSelectionMenu);
                                }
                            }
                            break;
                    }
                    break;
                case BattleMenu.Bag:
                    if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Backspace))
                    {
                        ChangeMenu(BattleMenu.FightSelectionMenu); 
                    }
                    break;
                case BattleMenu.PikiminStatusCheckbox:
                    if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Backspace))
                    {
                        ChangeMenu(BattleMenu.Pikimin);
                    }
                    break;
                case BattleMenu.PikiminStatus:
                    break;
                case BattleMenu.Dialog:
                    break;
                case BattleMenu.FightDialog:
                    break;
                case BattleMenu.CloseMenu:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

         /// <summary>
        /// Changing the move text to move text from active pikimin.
        /// </summary>
        private void ChangeMoveText(Moves[] pikiminMovesList)
        {
            if (pikiminMovesList[0] == null)
            {
                move1T.text = "-";
            }
            else if (pikiminMovesList[0] != null)
            {
                move1T.text = pikiminMovesList[0].MoveName;
            }
                
            if (pikiminMovesList[1] == null)
            {
                move2T.text = "-";
            }
            else if (pikiminMovesList[1] != null)
            {
                move2T.text = pikiminMovesList[1].MoveName;
            }
                
            if (pikiminMovesList[2] == null)
            {
                move3T.text = "-";
            }
            else if (pikiminMovesList[2] != null)
            {
                move3T.text = pikiminMovesList[2].MoveName;
            }
                
            if (pikiminMovesList[3] == null)
            {
                move4T.text = "-";
            }
            else if (pikiminMovesList[3] != null)
            {
                move4T.text = pikiminMovesList[3].MoveName;
            }
        }

         private static void PlayerChangePikimin(bool change)
         {
             if (CurrentBattleUIManager.SelectedPikimin.currentPikimin != null && CurrentBattleUIManager.SelectedPikimin.currentPikimin != BattleManagerInstance.ActivePlayerPikimin)
             {
                 BattleManagerInstance.PlayerWantToSwapPikimin = true;
             }
         }
        
    }

    public enum BattleState
    {
        Idle,
        Start,
        PlayerTurn,
        SwapDeadPikimin,
        EnemyTurn,
        EndBattle,
        Won,
        Lost,
        Caught,
        Stop
    }

    public enum BattleMenu
    {
        FightSelectionMenu,
        Pikimin,
        PikiminStatusCheckbox,
        PikiminStatus,
        Bag,
        Attack,
        Dialog,
        FightDialog,
        CloseMenu
    }
}
