using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Items;
using Pikimins;
using Players;
using UI;
using UnityEngine;
using static Audio.AudioManager;
using static Pikimins.Pikimin;
using static Map.Regions;
using static Players.ItemInventory;
using static Players.Player;
using static UI.BattleUIManager;
using static Players.PikiminInventory;
using static Players.PlayerController;
using static UI.DialogueManager;
using static UI.PikidexBehaviour;
using Random = UnityEngine.Random;

namespace Battle
{
    public class BattleManager : MonoBehaviour
    {
        #region Fields

        public static BattleManager BattleManagerInstance;

        [Header("Cameras")] 
        [SerializeField] private GameObject playerCamera;
        [SerializeField] private GameObject battleCamera;

        [Header("BattleBackground")] 
        [SerializeField] private SpriteRenderer battleBackground;

        [SerializeField] private Sprite[] backgroundImages;

        [Header("Battle Manager")] 
        [SerializeField] private BattleState battleState;

        [SerializeField] private RuntimeAnimatorController defaultPlayerAnim;
        [SerializeField] private RuntimeAnimatorController defaultEnemyAnim;
        
        [SerializeField] private Moves struggle;

        [Header("All Pikimin")] [SerializeField]
        private List<Pikimin> allPikimin = new List<Pikimin>();

        [SerializeField] private Transform dumbPikimin;

        [Header("Battle Pikimin")] [SerializeField]
        private Transform enemyPodium;

        [SerializeField] private Transform playerPodium;
        [SerializeField] private GameObject emptyPikimin;
        private GameObject enemyBattlePikimin;
        private GameObject playerBattlePikimin;
        private Pikimin activePlayerPikimin;
        private Pikimin activeEnemyPikimin;
        private int gainedExp;

        [Header("EndBattle")] 
        [SerializeField] private bool playerPikiminGotExp;

        [Header("Player")] 
        [SerializeField] private bool playerDidAction;

        [Header("Move")] 
        [SerializeField] private Moves playerMove;
        private bool? playerMoveIsEffective;
        [SerializeField] private bool playerWantToUseMove;
        [SerializeField] private bool playerMoveDidHit;
        [SerializeField] private bool playerMoveDamageDone;
        [SerializeField] private bool playersRoundStart;
        [SerializeField] private int playerMovePower;
        [SerializeField] private bool playerHasNoPp;

        [Header("Move Animation")] 
        [SerializeField] public Transform playerAttackAnimation;

        [SerializeField] public SpriteRenderer playerAttackSpriteRenderer;
        [SerializeField] public Animator playerAttackAnimator;

        [Header("Item")]
        [SerializeField] private bool playerWantToUseItem;
        [SerializeField] private bool didUseItem;
        [SerializeField] private bool itemHelper;
        [SerializeField] private Item usedItem;
        [SerializeField] private bool usedPikicube;
        [SerializeField] private bool pikicubeHelper;

        [Header("Try Catch")] 
        [SerializeField] private bool wildPikiminGotCaught;
        [SerializeField] private bool wildPikiminBrokeOut;
        [SerializeField] private bool pikiminBrokeOut;
        [SerializeField] private bool pikiminBrokeOutHelper;

        [Header("Swap Pikimin")] 
        [SerializeField] private bool playerWantToSwapPikimin;
        [SerializeField] private bool playerChangedPikimin;
        [SerializeField] private bool playerChangePikiminBool;
        [SerializeField] private int playerAmountUsedPikimin = 1;
        [SerializeField] private int playerAllPikiminAreDead;
        [SerializeField] private bool playerAddedDeadPikimin;
        [SerializeField] private bool playerPikiminDied;

        [Header("Npc")] 
        private TrainerBehaviour npc;
        private bool isNpcFight;

        [Header("Move")] 
        private Moves npcMove;
        private bool npcUsedMove;
        private bool npcMoveDamage;
        private bool npcMoveDidHit;
        private bool npcRoundStart;
        private bool? npcIsEffective;
        private int npcMovePower;

        [Header("Move Animation")] 
        [SerializeField] private Transform enemyAttackAnimation;

        [SerializeField] private SpriteRenderer enemyAttackSpriteRenderer;
        [SerializeField] private Animator enemyAttackAnimator;

        [Header("Swap Pikimin")] 
        private bool npcChangePikiminBool;
        private bool npcChangedPikimin;
        private bool createNewNpcPikimin;
        private bool npcChoosePikimin;
        private bool npcOldPikiminGone;
        private bool npcAllPikiminAreDead;
        private int npcAmountDeadPikimin;

        [Header("Legendary")] 
        private bool isLegendary;

        [Header("Battle Animations")] [SerializeField]
        private GameObject battleStartAnimation;
    
        private static readonly int Caught = Animator.StringToHash("Caught");

        #region Events

        public Action<int> UsePikiminMove;

        public delegate void PlayerEventArg();

        public PlayerEventArg ONPlayerDeath;

        public delegate void LegendaryBattle();

        public LegendaryBattle OnLegendaryBattleEnd;

        public delegate void BattleEventArg(Pikimin legendary, TrainerBehaviour newNpc, Rarity rarity, BiomeList biome, int minLvl, int maxLvl, AudioClip battleStartClip);

        public BattleEventArg OnBattleStart;

        #endregion

        #endregion

        #region Properties

        public GameObject BattleCamera => battleCamera;

        public BattleState BattleState
        {
            get => battleState;
            private set => battleState = value;
        }

        public List<Pikimin> AllPikimin => allPikimin;

        public Transform DumbPikimin => dumbPikimin;

        public Pikimin ActivePlayerPikimin => activePlayerPikimin;

        public Pikimin ActiveEnemyPikimin => activeEnemyPikimin;

        public int GainedExp => gainedExp;

        public Moves PlayerMove => playerMove;

        public bool PlayerHasNoPp => playerHasNoPp;

        public bool PlayerWantToUseItem
        {
            get => playerWantToUseItem;
            set => playerWantToUseItem = value;
        }

        public Item UsedItem
        {
            get => usedItem;
            set => usedItem = value;
        }

        public bool WildPikiminBrokeOut
        {
            get => wildPikiminBrokeOut;
            set => wildPikiminBrokeOut = value;
        }

        public bool PlayerWantToSwapPikimin
        {
            get => playerWantToSwapPikimin;
            set => playerWantToSwapPikimin = value;
        }

        public int PlayerAmountUsedPikimin => playerAmountUsedPikimin;

        public int PlayerAllPikiminAreDead
        {
            get => playerAllPikiminAreDead;
            set => playerAllPikiminAreDead = value;
        }

        public TrainerBehaviour Npc => npc;

        public bool IsNpcFight => isNpcFight;

        public Moves NpcMove => npcMove;

        #endregion

        #region General

        private void Awake()
        {
            BattleManagerInstance = this;
        }

        // Start is called before the first frame update
        private void Start()
        {
            BattleState = BattleState.Idle;
            UsePikiminMove += UsePlayerPikiminMove;
            OnBattleStart += StartBattleAnimation;
        }

