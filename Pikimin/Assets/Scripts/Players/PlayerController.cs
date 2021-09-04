using System.Linq;
using Items;
using Pikimins;
using SaveData;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Audio.AudioManager;
using static Players.Player;
using static Battle.BattleManager;
using static Map.Regions;
using static Players.ItemInventory;
using static SaveData.SaveManager;
using static Triggers.LongGrass;
using Random = UnityEngine.Random;

namespace Players
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Instance")] 
        public static PlayerController CurrentPlayerController;
        
        [Header("Movement")]
        public Vector2 targetPos;
        public Vector2 oldPos;
        [SerializeField]private Vector2 direction;
        private float lerpTime;
        private float speed;
        [SerializeField]private float runSpeed;
        [SerializeField]private float walkSpeed;
        [SerializeField]private float bikeSpeed;
        private bool running;
        [SerializeField] private bool isTeleporting;

        [Header("Components")]
        [SerializeField] private GameObject menu;
        [SerializeField] private GameObject settings;
        [SerializeField] private Vector2 change;
        public Animator animator;

        [Header("Interacting")]
        [SerializeField] private GameObject[] interactionBoxes;
        [SerializeField] public bool? LooksUp;
        [SerializeField] public bool? LooksRight;
        [SerializeField] private bool triedInteracting;
        [SerializeField] private float activeTime;
        
        [Header("Cutting")]
        public GameObject[] cuttingBoxed;
        public bool triedCutting;

        [Header("Surfing")] 
        public GameObject[] surfingBoxes;
        public bool triedSurfing;

        [Header("Items")] 
        [SerializeField] private Item bicycle;
        [SerializeField] public bool foundItem;
        
        [Header("Fishing")]
        [SerializeField] private Item fishingRod;
        public GameObject[] fishingBoxes;
        public bool triedFishing;
        public bool fishing;
        public float fishingTime;
        
        [Header("Tilemap")]
        public Tilemap groundTilemap;
        public Tilemap colliderTilemap;
        public Tilemap dekoColliderTilemap;
        public Tilemap cutTreeTilemap;
        public Tilemap surfTilemap;

        // Animation Index.
        private static readonly int MoveX = Animator.StringToHash("moveX");
        private static readonly int MoveY = Animator.StringToHash("moveY");
        private static readonly int IsWalking = Animator.StringToHash("isWalking");
        private static readonly int IsRunning = Animator.StringToHash("isRunning");
        private static readonly int IsSurfing = Animator.StringToHash("isSurfing");
        private static readonly int IsDriving = Animator.StringToHash("isDriving");
        private static readonly int IsOnBike = Animator.StringToHash("isOnBike");
        private static readonly int IsFlying = Animator.StringToHash("isFlying");
        private static readonly int IsFishing = Animator.StringToHash("isFishing");
        private static readonly int FoundItem = Animator.StringToHash("FoundItem");

        public bool isIndoors;
        
        public Vector2 Direction => direction;

        private void Awake()
        {
            CurrentPlayerController = this;
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            LoadData.LoadDataInstance.OnLoad += LoadPlayerControllerData;
            speed = walkSpeed;
        }

        // Update is called once per frame
        private void Update()
        {
            if (triedInteracting)
            {
                activeTime -= Time.deltaTime;
                if(activeTime < 0)
                {
                    triedInteracting = false;
                    activeTime = 0.2f;
                    for (int i = 0; i < interactionBoxes.Length; i++)
                    {
                        interactionBoxes[i].SetActive(false);
                    }
                }
            }

            if (triedCutting)
            {
                activeTime -= Time.deltaTime;
                   
                if(activeTime < 0)
                {
                    triedCutting = false;
                    activeTime = .2f;
                    for (int i = 0; i < cuttingBoxed.Length; i++)
                    {
                        cuttingBoxed[i].SetActive(false);
                    }
                }
            }

            if (triedSurfing)
            {
                activeTime -= Time.deltaTime;
                if(activeTime < 0)
                {
                    triedSurfing = false;
                    activeTime = .2f;
                    for (int i = 0; i < surfingBoxes.Length; i++)
                    {
                        surfingBoxes[i].SetActive(false);
                    }
                }
            }
            
            if (triedFishing)
            {
                activeTime -= Time.deltaTime;
                if(activeTime < 0)
                {
                    triedFishing = false;
                    activeTime = .2f;
                    for (int i = 0; i < fishingBoxes.Length; i++)
                    {
                        fishingBoxes[i].SetActive(false);
                    }
                }
            }

            if (fishing)
            {
                fishingTime -= Time.deltaTime;
                if(fishingTime < 0)
                {
                    fishing = false;
                    fishingTime = 3f;
                    ResetAnimations();
                    Fishing();
                }
            }

            if (CurrentPlayer.CurrentState != PlayerState.Fight && CurrentPlayer.CurrentState != PlayerState.Interact)
            {
                InputCheck();
            }
        }

        /// <summary>
        /// Checks for input.
        /// </summary>
        private void InputCheck()
        {
            // Opens the Menu.
            if((Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Tab)) && CurrentPlayer.CurrentState == PlayerState.Idle)
            {
                CurrentPlayer.ChangeState(PlayerState.Interact);
                BattleManagerInstance.BattleCamera.SetActive(false);
                menu.SetActive(true);
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && CurrentPlayer.CurrentState == PlayerState.Idle)
            {
                CurrentPlayer.ChangeState(PlayerState.Interact);
                BattleManagerInstance.BattleCamera.SetActive(false);
                settings.SetActive(true);
            }

            // Interacting.
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E))
            {
                if (CurrentPlayer.CurrentState == PlayerState.Idle)
                {
                    triedInteracting = true;
                    if (LooksRight == true)
                    {
                        interactionBoxes[1].SetActive(true);
                    }
                    else if (LooksRight == false)
                    {
                        interactionBoxes[3].SetActive(true);
                    }
                    else if (LooksUp == true)
                    {
                        interactionBoxes[0].SetActive(true);
                    }
                    else if (LooksUp == false)
                    {
                        interactionBoxes[2].SetActive(true);
                    }
                }
            }

            //Direction
            change = Vector2.zero;
            if (Input.GetAxisRaw("Vertical") != 0  && (CurrentPlayer.CurrentState != PlayerState.Fight || CurrentPlayer.CurrentState != PlayerState.Interact))
            {
                change.y = Input.GetAxisRaw("Vertical");
            }
            else if (Input.GetAxisRaw("Horizontal") != 0 && (CurrentPlayer.CurrentState != PlayerState.Fight || CurrentPlayer.CurrentState != PlayerState.Interact))
            {
                change.x = Input.GetAxisRaw("Horizontal");
            }

            //Running
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                running = true;
            }
            else
            {
                running = false;
            }
            
            //Bike
            if (playerItems.OwnedItems.Contains(bicycle) && !isIndoors)
            {
                if (Input.GetKeyDown(KeyCode.X) && (CurrentPlayer.CurrentState != PlayerState.Fight || CurrentPlayer.CurrentState != PlayerState.Interact || CurrentPlayer.CurrentState != PlayerState.Walk))
                {
                    CurrentPlayer.SwapBikeState();
                }
            }
            
            //Rod
            if (playerItems.OwnedItems.Contains(fishingRod) && CurrentPlayer.CurrentState == PlayerState.Idle)
            {
                if (Input.GetKeyDown(KeyCode.Y) && (CurrentPlayer.CurrentState != PlayerState.Fight || CurrentPlayer.CurrentState != PlayerState.Interact))
                {
                    fishingRod.OnUse();
                }
            }
            UpdateAnimation();
        }

        /// <summary>
        /// Makes character move.
        /// </summary>
        private void MoveCharacter()
        {
            if(CurrentPlayer.CurrentState == PlayerState.Walk)
            {
                lerpTime += Time.deltaTime * speed;
                transform.position = Vector2.Lerp(oldPos, targetPos, lerpTime);
                if ((Vector2)transform.position == targetPos)
                {
                    lerpTime = 0;
                    CurrentPlayer.ChangeState(PlayerState.Idle);
                }
            }
        }

        /// <summary>
        /// Returns if can Move.
        /// </summary>
        /// <param name="newDirection"></param>
        /// <returns></returns>
        private bool CanMove(Vector2 newDirection)
        {
            Vector3Int gridPos = groundTilemap.WorldToCell(transform.position + (Vector3) newDirection);

            if (colliderTilemap.HasTile(gridPos) || !groundTilemap.HasTile(gridPos) || dekoColliderTilemap.HasTile(gridPos) || cutTreeTilemap.HasTile(gridPos) || surfTilemap.HasTile(gridPos))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Dels next Tile.
        /// </summary>
        public void CutTree()
        {
            Vector3Int gridPos = groundTilemap.WorldToCell(transform.position + (Vector3) direction);
            
            cutTreeTilemap.SetTile(gridPos,null);
        }

        /// <summary>
        /// Makes Player go onto Water.
        /// </summary>
        public void Surf()
        {
            if (direction != Vector2.zero)
            {
                transform.position = targetPos + direction;
            
                CurrentPlayer.SwapSurfState();
                if (CurrentPlayer.UseBike)
                {
                    CurrentPlayer.SwapBikeState();
                }
            }
        }

        public void Fish()
        {
            if (!CurrentPlayer.IsSurfing)
            {
                fishing = true;
            }
        }

        private void Fishing()
        {
            var encounterChance = Random.Range(0.0f, 100.0f);

            if (encounterChance <= 33)
            {
                RegionsInstance.PikiminLvlAtRegion(CurrentPlayer.PlayerRegion, out var minLvl, out var maxLvl);
                Pikimin.Rarity rarity = CalculateRarity(false);
                if (rarity != Pikimin.Rarity.Npc)
                {
                    BattleManagerInstance.OnBattleStart?.Invoke(null,null,rarity, BiomeList.Water, minLvl, maxLvl, AudioManagerInstance.TrainerSounds[1]);
                }
            } 
        }

        public void TeleportPlayer(Vector2 newPos)
        {
            isTeleporting = true;
            transform.position = newPos;
            CurrentPlayer.ChangeState(PlayerState.Idle);
        }

        /// <summary>
        /// Set Animation bool.
        /// </summary>
        private void UpdateAnimation()
        {
            if (change.y != 0 || change.x != 0)
            {
                if (CurrentPlayer.CurrentState == PlayerState.Idle)
                {
                    var position = transform.position;
                    oldPos = position;
                    targetPos = (Vector2)position + change;
                    direction = targetPos - oldPos;
                }
                
                if (CanMove(change) && !isTeleporting)
                {
                    CurrentPlayer.ChangeState(PlayerState.Walk);
                }
                
                animator.SetFloat(MoveX, direction.x);
                animator.SetFloat(MoveY, direction.y);
                
                switch (direction.x)
                {
                    case 1:
                        LooksRight = true;
                        LooksUp = null;
                        break;
                    case -1:
                        LooksUp = null;
                        LooksRight = false;
                        break;
                    default:
                    {
                        switch (direction.y)
                        {
                            case 1:
                                LooksUp = true;
                                LooksRight = null;
                                break;
                            case -1:
                                LooksRight = null;
                                LooksUp = false;
                                break;
                        }
                        break;
                    }
                }
            }
            else
            {
                isTeleporting = false;
            }
            
            if (CurrentPlayer.CurrentState == PlayerState.Walk)
            {
                switch (running)
                {
                    case false when !CurrentPlayer.UseBike && !CurrentPlayer.IsSurfing:
                        animator.SetBool(IsRunning, false);
                        animator.SetBool(IsDriving,false);
                        animator.SetBool(IsOnBike,false);
                        animator.SetBool(FoundItem, false);
                        animator.SetBool(IsSurfing, false);
                        animator.SetBool(IsFishing, false);

                        animator.SetBool(IsWalking, true);
                        speed = walkSpeed;
                        break;
                    case true when !CurrentPlayer.UseBike && !CurrentPlayer.IsSurfing:
                        animator.SetBool(IsWalking,false);
                        animator.SetBool(IsDriving,false);
                        animator.SetBool(IsOnBike,false);
                        animator.SetBool(FoundItem, false);
                        animator.SetBool(IsSurfing, false);
                        animator.SetBool(IsFishing, false);

                        animator.SetBool(IsRunning, true);
                        speed = runSpeed;
                        break;
                    default:
                    {
                        if (CurrentPlayer.UseBike && !CurrentPlayer.IsSurfing && !running)
                        {
                            animator.SetBool(IsWalking,false);
                            animator.SetBool(IsRunning, false);
                            animator.SetBool(FoundItem, false);
                            animator.SetBool(IsSurfing, false);
                            animator.SetBool(IsFishing, false);

                            animator.SetBool(IsOnBike, true);
                            animator.SetBool(IsDriving, true);
                            speed = bikeSpeed;
                        }
                        else if (CurrentPlayer.IsSurfing && !running && !CurrentPlayer.UseBike)
                        {
                            animator.SetBool(IsRunning, false);
                            animator.SetBool(IsWalking, false);
                            animator.SetBool(IsDriving, false);
                            animator.SetBool(FoundItem, false);
                            animator.SetBool(IsOnBike, false);
                            animator.SetBool(IsFishing, false);
                    
                            animator.SetBool(IsSurfing, true);
                            speed = runSpeed;
                        }

                        break;
                    }
                }
            }
            else if (foundItem)
            {
                animator.SetBool(IsWalking,false);
                animator.SetBool(IsRunning, false);
                animator.SetBool(IsOnBike, false);
                animator.SetBool(IsDriving, false);
                animator.SetBool(IsSurfing, false);
                animator.SetBool(IsFishing, false);

                animator.SetBool(FoundItem, true);
                foundItem = false;
            }
            else if (fishing)
            {
                animator.SetBool(IsRunning, false);
                animator.SetBool(IsWalking, false);
                animator.SetBool(IsDriving,false);
                animator.SetBool(IsOnBike,false);
                animator.SetBool(IsSurfing, false);
                animator.SetBool(IsFishing, true);
            }
            else
            {
                if (CurrentPlayer.UseBike && !CurrentPlayer.IsSurfing)
                {
                    animator.SetBool(IsRunning, false);
                    animator.SetBool(IsWalking, false);
                    animator.SetBool(IsDriving, false);
                    animator.SetBool(FoundItem, false);
                    animator.SetBool(IsSurfing, false);
                    animator.SetBool(IsFishing, false);

                    animator.SetBool(IsOnBike, true);
                }
                else if(CurrentPlayer.IsSurfing && !CurrentPlayer.UseBike)
                {
                    animator.SetBool(IsRunning, false);
                    animator.SetBool(IsWalking, false);
                    animator.SetBool(IsDriving, false);
                    animator.SetBool(FoundItem, false);
                    animator.SetBool(IsOnBike, false);
                    animator.SetBool(IsFishing, false); 
                    
                    animator.SetBool(IsSurfing, true);
                }
                else
                {
                    animator.SetBool(IsRunning, false);
                    animator.SetBool(IsWalking, false);
                    animator.SetBool(IsDriving, false);
                    animator.SetBool(IsOnBike, false);
                    animator.SetBool(IsSurfing, false);
                    animator.SetBool(IsFishing, false);
                }
            }
            MoveCharacter();
        }

        /// <summary>
        /// Makes the player look up.
        /// </summary>
        public void LookUp()
        {
            CurrentPlayer.ChangeState(PlayerState.Idle);
            change.x = 0;
            change.y = 1;
            ResetAnimations();
            UpdateAnimation();
            interactionBoxes[0].SetActive(true);
            triedInteracting = true;
        }
        
        /// <summary>
        /// Makes the player look down.
        /// </summary>
        public void LookDown()
        {
            CurrentPlayer.ChangeState(PlayerState.Idle);
            change.x = 0;
            change.y = -1;
            ResetAnimations();
            UpdateAnimation();
        }

        /// <summary>
        /// Activates all Interaction Boxes at the same time.
        /// </summary>
        public void Look()
        {
            for (int i = 0; i < interactionBoxes.Length; i++)
            {
                interactionBoxes[i].SetActive(true);
            }

            triedInteracting = true;
        }

        /// <summary>
        /// Self explaining.
        /// </summary>
        public void ResetAnimations()
        {
            animator.SetBool(IsRunning, false);
            animator.SetBool(IsWalking, false);
            animator.SetBool(IsDriving, false);
            animator.SetBool(IsOnBike, false);
            animator.SetBool(IsSurfing, false);
            animator.SetBool(IsFishing, false);
        }

        /// <summary>
        /// Stops Player on tile he is going to stand on.
        /// </summary>
        public void StopPlayer()
        {
            isTeleporting = true;
        }

        private void LoadPlayerControllerData()
        {
           PlayerControllerData data = LoadPlayerController();

           direction.x = data.direction[0];
           direction.y = data.direction[1];

           animator.SetFloat(MoveX, direction.x);
           animator.SetFloat(MoveY, direction.y);
        }
    }
}