        private void Update()
        {
            switch (BattleState)
            {
                case BattleState.Start when Dialogue.chattingHasEnded:
                case BattleState.PlayerTurn when !playerPikiminDied:
                    PlayersTurn();
                    break;
                case BattleState.SwapDeadPikimin
                    when activePlayerPikimin.CurrentState == PikiminState.Dead || playerChangedPikimin:
                    ChangePikiminAfterDead();
                    break;
                case BattleState.SwapDeadPikimin
                    when activeEnemyPikimin.CurrentState == PikiminState.Dead || npcChangedPikimin:
                    EnemySwapPikimin();
                    break;
                case BattleState.EnemyTurn:
                    EnemysTurn();
                    break;
                case BattleState.Idle:
                    break;
                case BattleState.EndBattle:
                    EndBattle();
                    break;
                case BattleState.Won:
                    WonEndBattle();
                    break;
                case BattleState.Lost:
                    LostEndBattle();
                    break;
                case BattleState.Caught:
                    CaughtEndBattle();
                    break;
                case BattleState.Stop:
                    StopEndBattle();
                    break;
                case BattleState.Start:
                    Dialogue.BattleDialogueManager();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion

        #region EnterBattle

        /// <summary>
        /// Starting battle with wild pikimin.
        /// </summary>
        /// <param name="legendary"></param>
        /// <param name="newNpc"></param>
        /// <param name="rarity"></param>
        /// <param name="biome"></param>
        /// <param name="minLvl"></param>
        /// <param name="maxLvl"></param>
        private void EnterBattle(Pikimin legendary, TrainerBehaviour newNpc, Rarity rarity, BiomeList biome, int minLvl, int maxLvl)
        {
            npc = newNpc;
            SetupBattle(legendary, newNpc, rarity, biome, minLvl, maxLvl);
        }

        #endregion

        #region Setup

        /// <summary>
        /// Crate's PlayerPikimin and if needed WildPikimin.
        /// </summary>
        private void SetupBattle(Pikimin legendary, TrainerBehaviour newNpc, Rarity rarity, BiomeList biome, int minLvl, int maxLvl)
        {
            BattleState = BattleState.Start;
            CurrentBattleUIManager.ChangeMenu(BattleMenu.FightDialog);
        
            //Temp.
            playerPikiminInventory.FixPikiminAmount();

            ChangeCamera();
            SetBackground(biome);
            CheckDeadForDeadPikimins();
            CreateStartPikimin();

            if (biome != BiomeList.Npc && newNpc == null && legendary == null)
            {
                SetupWildBattle(rarity, biome, minLvl, maxLvl);
                CurrentPlayerController.transform.position = CurrentPlayerController.oldPos;
            }
            else if (biome == BiomeList.Npc && newNpc != null && legendary == null)
            {
                SetupNpcBattle(newNpc);
            }
            else if (legendary != null)
            {
                SetupLegendaryBattle(legendary);
                CurrentPlayerController.transform.position = CurrentPlayerController.oldPos;
            }
        }

        /// <summary>
        /// Creat first player pikimin.
        /// </summary>
        private void CreateStartPikimin()
        {
            foreach (var pikimin in CurrentPlayer.playerOwnedPikimins)
            {
                if (pikimin == null) continue;
                if (pikimin.CurrentState != PikiminState.Alive) continue;
                CreatePlayerPikimin(pikimin);
                break;
            }
        }

        /// <summary>
        /// Sets up NPC Battle.
        /// </summary>
        private void SetupWildBattle(Rarity rarity, BiomeList biome, int minLvl, int maxLvl)
        {
            isNpcFight = false;
            isLegendary = false;

            CreatePikimin(rarity, biome, CurrentPlayer.PlayerRegion, minLvl, maxLvl);

            CurrentBattleUIManager.IsFighting(activePlayerPikimin, activeEnemyPikimin);
        }

        /// <summary>
        /// Sets up Wild Battle.
        /// </summary>
        private void SetupNpcBattle(TrainerBehaviour newNpc)
        {
            isNpcFight = true;
            isLegendary = false;
            npcAmountDeadPikimin = 0;
            npcAllPikiminAreDead = false;
            CreateNpcPikimin(newNpc.OwnedPikimin[0]);
            CurrentBattleUIManager.IsFighting(activePlayerPikimin, activeEnemyPikimin);
        }

        private void SetupLegendaryBattle(Pikimin legendary)
        {
            isNpcFight = false;
            isLegendary = true;
            CreatePikimin(legendary);
            CurrentBattleUIManager.IsFighting(activePlayerPikimin, activeEnemyPikimin);
        }

        /// <summary>
        /// Sets the Battle background.
        /// </summary>
        private void SetBackground(BiomeList biome)
        {
            switch (biome)
            {
                case BiomeList.Grass:
                    battleBackground.sprite = backgroundImages[1];
                    break;
                case BiomeList.Forest:
                    battleBackground.sprite = backgroundImages[2];
                    break;
                case BiomeList.Rock:
                    battleBackground.sprite = backgroundImages[3];
                    break;
                case BiomeList.Water:
                    battleBackground.sprite = backgroundImages[4];
                    break;
                case BiomeList.Npc:
                    battleBackground.sprite = backgroundImages[0];
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(biome), biome, null);
            }
        }

        #endregion

        #region Player

        /// <summary>
        /// Manage players turn.
        /// </summary>
        private void PlayersTurn()
        {
            if (!playersRoundStart)
            {
                BattleState = BattleState.PlayerTurn;
                PikidexBehaviour.PikidexInstance.SeenPikiminUpdater(ActiveEnemyPikimin);
                Dialogue.chattingHasEnded = false;
                CurrentBattleUIManager.ChangeMenu(BattleMenu.FightSelectionMenu);
                playersRoundStart = true;
            }

            if (playerWantToUseMove)
            {
                if (playerHasNoPp)
                {
                    PlayerUseStruggle();
                }
                else
                {
                    PlayerUseMove();
                }
            }
            else if (playerWantToUseItem)
            {
                PlayerItem();
            }
            else if (playerWantToSwapPikimin)
            {
                PlayerChangePikimin();
            }

            if (playerDidAction)
            {
                EndPlayerTurn();
            }
        }

        /// <summary>
        /// Use pikimin move from array.
        /// </summary>
        private void UsePlayerPikiminMove(int selection)
        {
            playerHasNoPp = false;
            playerWantToUseMove = false;
            int noPp = 0;
            int allMoves = 0;
            bool allPpGone = false;

            for (int i = 0; i < activePlayerPikimin.CurrentMoves.Length; i++)
            {
                if (activePlayerPikimin.CurrentMoves[i] != null)
                {
                    allMoves++;
                    if (activePlayerPikimin.CurrentMoves[i].CurrentPp == 0)
                    {
                        noPp++;
                    }
                }

                if (i == activePlayerPikimin.CurrentMoves.Length -1 && allMoves == noPp)
                { 
                    allPpGone = true;
                }
            }

            if (allPpGone)
            {
                playerHasNoPp = true;
            }
            else
            {
                playerMove = activePlayerPikimin.CurrentMoves[selection];
            }
            playerWantToUseMove = true;
        }

        /// <summary>
        /// Manage Player Move
        /// </summary>
        private void PlayerUseMove()
        {
            if (playerMove && !playerMoveDamageDone)
            {
                CurrentBattleUIManager.ChangeMenu(BattleMenu.FightDialog);
            
                (playerMovePower, playerMoveDidHit) = activePlayerPikimin.UseMove(playerMove);

                if (playerMoveDidHit)
                {
                    playerMoveIsEffective = CheckForEffectivity(playerMove);
                    activeEnemyPikimin.TakeDamage(DamageCalculation(playerMovePower, activePlayerPikimin.PikiminStats.AttackStat,
                        activeEnemyPikimin.PikiminStats.DefenseStat, activePlayerPikimin.Level, CalculateCrit(activeEnemyPikimin.PikiminStats.SpeedStat, activePlayerPikimin.PikiminStats.SpeedStat), playerMoveIsEffective));
                    StartCoroutine(MoveAnimation(playerMove, playerAttackSpriteRenderer, playerAttackAnimator, playerAttackAnimation));
                    CurrentBattleUIManager.UpdateUI(activePlayerPikimin, activeEnemyPikimin);
                }
                playerMoveDamageDone = true;
            }

            Dialogue.BattleDialogueManager(playerMoveDidHit, playerMoveIsEffective);

            if (Dialogue.chattingHasEnded)
            {
                playerAttackAnimator.enabled = false;
                playerAttackSpriteRenderer.enabled = false;
                playerDidAction = true;
                Dialogue.chattingHasEnded = false;
            }

            if (activeEnemyPikimin.CurrentState == PikiminState.Dead && npc != null && playerDidAction)
            {
                EndPlayerTurn();
                BattleState = BattleState.SwapDeadPikimin;
            }
        }

        private void PlayerUseStruggle()
        {
            if (playerMove && !playerMoveDamageDone)
            {
                CurrentBattleUIManager.ChangeMenu(BattleMenu.FightDialog);

                var temp = Random.Range(0, 100);
                playerMoveDidHit = temp > 10;

                if (playerMoveDidHit)
                {
                    playerMoveIsEffective = null;
                    activeEnemyPikimin.TakeDamage(DamageCalculation(50, activePlayerPikimin.PikiminStats.AttackStat,
                        activeEnemyPikimin.PikiminStats.DefenseStat, activePlayerPikimin.Level, CalculateCrit(activeEnemyPikimin.PikiminStats.SpeedStat, activePlayerPikimin.PikiminStats.SpeedStat), playerMoveIsEffective));
                    activePlayerPikimin.TakeDamage(DamageCalculation(25, activePlayerPikimin.PikiminStats.AttackStat,
                        activeEnemyPikimin.PikiminStats.DefenseStat, activePlayerPikimin.Level, CalculateCrit(activeEnemyPikimin.PikiminStats.SpeedStat, activePlayerPikimin.PikiminStats.SpeedStat), playerMoveIsEffective));
                    StartCoroutine(MoveAnimation(struggle, playerAttackSpriteRenderer, playerAttackAnimator, playerAttackAnimation));
                    CurrentBattleUIManager.UpdateUI(activePlayerPikimin, activeEnemyPikimin);
                }
                playerMoveDamageDone = true;
            }

            Dialogue.BattleDialogueManager(playerMoveDidHit, playerMoveIsEffective);

            if (Dialogue.chattingHasEnded)
            {
                playerAttackAnimator.enabled = false;
                playerAttackSpriteRenderer.enabled = false;
                playerDidAction = true;
                Dialogue.chattingHasEnded = false;
            }

            if (activeEnemyPikimin.CurrentState == PikiminState.Dead && npc != null && playerDidAction)
            {
                EndPlayerTurn();
                BattleState = BattleState.SwapDeadPikimin;
            }
        }

            /// <summary>
        /// Ends PlayerTurn after using Item.
        /// </summary>
        private void PlayerItem()
        {
            if (!didUseItem)
            {
                didUseItem = true;
                Dialogue.chattingHasEnded = false;
                CurrentBattleUIManager.ChangeMenu(BattleMenu.FightDialog);
                
                if (usedItem is Potion)
                {
                    Potion temp = usedItem as Potion;
                    if (!(temp is null) && temp.CanRevive)
                    {
                        playerAllPikiminAreDead--;
                    }
                }
            }

            if (!Dialogue.chattingHasEnded && !Input.GetKeyDown(KeyCode.E) && !Input.GetKeyDown(KeyCode.Return) || itemHelper)
            {
                Dialogue.ManuelDialogue(CurrentPlayer.name + " used " + usedItem.ItemName + ".");
                CurrentBattleUIManager.UpdateUI(activePlayerPikimin, activeEnemyPikimin);
            
                if (usedItem is Pikicube && !usedPikicube)
                {
                    StartCoroutine(PikicubeAnimation(usedItem as Pikicube, playerAttackSpriteRenderer, playerAttackAnimator,
                        playerAttackAnimation));

                    usedPikicube = true;
                }

                itemHelper = true;
            }
        
            if (Dialogue.chattingHasEnded && !(usedItem is Pikicube) || pikicubeHelper)
            {
                if (wildPikiminGotCaught && pikicubeHelper) 
                {
                    BattleState = BattleState.EndBattle;
                    Dialogue.chattingHasEnded = false;
                    playerDidAction = true; 
                    playerWantToUseItem = false;
                    itemHelper = false;
                }
                else if (wildPikiminBrokeOut && pikicubeHelper)
                {
                    pikiminBrokeOut = true;
                    Dialogue.chattingHasEnded = false;
                    itemHelper = false;
                }
                else if (itemHelper)
                {
                    Dialogue.chattingHasEnded = false;
                    itemHelper = false;
                    playerDidAction = true;
                    playerWantToUseItem = true;
                }
            }
        
            if (pikiminBrokeOut && !Input.GetKeyDown(KeyCode.E) && !Input.GetKeyDown(KeyCode.Return) || pikiminBrokeOutHelper)
            {
                Dialogue.ManuelDialogue(activeEnemyPikimin.PikiminName + " broke out.");

                pikiminBrokeOutHelper = true;

                if (Dialogue.chattingHasEnded)
                {
                    playerDidAction = true;
                    playerWantToUseItem = false;
                    pikiminBrokeOut = false;
                    pikiminBrokeOutHelper = false;
                    itemHelper = false;
                    usedPikicube = false;
                    pikicubeHelper = false;
                }
            }
        }

        /// <summary>
        /// Manage the pikimin change.
        /// </summary>
        private void PlayerChangePikimin()
        {
            if (!playerChangePikiminBool)
            {
                playerChangePikiminBool = true;
                Dialogue.chattingHasEnded = false;
            }

            switch (CurrentBattleUIManager.SelectedPikimin.currentPikimin.CurrentState)
            {
                case PikiminState.Alive:
                {
                    if (!playerChangedPikimin)
                    {
                        ChangeMyPikimin(CurrentBattleUIManager.SelectedPikimin.currentPikimin);
                        CurrentBattleUIManager.ChangeMenu(BattleMenu.FightDialog);
                        playerChangedPikimin = true;
                    }
                
                    Dialogue.ManuelDialogue(activePlayerPikimin.PikiminName + " I choose you!");
                    
                    if (Dialogue.chattingHasEnded)
                    {
                        if (BattleState == BattleState.PlayerTurn)
                        {
                            playerDidAction = true; 
                        }
                    
                        if (BattleState == BattleState.SwapDeadPikimin)
                        {
                            playerWantToSwapPikimin = false;
                            playerPikiminDied = false;
                            playerAddedDeadPikimin = false;
                            BattleState = BattleState.PlayerTurn;
                        }
                        playerChangePikiminBool = false;
                    }
                    break;
                }
                case PikiminState.Dead:
                {
                    CurrentBattleUIManager.ChangeMenu(BattleMenu.Pikimin);
                    CurrentBattleUIManager.dialogMenu.SetActive(true);
                
                    Dialogue.ManuelDialogue("This Pikimin is dead, choose a different one.");

                    if (Dialogue.chattingHasEnded)
                    {
                        CurrentBattleUIManager.dialogMenu.SetActive(false);
                        playerChangePikiminBool = false;
                        playerWantToSwapPikimin = false;
                    }
                    break;
                }
            }
        }

        private void ChangePikiminAfterDead()
        {
            if (!playerAddedDeadPikimin)
            {
                playerAllPikiminAreDead++;
                playerAmountUsedPikimin++;
                playerAddedDeadPikimin = true;
                Dialogue.chattingHasEnded = false;
            }

            if (playerAllPikiminAreDead == playerPikiminInventory.pikiminAmount)
            {
                BattleState = BattleState.EndBattle;
            }
            else
            {
                if (!playerPikiminDied)
                {
                    CurrentBattleUIManager.ChangeMenu(BattleMenu.Pikimin);
                    CurrentBattleUIManager.dialogMenu.SetActive(true);
                    playerPikiminDied = true;
                }
                            
                Dialogue.ManuelDialogue("Choose your next pikimin.");

                if (Dialogue.chattingHasEnded)
                {
                    CurrentBattleUIManager.dialogMenu.SetActive(false);
                    Dialogue.chattingHasEnded = false;
                }

                if (playerWantToSwapPikimin)
                {
                    PlayerChangePikimin();
                }
            }
        }
    
        /// <summary>
        /// Ends Player Turn and reset all bools.
        /// </summary>
        private void EndPlayerTurn()
        {
            playersRoundStart = false;
            playerWantToUseMove = false;
            playerWantToUseItem = false;
            playerWantToSwapPikimin = false;
            playerDidAction = false;
            playerChangePikiminBool = false;
        
            playerMoveDamageDone = false;
            playerMoveDidHit = false;

            playerChangedPikimin = false;

            didUseItem = false;
            wildPikiminBrokeOut = false;

            playerAttackAnimator.enabled = false;
            playerAttackSpriteRenderer.enabled = false;

            if (BattleState != BattleState.EndBattle && BattleState != BattleState.SwapDeadPikimin)
            {
                BattleState = activeEnemyPikimin.CurrentState == PikiminState.Dead ? BattleState.EndBattle : BattleState.EnemyTurn;
            }
        }

        /// <summary>
        /// Exchange Pikimins.
        /// </summary>
        /// <param name="changedPikimin"></param>
        private void ChangeMyPikimin(Pikimin changedPikimin)
        {
            if (playerWantToSwapPikimin)
            {
                CreatePlayerPikimin(changedPikimin);
                CurrentBattleUIManager.IsFighting(activePlayerPikimin, activeEnemyPikimin);
            }
        }
        #endregion

        #region Npc
        /// <summary>
        /// Enemy Turn.
        /// </summary>
        /// <returns></returns>
        private void EnemysTurn()
        {
            activeEnemyPikimin.GetComponent<SpriteRenderer>().enabled = true;
            EnemyMove();

            if (Dialogue.chattingHasEnded)
            {
                EndEnemyTurn();
                Dialogue.chattingHasEnded = false;
            }
        }

        /// <summary>
        /// Use Random Move.
        /// </summary>
        private void EnemyMove()
        {
            if (!npcUsedMove && !npcRoundStart)
            {
                (npcMovePower, npcMoveDidHit, npcMove) = activeEnemyPikimin.UseRandomMove();
                npcRoundStart = true;
                Dialogue.chattingHasEnded = false;
            }

            Dialogue.BattleDialogueManager(npcMoveDidHit, npcIsEffective);

            if (npcMoveDidHit)
            {
                if (!npcMoveDamage)
                {
                    npcIsEffective  = CheckForEffectivity(npcMove);
                    activePlayerPikimin.TakeDamage(DamageCalculation(npcMovePower, activeEnemyPikimin.PikiminStats.AttackStat,
                        activePlayerPikimin.PikiminStats.DefenseStat, activeEnemyPikimin.Level, CalculateCrit(activePlayerPikimin.PikiminStats.SpeedStat, activeEnemyPikimin.PikiminStats.SpeedStat),
                        npcIsEffective));
                    StartCoroutine(MoveAnimation(npcMove, enemyAttackSpriteRenderer, enemyAttackAnimator, enemyAttackAnimation));
                    CurrentBattleUIManager.UpdateUI(activePlayerPikimin, activeEnemyPikimin);
                    npcMoveDamage = true;
                }
            }
            if (Dialogue.chattingHasEnded) 
            {
                if (activePlayerPikimin.CurrentState == PikiminState.Dead)
                {
                    BattleState = BattleState.SwapDeadPikimin;
                }
                enemyAttackAnimator.enabled = false;
                enemyAttackSpriteRenderer.enabled = false;
                npcUsedMove = true;
            }
        }

        private void EnemySwapPikimin()
        {
            if (!npcChangePikiminBool)
            {
                npcAmountDeadPikimin++;
                npcChangePikiminBool = true;
                Dialogue.chattingHasEnded = false;
            }

            if (npcAmountDeadPikimin == npc.OwnedPikimin.Length)
            {
                npcAllPikiminAreDead = true;
            }
        
            Dialogue.ManuelDialogue(activeEnemyPikimin.PikiminName + " has been beaten.");

            if (Dialogue.chattingHasEnded)
            {
                createNewNpcPikimin = true;
                Dialogue.chattingHasEnded = false;
            }

            if (npcAllPikiminAreDead)
            {
                BattleState = BattleState.EndBattle;
            }
            else
            {
                if (createNewNpcPikimin)
                {
                    if (!npcOldPikiminGone)
                    {
                        Destroy(enemyBattlePikimin.gameObject);
                        npcOldPikiminGone = true;
                    }
            
                    if (npcOldPikiminGone)
                    {
                        for (int i = 0; i < npc.OwnedPikimin.Length; i++)
                        {
                            if (npc.OwnedPikimin[i].CurrentState == PikiminState.Alive && activeEnemyPikimin.CurrentState == PikiminState.Dead)
                            {
                                CreateNpcPikimin(npc.OwnedPikimin[i]);
                                activeEnemyPikimin = npc.OwnedPikimin[i];
                                CurrentBattleUIManager.UpdateUI(activePlayerPikimin,activeEnemyPikimin);
                                npcChangedPikimin = true;
                                Dialogue.chattingHasEnded = false;
                                break;
                            }
                        }
                    }
                }

                if (npcChangedPikimin && !(Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return)) || npcChoosePikimin)
                {
                    Dialogue.ManuelDialogue(npc.CharacterName + " chose " + activeEnemyPikimin.PikiminName + ".");
                    npcChoosePikimin = true;

                    if (Dialogue.chattingHasEnded)
                    {
                        createNewNpcPikimin = false;
                        Dialogue.chattingHasEnded = false;
                    
                        if (BattleState == BattleState.SwapDeadPikimin)
                        {
                            npcOldPikiminGone = false;
                            npcChangePikiminBool = false;
                            npcChangedPikimin = false;
                            npcChoosePikimin = false;
                            BattleState = BattleState.PlayerTurn;
                        }
                    }
                }
            }
        }
    
        /// <summary>
        /// End enemy turn.
        /// </summary>
        private void EndEnemyTurn()
        {
            npcRoundStart = false;
            npcUsedMove = false;
            npcMoveDamage = false;
            npcMoveDidHit = false;
            playerWantToSwapPikimin = false;
        
            if (BattleState != BattleState.EndBattle && BattleState != BattleState.SwapDeadPikimin)
            {
                BattleState = BattleState.PlayerTurn;
                CurrentBattleUIManager.currentMenu = BattleMenu.FightSelectionMenu;
            }
        }
        #endregion

        #region EndBattle
        /// <summary>
        /// Ending the battle
        /// </summary>
        private void EndBattle()
        {
            if (playerAllPikiminAreDead < playerPikiminInventory.pikiminAmount && !wildPikiminGotCaught)
            {
                BattleState = BattleState.Won;
            }
            else if (playerAllPikiminAreDead == playerPikiminInventory.pikiminAmount)
            {
                BattleState = BattleState.Lost;
            }
            else if (wildPikiminGotCaught && !isNpcFight)
            {
                BattleState = BattleState.Caught;
            }
        }
    
        private void CaughtEndBattle()
        {
            Dialogue.ManuelDialogue("You caught " + activeEnemyPikimin.PikiminName + ".");
            
            if (Dialogue.chattingHasEnded)
            {
                BattleState = BattleState.Stop;
            }

            if (isLegendary)
            {
                OnLegendaryBattleEnd?.Invoke();
            }
        }
    
        private void LostEndBattle()
        {
            Dialogue.BattleDialogueManager();
            
            if (Dialogue.chattingHasEnded)
            {
                ONPlayerDeath.Invoke();
                if (isNpcFight)
                {
                    npc.Reset();
                    playerItems.AddMoney(-Convert.ToInt32(npc.Money * 0.5f));
                }
                BattleState = BattleState.Stop;
            }
        }

        private void WonEndBattle()
        {
            if (!playerPikiminGotExp)
            {
                gainedExp = 0;
                if (isNpcFight)
                {
                    for (int i = 0; i < npc.OwnedPikimin.Length; i++)
                    {
                        if (npc.OwnedPikimin[i] != null)
                        {
                            gainedExp += npc.OwnedPikimin[i].GainedExpCalculation(playerAmountUsedPikimin);
                        }
                    }
                }
                else
                {
                    gainedExp = activeEnemyPikimin.GainedExpCalculation(playerAmountUsedPikimin);
                }

                for (int i = 0; i < playerPikiminInventory.OwnedPikimin.Length; i++)
                {
                    if (playerPikiminInventory.OwnedPikimin[i] != null && playerPikiminInventory.OwnedPikimin[i].CurrentState == PikiminState.Alive)
                    {
                        playerPikiminInventory.OwnedPikimin[i].AddExp(gainedExp);
                    }
                }
                playerPikiminGotExp = true;
            
                if (npc != null && npc is DojoChefBehaviour)
                {
                    AudioManagerInstance.PlayAudioClip(false, AudioManagerInstance.DojoChefSounds[2]);
                }
                else
                {
                    AudioManagerInstance.PlayAudioClip(false, AudioManagerInstance.TrainerSounds[2]);
                }
            }

            Dialogue.BattleDialogueManager();
            
            if (Dialogue.chattingHasEnded)
            {
                BattleState = BattleState.Stop;
                if (isNpcFight)
                {
                    npc.WasBeenBeaten();
                    playerItems.AddMoney(npc.Money);
                }

                if (isLegendary)
                {
                    OnLegendaryBattleEnd?.Invoke();
                }
            }
        }

        private void StopEndBattle()
        {
            AudioManagerInstance.PlayAudioClip(false, AudioManagerInstance.AreaSounds[(int) CurrentPlayer.PlayerRegion]);

            CurrentPlayer.ChangeState(PlayerState.Idle);
            CurrentBattleUIManager.ChangeMenu(BattleMenu.CloseMenu);
            BattleState = BattleState.Idle;
            ChangeCamera();

            if (isNpcFight)
            {
                npc = null;
                isNpcFight = false;
                npcChangePikiminBool = false;
                npcAllPikiminAreDead = false;
                npcAmountDeadPikimin = 0;
            }
            
            Destroy(activeEnemyPikimin.IsWild ? activeEnemyPikimin.gameObject : enemyBattlePikimin.gameObject);
            Destroy(playerBattlePikimin);

            ResetAll();
        }
    
        /// <summary>
        /// Makes you run from a wild pikimin.
        /// </summary>
        public void RunFromPikimin()
        {
            if (!isNpcFight)
            {
                CurrentPlayer.ChangeState(PlayerState.Idle);
                CurrentBattleUIManager.ChangeMenu(BattleMenu.CloseMenu);
                BattleState = BattleState.Idle;
                ChangeCamera();
                ResetAll();

                Destroy(activeEnemyPikimin.gameObject);
                Destroy(playerBattlePikimin);
            
                AudioManagerInstance.PlayAudioClip(false, AudioManagerInstance.AreaSounds[(int) CurrentPlayer.PlayerRegion]);
            }

            if (isLegendary)
            {
                OnLegendaryBattleEnd?.Invoke();
            }
        }
    
        /// <summary>
        /// Resets ALL of them bools.
        /// </summary>
        private void ResetAll()
        {
            Dialogue.chattingHasEnded = false;
        
            //Pikimin
            //activeEnemyPikimin = null;
            activePlayerPikimin = null;
            
            //Player
            playersRoundStart = false;
            playerDidAction = false;
            playerPikiminDied = false;
            playerAddedDeadPikimin = false;
            
            //Move
            playerMove = null;
            playerWantToUseMove = false;
            playerMoveDidHit = false;
            playerMoveDamageDone = false;
            playerMovePower = 0;

            //Item
            usedItem = null;
            playerWantToUseItem = false;
            didUseItem = false;
            itemHelper = false;
            
            //Cube
            usedPikicube = false;
            pikiminBrokeOut = false;
            pikiminBrokeOutHelper = false;
            wildPikiminBrokeOut = false;
            wildPikiminGotCaught = false;
            pikicubeHelper = false;

            //SwapPikimin
            playerWantToSwapPikimin = false;
            playerChangePikiminBool = false;
            playerChangedPikimin = false;
            
            //Wild
            isLegendary = false;
            
            //Npc
            isNpcFight = false;
            npcRoundStart = false;
            npcAllPikiminAreDead = false;
            
            //Move
            npcMovePower = 0;
            npcUsedMove = false;
            npcMoveDamage = false;
            npcMoveDidHit = false;
            
            //SwapPikimin
            createNewNpcPikimin = false;
            npcChangedPikimin = false;
            npcChangePikiminBool = false;
            npcChoosePikimin = false;
            npcOldPikiminGone = false;
            
            //EndBattle
            gainedExp = 0;
            playerPikiminGotExp = false;
        }

        /// <summary>
        /// Starts EndBattle.
        /// </summary>
        public void CaughtPikimin(Pikimin newPikimin)
        {
            if (battleState != BattleState.Idle)
            {
                wildPikiminGotCaught = true;
            }
        }

        /// <summary>
        /// Checks how many Pikimin are dead.
        /// </summary>
        private void CheckDeadForDeadPikimins()
        {
            playerAmountUsedPikimin = 1;
            playerAllPikiminAreDead = 0;
            foreach (var t in CurrentPlayer.playerOwnedPikimins)
            {
                if (t == null) continue;
                if (t.CurrentState == PikiminState.Dead)
                {
                    playerAllPikiminAreDead++;
                }
            }
        }
        #endregion

        #region CreatePikimins
        /// <summary>
        /// Instantiates a Pikimin.
        /// </summary>
        /// <param name="prefab">Pikimin</param>
        /// <param name="myParent">Patent</param>
        /// <returns>Pikimin Instantiated</returns>
        public Pikimin InstantiatePikimin(Pikimin prefab,Transform myParent)
        {
            var tempPikimin = Instantiate(prefab, myParent, true);
            tempPikimin.AddMember(tempPikimin);
            tempPikimin.transform.position = dumbPikimin.position;

            return tempPikimin;
        }
    
        /// <summary>
        /// Creating players first pikimin.
        /// </summary>
        private void CreatePlayerPikimin(Pikimin chosenPikimin)
        {
            if (playerBattlePikimin != null)
            {
                Destroy(playerBattlePikimin);
            }
        
            Pikimin pikimin = chosenPikimin;
            playerBattlePikimin = Instantiate(emptyPikimin, playerPodium.transform.position, Quaternion.identity);

            Vector3 pikiLocalPos = new Vector3(0, 1, 0);

            playerBattlePikimin.transform.parent = playerPodium;
            playerBattlePikimin.transform.localPosition = pikiLocalPos;
        
            playerBattlePikimin.GetComponent<SpriteRenderer>().sprite = pikimin.BackSprite;

            Pikimin tempPiki = pikimin;

            activePlayerPikimin = tempPiki;
        }

        /// <summary>
        /// Creates Npcs Pikimin.
        /// </summary>
        /// <param name="npcPikimin"></param>
        private void CreateNpcPikimin(Pikimin npcPikimin)
        {
            enemyBattlePikimin = Instantiate(emptyPikimin, enemyPodium.transform.position, Quaternion.identity);

            Vector3 pikiLocalPos = new Vector3(0, 1, 0);

            enemyBattlePikimin.transform.parent = enemyPodium;
            enemyBattlePikimin.transform.localPosition = pikiLocalPos;
        
            enemyBattlePikimin.GetComponent<SpriteRenderer>().sprite = npcPikimin.FrontSprite;

            Pikimin tempPiki = npcPikimin;
            activeEnemyPikimin = tempPiki;
        }

        /// <summary>
        /// Instantiate Random Pikimin form List by rarity on enemyPodium.
        /// </summary>
        /// <param name="rarity">Rarity</param>
        /// <param name="biome">BiomeList</param>
        /// <param name="maxLvl"></param>
        /// <param name="regions"></param>
        /// <param name="minLvl"></param>
        /// <returns>Pikimin randomPikiminFromList</returns>
        private void CreatePikimin(Rarity rarity, BiomeList biome, PikiminRegions regions, int minLvl, int maxLvl)
        {
            var tempPikimin2 = GetPikiminByRarity(rarity, biome, regions);
            Pikimin randomPikiminFromList;

            if (!tempPikimin2.Any())
            {
                var tempRarity = rarity;
            
                while (!tempPikimin2.Any())
                {
                    switch (tempRarity)
                    {
                        case Rarity.VeryCommon:
                            tempRarity = Rarity.VeryRare;
                            break;
                        case Rarity.Common:
                            tempRarity = Rarity.VeryCommon;
                            break;
                        case Rarity.Uncommon:
                            tempRarity = Rarity.Common;
                            break;
                        case Rarity.Rare:
                            tempRarity = Rarity.Uncommon;
                            break;
                        case Rarity.VeryRare:
                            tempRarity = Rarity.Rare;
                            break;
                    }
                    tempPikimin2 = GetPikiminByRarity(tempRarity, biome, regions);
                }
                randomPikiminFromList = GetRandomPikiminFromList(tempPikimin2);
            }
            else
            {
                randomPikiminFromList = GetRandomPikiminFromList(tempPikimin2);
            }

            var tempPikimin = randomPikiminFromList;

            activeEnemyPikimin = Instantiate(tempPikimin, enemyPodium.transform.position, Quaternion.identity);
        
            var myTransform = activeEnemyPikimin.transform;
        
            activeEnemyPikimin.AddMember(tempPikimin);
        
            var tempLvl = Random.Range(minLvl, maxLvl);
            activeEnemyPikimin.ChangeLevel(tempLvl);

            activeEnemyPikimin.IsWild = true;
        
            var pikiLocalPos = new Vector3(0, 1, 0);
        
            myTransform.parent = enemyPodium;
            myTransform.localPosition = pikiLocalPos;
        }
    
        private void CreatePikimin(Pikimin legendary)
        {

            activeEnemyPikimin = Instantiate(legendary, enemyPodium.transform.position, Quaternion.identity);
        
            var myTransform = activeEnemyPikimin.transform;
        
            activeEnemyPikimin.AddMember(legendary);
        
            activeEnemyPikimin.ChangeLevel(60);

            activeEnemyPikimin.IsWild = true;
        
            var pikiLocalPos = new Vector3(0, 1, 0);
        
            myTransform.parent = enemyPodium;
            myTransform.localPosition = pikiLocalPos;
        }

        /// <summary>
        /// Creates list of pikimin with chosen rarity.
        /// </summary>
        /// <param name="rarity">Rarity</param>
        /// <param name="biome">BiomeList</param>
        /// <param name="regions"></param>
        /// <returns>List of Pikimin</returns>
        private List<Pikimin> GetPikiminByRarity(Rarity rarity, BiomeList biome, PikiminRegions regions)
        {
            var wildPikimin = new List<Pikimin>();
            for (int i = 0; i < allPikimin.Count; i++)
            {
                for (int j = 0; j < allPikimin[i].Regions.Length; j++)
                {
                    if (allPikimin[i].Regions[j] == regions)
                    {
                        wildPikimin.Add(allPikimin[i]);
                        break;
                    }
                }
            }
            var pikiList = wildPikimin.Where(pikimin => pikimin.MyRarity == rarity && pikimin.BiomeFound == biome).ToList();
            return pikiList;
        }

        /// <summary>
        /// Returns a random Pikimin.
        /// </summary>
        /// <param name="pikiList">Rarity</param>
        /// <returns>Pikimin randomPikimin</returns>
        private static Pikimin GetRandomPikiminFromList(IReadOnlyList<Pikimin> pikiList)
        {
            var pikiIndex = Random.Range(0, pikiList.Count);

            var randomPikimin = pikiList[pikiIndex];
            return randomPikimin;
        }
        #endregion

        #region DamageCalculation
        /// <summary>
        /// Calculates the modifier.
        /// </summary>
        /// <param name="isCrit"></param>
        /// <param name="isEffective">true/null/false</param>
        /// <returns>float modifier</returns>
        private static float ModifierCalculation(bool isCrit,bool? isEffective)
        {
            var criticalMultiplier = isCrit ? 2f : 1f;
            float effectivityMultiplier;
            
            switch (isEffective)
            {
                case true:
                    effectivityMultiplier = 2f;
                    break;
                case null:
                    effectivityMultiplier = 1f;
                    break;
                case false:
                    effectivityMultiplier = 0.5f;
                    break;
                default:
                    effectivityMultiplier = 0f;
                    break;
            }

            var modifier = Random.Range(.85f, 1) * criticalMultiplier * effectivityMultiplier;

            return modifier;
        }


        /// <summary>
        /// Calculates damage.
        /// </summary>
        /// <param name="power">Attacks Power</param>
        /// <param name="attack">Attack Stat Attacker</param>
        /// <param name="defense">Defense Stat Defender</param>
        /// <param name="level">int level</param>
        /// <param name="isCrit">bool</param>
        /// <param name="isEffective">bool?</param>
        /// <returns>Damage in Hp done.</returns>
        private int DamageCalculation(int power, int attack, int defense,int level, bool isCrit, bool? isEffective)
        {
            var damage = (((2 * level / 5 + 2) * power * attack / defense) / 50 + 2) * ModifierCalculation(isCrit, isEffective);

            return Convert.ToInt32(damage);
        }

        /// <summary>
        /// Check Effectivity.
        /// </summary>
        /// <returns>bool?</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private bool? CheckForEffectivity(Moves move)
        {
            if (BattleState == BattleState.PlayerTurn)
            {
                switch (move.MoveType)
                {
                    case PikiminType.Fire when activeEnemyPikimin.PikiType == PikiminType.Grass ||
                                               activeEnemyPikimin.PikiType == PikiminType.Ice ||
                                               activeEnemyPikimin.PikiType == PikiminType.Bug:
                    case PikiminType.Water when activeEnemyPikimin.PikiType == PikiminType.Fire ||
                                                activeEnemyPikimin.PikiType == PikiminType.Ground ||
                                                activeEnemyPikimin.PikiType == PikiminType.Rock:
                    case PikiminType.Electric when activeEnemyPikimin.PikiType == PikiminType.Water ||
                                                   activeEnemyPikimin.PikiType == PikiminType.Flying:
                    case PikiminType.Grass when activeEnemyPikimin.PikiType == PikiminType.Water ||
                                                activeEnemyPikimin.PikiType == PikiminType.Ground ||
                                                activeEnemyPikimin.PikiType == PikiminType.Rock:
                    case PikiminType.Ice when activeEnemyPikimin.PikiType == PikiminType.Grass ||
                                              activeEnemyPikimin.PikiType == PikiminType.Ground ||
                                              activeEnemyPikimin.PikiType == PikiminType.Flying ||
                                              activeEnemyPikimin.PikiType == PikiminType.Dragon:
                    case PikiminType.Fighting when activeEnemyPikimin.PikiType == PikiminType.Normal ||
                                                   activeEnemyPikimin.PikiType == PikiminType.Ice ||
                                                   activeEnemyPikimin.PikiType == PikiminType.Rock:
                    case PikiminType.Poison when activeEnemyPikimin.PikiType == PikiminType.Grass:
                    case PikiminType.Ground when activeEnemyPikimin.PikiType == PikiminType.Fire ||
                                                 activeEnemyPikimin.PikiType == PikiminType.Electric ||
                                                 activeEnemyPikimin.PikiType == PikiminType.Poison ||
                                                 activeEnemyPikimin.PikiType == PikiminType.Rock:
                    case PikiminType.Flying when activeEnemyPikimin.PikiType == PikiminType.Grass ||
                                                 activeEnemyPikimin.PikiType == PikiminType.Fighting ||
                                                 activeEnemyPikimin.PikiType == PikiminType.Bug:
                    case PikiminType.Psychic when activeEnemyPikimin.PikiType == PikiminType.Fighting || 
                                                  activeEnemyPikimin.PikiType == PikiminType.Poison:
                    case PikiminType.Bug when activeEnemyPikimin.PikiType == PikiminType.Grass ||
                                              activeEnemyPikimin.PikiType == PikiminType.Psychic:
                    case PikiminType.Rock when activeEnemyPikimin.PikiType == PikiminType.Fire ||
                                               activeEnemyPikimin.PikiType == PikiminType.Ice ||
                                               activeEnemyPikimin.PikiType == PikiminType.Flying ||
                                               activeEnemyPikimin.PikiType == PikiminType.Bug:
                    case PikiminType.Ghost when activeEnemyPikimin.PikiType == PikiminType.Psychic ||
                                                activeEnemyPikimin.PikiType == PikiminType.Ghost:
                    case PikiminType.Dragon when activeEnemyPikimin.PikiType == PikiminType.Dragon:
                        return true;
                    case PikiminType.Normal when activeEnemyPikimin.PikiType == PikiminType.Rock: 
                    case PikiminType.Fire when activeEnemyPikimin.PikiType == PikiminType.Fire ||
                                               activeEnemyPikimin.PikiType == PikiminType.Water ||
                                               activeEnemyPikimin.PikiType == PikiminType.Rock ||
                                               activeEnemyPikimin.PikiType == PikiminType.Dragon:
                    case PikiminType.Water when activeEnemyPikimin.PikiType == PikiminType.Water ||
                                                activeEnemyPikimin.PikiType == PikiminType.Grass ||
                                                activeEnemyPikimin.PikiType == PikiminType.Dragon:
                    case PikiminType.Electric when activeEnemyPikimin.PikiType == PikiminType.Electric ||
                                                   activeEnemyPikimin.PikiType == PikiminType.Grass ||
                                                   activeEnemyPikimin.PikiType == PikiminType.Ground ||
                                                   activeEnemyPikimin.PikiType == PikiminType.Dragon:
                    case PikiminType.Grass when activeEnemyPikimin.PikiType == PikiminType.Fire ||
                                                activeEnemyPikimin.PikiType == PikiminType.Grass ||
                                                activeEnemyPikimin.PikiType == PikiminType.Poison ||
                                                activeEnemyPikimin.PikiType == PikiminType.Flying ||
                                                activeEnemyPikimin.PikiType == PikiminType.Bug ||
                                                activeEnemyPikimin.PikiType == PikiminType.Dragon:
                    case PikiminType.Ice when activeEnemyPikimin.PikiType == PikiminType.Water ||
                                              activeEnemyPikimin.PikiType == PikiminType.Ice:
                    case PikiminType.Fighting when activeEnemyPikimin.PikiType == PikiminType.Poison ||
                                                   activeEnemyPikimin.PikiType == PikiminType.Flying ||
                                                   activeEnemyPikimin.PikiType == PikiminType.Psychic ||
                                                   activeEnemyPikimin.PikiType == PikiminType.Bug ||
                                                   activeEnemyPikimin.PikiType == PikiminType.Ghost:
                    case PikiminType.Poison when activeEnemyPikimin.PikiType == PikiminType.Poison ||
                                                 activeEnemyPikimin.PikiType == PikiminType.Ground ||
                                                 activeEnemyPikimin.PikiType == PikiminType.Rock ||
                                                 activeEnemyPikimin.PikiType == PikiminType.Ghost:
                    case PikiminType.Ground when activeEnemyPikimin.PikiType == PikiminType.Grass ||
                                                 activeEnemyPikimin.PikiType == PikiminType.Flying ||
                                                 activeEnemyPikimin.PikiType == PikiminType.Bug:
                    case PikiminType.Flying when activeEnemyPikimin.PikiType == PikiminType.Electric ||
                                                 activeEnemyPikimin.PikiType == PikiminType.Rock:
                    case PikiminType.Psychic when activeEnemyPikimin.PikiType == PikiminType.Psychic:
                    case PikiminType.Bug when activeEnemyPikimin.PikiType == PikiminType.Fire ||
                                              activeEnemyPikimin.PikiType == PikiminType.Fighting ||
                                              activeEnemyPikimin.PikiType == PikiminType.Flying ||
                                              activeEnemyPikimin.PikiType == PikiminType.Ghost:
                    case PikiminType.Rock when activeEnemyPikimin.PikiType == PikiminType.Fighting ||
                                               activeEnemyPikimin.PikiType == PikiminType.Ground:
                    case PikiminType.Ghost when activeEnemyPikimin.PikiType == PikiminType.Normal ||
                                                activeEnemyPikimin.PikiType == PikiminType.Psychic:
                        return false;
                    default:
                        return null;
                }
            }

            if (BattleState == BattleState.EnemyTurn)
            {
                switch (move.MoveType)
                {
                    case PikiminType.Fire when activePlayerPikimin.PikiType == PikiminType.Grass || 
                                               activePlayerPikimin.PikiType == PikiminType.Ice ||
                                               activePlayerPikimin.PikiType == PikiminType.Bug:
                    case PikiminType.Water when activePlayerPikimin.PikiType == PikiminType.Fire || 
                                                activePlayerPikimin.PikiType == PikiminType.Ground || 
                                                activePlayerPikimin.PikiType == PikiminType.Rock:
                    case PikiminType.Electric when activePlayerPikimin.PikiType == PikiminType.Water || 
                                                   activePlayerPikimin.PikiType == PikiminType.Flying:
                    case PikiminType.Grass when activePlayerPikimin.PikiType == PikiminType.Water || 
                                                activePlayerPikimin.PikiType == PikiminType.Ground || 
                                                activePlayerPikimin.PikiType == PikiminType.Rock:
                    case PikiminType.Ice when activePlayerPikimin.PikiType == PikiminType.Grass ||
                                              activePlayerPikimin.PikiType == PikiminType.Ground ||
                                              activePlayerPikimin.PikiType == PikiminType.Flying ||
                                              activePlayerPikimin.PikiType == PikiminType.Dragon:
                    case PikiminType.Fighting when activePlayerPikimin.PikiType == PikiminType.Normal ||
                                                   activePlayerPikimin.PikiType == PikiminType.Ice ||
                                                   activePlayerPikimin.PikiType == PikiminType.Rock:
                    case PikiminType.Poison when activePlayerPikimin.PikiType == PikiminType.Grass:
                    case PikiminType.Ground when activePlayerPikimin.PikiType == PikiminType.Fire ||
                                                 activePlayerPikimin.PikiType == PikiminType.Electric ||
                                                 activePlayerPikimin.PikiType == PikiminType.Poison ||
                                                 activePlayerPikimin.PikiType == PikiminType.Rock:
                    case PikiminType.Flying when activePlayerPikimin.PikiType == PikiminType.Grass ||
                                                 activePlayerPikimin.PikiType == PikiminType.Fighting ||
                                                 activePlayerPikimin.PikiType == PikiminType.Bug:
                    case PikiminType.Psychic when activePlayerPikimin.PikiType == PikiminType.Fighting || 
                                                  activePlayerPikimin.PikiType == PikiminType.Poison:
                    case PikiminType.Bug when activePlayerPikimin.PikiType == PikiminType.Grass || 
                                              activePlayerPikimin.PikiType == PikiminType.Psychic:
                    case PikiminType.Rock when activePlayerPikimin.PikiType == PikiminType.Fire ||
                                               activePlayerPikimin.PikiType == PikiminType.Ice ||
                                               activePlayerPikimin.PikiType == PikiminType.Flying ||
                                               activePlayerPikimin.PikiType == PikiminType.Bug:
                    case PikiminType.Ghost when activePlayerPikimin.PikiType == PikiminType.Psychic ||
                                                activePlayerPikimin.PikiType == PikiminType.Ghost:
                    case PikiminType.Dragon when activePlayerPikimin.PikiType == PikiminType.Dragon:
                        return true;
                    case PikiminType.Normal when activePlayerPikimin.PikiType == PikiminType.Rock: 
                    case PikiminType.Fire when activePlayerPikimin.PikiType == PikiminType.Fire || 
                                               activePlayerPikimin.PikiType == PikiminType.Water ||
                                               activePlayerPikimin.PikiType == PikiminType.Rock || 
                                               activePlayerPikimin.PikiType == PikiminType.Dragon:
                    case PikiminType.Water when activePlayerPikimin.PikiType == PikiminType.Water || 
                                                activePlayerPikimin.PikiType == PikiminType.Grass || 
                                                activePlayerPikimin.PikiType == PikiminType.Dragon:
                    case PikiminType.Electric when activePlayerPikimin.PikiType == PikiminType.Electric ||
                                                   activePlayerPikimin.PikiType == PikiminType.Grass ||
                                                   activePlayerPikimin.PikiType == PikiminType.Ground || 
                                                   activePlayerPikimin.PikiType == PikiminType.Dragon:
                    case PikiminType.Grass when activePlayerPikimin.PikiType == PikiminType.Fire || 
                                                activePlayerPikimin.PikiType == PikiminType.Poison ||
                                                activePlayerPikimin.PikiType == PikiminType.Flying ||
                                                activePlayerPikimin.PikiType == PikiminType.Bug ||
                                                activePlayerPikimin.PikiType == PikiminType.Dragon:
                    case PikiminType.Ice when activePlayerPikimin.PikiType == PikiminType.Water ||
                                              activePlayerPikimin.PikiType == PikiminType.Ice:
                    case PikiminType.Fighting when activePlayerPikimin.PikiType == PikiminType.Poison || 
                                                   activePlayerPikimin.PikiType == PikiminType.Flying || 
                                                   activePlayerPikimin.PikiType == PikiminType.Psychic || 
                                                   activePlayerPikimin.PikiType == PikiminType.Bug || 
                                                   activePlayerPikimin.PikiType == PikiminType.Ghost:
                    case PikiminType.Poison when activePlayerPikimin.PikiType == PikiminType.Poison ||
                                                 activePlayerPikimin.PikiType == PikiminType.Ground ||
                                                 activePlayerPikimin.PikiType == PikiminType.Rock ||
                                                 activePlayerPikimin.PikiType == PikiminType.Ghost:
                    case PikiminType.Ground when activePlayerPikimin.PikiType == PikiminType.Grass || 
                                                 activePlayerPikimin.PikiType == PikiminType.Flying ||
                                                 activePlayerPikimin.PikiType == PikiminType.Bug:
                    case PikiminType.Flying when activePlayerPikimin.PikiType == PikiminType.Electric || 
                                                 activePlayerPikimin.PikiType == PikiminType.Rock:
                    case PikiminType.Psychic when activePlayerPikimin.PikiType == PikiminType.Psychic:
                    case PikiminType.Bug when activePlayerPikimin.PikiType == PikiminType.Fire || 
                                              activePlayerPikimin.PikiType == PikiminType.Fighting ||
                                              activePlayerPikimin.PikiType == PikiminType.Flying ||
                                              activePlayerPikimin.PikiType == PikiminType.Ghost:
                    case PikiminType.Rock when activePlayerPikimin.PikiType == PikiminType.Fighting ||
                                               activePlayerPikimin.PikiType == PikiminType.Ground:
                    case PikiminType.Ghost when activePlayerPikimin.PikiType == PikiminType.Normal ||
                                                activePlayerPikimin.PikiType == PikiminType.Psychic:
                        return false;
                    default:
                        return null;
                }
            }
            return null;
        }

        /// <summary>
        /// calculates if hit is a crit or not
        /// </summary>
        /// <param name="defenderSpeed">1st</param>
        /// <param name="attackerSpeed">2nd</param>
        /// <returns>bool</returns>
        private static bool CalculateCrit(float defenderSpeed, float attackerSpeed)
        {
            if (defenderSpeed > attackerSpeed)
            {
                return Random.Range(0f,100f) > 97.5f;
            }

            if (attackerSpeed > defenderSpeed)
            {
                return Random.Range(0f,100f) > 95;
            }

            return false;
        }
        #endregion
    
        #region Tools
        /// <summary>
        /// Changes battle and normal camera.
        /// </summary>
        private void ChangeCamera()
        {
            if (BattleState == BattleState.Start)
            {
                playerCamera.SetActive(false);
                battleCamera.SetActive(true);
            }
            else if (BattleState == BattleState.Idle || BattleState == BattleState.Won || BattleState == BattleState.Lost)
            {
                playerCamera.SetActive(true);
                battleCamera.SetActive(false);
            }
        }

        private void StartBattleAnimation(Pikimin legendary, TrainerBehaviour newNpc, Rarity rarity, BiomeList biome, int minLvl, int maxLvl, AudioClip battleStartClip)
        {
            try
            {
                CurrentPlayer.ChangeState(PlayerState.Fight);

                if (!CurrentPlayer.IsSurfing)
                {
                    CurrentPlayerController.ResetAnimations();
                }

                if (battleStartAnimation == null) return;

                if (battleStartClip != null)
                {
                    if (newNpc != null && newNpc.rivalChapterSound)
                    {
                        AudioManagerInstance.BackgroundSound.UnPause();
                    }
                    else
                    {
                        AudioManagerInstance.PlayAudioClip(false, battleStartClip);
                    }
                }

                var panel = Instantiate(battleStartAnimation, transform.position, Quaternion.identity);
                Destroy(panel, 3f);

                StartCoroutine(MyCo(legendary, newNpc, rarity, biome, minLvl, maxLvl));
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                
                throw;
            }
        }

        private IEnumerator MyCo(Pikimin legendary, TrainerBehaviour newNpc,Rarity rarity, BiomeList biome, int minLvl, int maxLvl)
        {
            yield return new WaitForSeconds(1.5f);
            EnterBattle(legendary, newNpc, rarity, biome, minLvl, maxLvl);
        }

        private IEnumerator MoveAnimation(Moves move, SpriteRenderer spriteRenderer, Animator animator, Transform newTransform)
        {
            spriteRenderer.sprite = move.Sprite;
            Vector3 temp = newTransform.position;
        
            AudioManagerInstance.PlayAudioClip(AudioManagerInstance.Sfx, move.Clip);

            if (battleState == BattleState.PlayerTurn)
            {
                animator.runtimeAnimatorController = move.AnimatorControllerPlayer;
            }
            else if (battleState == BattleState.EnemyTurn)
            {
                animator.runtimeAnimatorController = move.AnimatorControllerEnemy;
            }
        
            spriteRenderer.enabled = true;
            animator.enabled = true;

            if (battleState == BattleState.PlayerTurn)
            {
                yield return new WaitForSeconds(move.AnimationClipPlayer.length);
            }
            else if (battleState == BattleState.EnemyTurn)
            {
                yield return new WaitForSeconds(move.AnimationClipEnemy.length);
            }

            if (battleState == BattleState.PlayerTurn)
            {
                animator.runtimeAnimatorController = defaultPlayerAnim;
            }
            else if (battleState == BattleState.EnemyTurn)
            {
                animator.runtimeAnimatorController = defaultEnemyAnim;
            }

            AudioManagerInstance.Sfx.clip = null;
            spriteRenderer.sprite = null;
            newTransform.position = temp;
            animator.enabled = false;
            spriteRenderer.enabled = false;
        }
    
        private IEnumerator PikicubeAnimation(Pikicube cube, SpriteRenderer spriteRenderer, Animator animator, Transform newTransform)
        {
            spriteRenderer.sprite = cube.spriteRenderer.sprite;
            Vector3 temp = newTransform.position;

            animator.runtimeAnimatorController = cube.AnimatorControllerPikicube;

            spriteRenderer.enabled = true;
            animator.enabled = true;

            if (wildPikiminGotCaught)
            {
                animator.SetBool(Caught, true);
                activeEnemyPikimin.GetComponent<SpriteRenderer>().enabled = false;
                yield return new WaitForSeconds(cube.caught.length);
            }
            else if (wildPikiminBrokeOut)
            {
                animator.SetBool(Caught, false);
                activeEnemyPikimin.GetComponent<SpriteRenderer>().enabled = false;
                yield return new WaitForSeconds(cube.failedCatch.length);
                activeEnemyPikimin.GetComponent<SpriteRenderer>().enabled = true;
            
            }

            if (battleState == BattleState.PlayerTurn)
            {
                animator.runtimeAnimatorController = defaultPlayerAnim;
            }
        
            pikicubeHelper = true;
            spriteRenderer.sprite = null;
            newTransform.position = temp;
            animator.enabled = false;
            spriteRenderer.enabled = false;
        }

        public void CreatePlayersFirstPikimin(Pikimin pikimin, int lvl)
        {
            playerPikiminInventory.OwnedPikimin[0] = BattleManagerInstance.InstantiatePikimin(pikimin, playerPikiminInventory.transform);
            playerPikiminInventory.OwnedPikimin[0].gameObject.GetComponent<SpriteRenderer>().enabled = false;
            playerPikiminInventory.OwnedPikimin[0].ChangeLevel(lvl);
            playerPikiminInventory.OwnedPikimin[0].SetIvs(Random.Range(7,15),Random.Range(7,15),Random.Range(7,15),Random.Range(7,15));
            playerPikiminInventory.OwnedPikimin[0].IsPlayerPikimin = true;
            playerPikiminInventory.pikiminAmount++;
            PikidexInstance.CaughtPikiminUpdater(playerPikiminInventory.OwnedPikimin[0]);
        }
        #endregion
    }
}

