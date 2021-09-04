using System;
using System.Collections.Generic;
using Pikimins;
using SaveData;
using UI;
using UnityEngine;
using static Audio.AudioManager;
using static Battle.BattleManager;
using static Map.Regions;
using static Players.PikiminInventory;
using static Players.PlayerController;
using static SaveData.SaveManager;

namespace Players
{
    public enum PlayerState
    {
        Walk,
        Interact,
        Idle,
        Fight
    }
    public class Player : MonoBehaviour
    {
        public static Player CurrentPlayer;
        
        [Header("PlayerState")]
        [SerializeField] private PlayerState currentState;

        [Header("Region")]
        [SerializeField] private PikiminRegions playerRegion;
        public byte lastReha;
        
        [Header("Owned Pikimin Info")]
        public Pikimin[] playerOwnedPikimins = new Pikimin[4];

        [Header("Piki center")]
        [SerializeField] private Transform rehaRespawnPoint;
        [SerializeField] private Transform startPoint;

        [Header("Items Usage")] 
        [SerializeField] private bool useBike;

        [SerializeField] private bool isSurfing;
        
        [SerializeField] public bool wantToFly;

        [SerializeField] private bool isProtected;

        [SerializeField] private byte beatenDojoChefs;
        
        public PlayerState CurrentState => currentState;

        public PikiminRegions PlayerRegion => playerRegion;

        public bool UseBike => useBike;

        public bool IsSurfing => isSurfing;

        public byte BeatenDojoChefs
        {
            get => beatenDojoChefs; 
            private set => beatenDojoChefs = value; 
        }

        public bool IsProtected => isProtected;

        private void Awake()
        {
            if (CurrentPlayer != null)
            {
                Destroy(this);
            }
            else
            {
                CurrentPlayer = this;
            }
            
            
        }

        private void Start()
        {
            currentState = PlayerState.Idle;
            lastReha = 20;
            CurrentPlayer.transform.position = startPoint.position;
            LoadData.LoadDataInstance.OnLoad += LoadPlayerData;
            BattleManagerInstance.ONPlayerDeath += Death;
        }

        private void Update()
        {
            playerOwnedPikimins = playerPikiminInventory.OwnedPikimin;
        }
        
        /// <summary>
        /// Changes the state of the Player.
        /// </summary>
        /// <param name="newState"></param>
        public void ChangeState(PlayerState newState)
        {
            currentState = newState;
        }

        /// <summary>
        /// Change playerRegion.
        /// </summary>
        /// <param name="newRegion"></param>
        public void ChangeRegion(PikiminRegions newRegion)
        {
            playerRegion = newRegion;
        }

        /// <summary>
        /// Activates and deactivates the bike.
        /// </summary>
        public void SwapBikeState()
        {
            if (currentState == PlayerState.Idle)
            {
                useBike = !useBike;
            }
            else if (CurrentPlayerController.isIndoors || CurrentPlayer.isSurfing)
            {
                useBike = false;
            }
        }

        /// <summary>
        /// Activate Surfer Animation.
        /// </summary>
        public void SwapSurfState()
        {
            isSurfing = !isSurfing;
        }

        /// <summary>
        /// Spawn in front of PikiCenter.
        /// </summary>
        private void Death()
        {
            if(lastReha == 20)
            {
                // If you lose before entering the first reha, you spawn at your home with healed Pikimin.
                CurrentPlayer.gameObject.transform.position = startPoint.position;
                foreach (var t in playerPikiminInventory.OwnedPikimin)
                {
                    if (t != null)
                    {
                        t.Heal(9999, true, true);
                    }
                }
            }
            else
            {
                CurrentPlayer.gameObject.transform.position = rehaRespawnPoint.position;
            }
            useBike = false;
            CurrentPlayerController.isIndoors = true;
            CurrentPlayerController.LookUp();
        }

        /// <summary>
        /// Teleports the Player to his Start Position.
        /// </summary>
        public void PutToStart()
        {
            gameObject.transform.position = startPoint.position;
        }

        /// <summary>
        /// Increases the number of beaten dojo chefs.
        /// </summary>
        public void AfterBeatingDojoChef()
        {
            // Failsafe.
            if (BeatenDojoChefs < 3)
            {
                BeatenDojoChefs++;
            }
        }

        public void SwapProtected()
        {
            isProtected = !isProtected;
        }

        private void LoadPlayerData()
        {
            //Read Playerdata
            PlayerData data = LoadPlayer();

            var myTransform = transform;
            //Position
            Vector3 position;
            position.x = data.position[0];
            position.y = data.position[1];
            position.z = data.position[2];
            myTransform.position = position;
            
            //Rotation
            Quaternion rotation = default;
            rotation.x = data.rotation[0];
            rotation.y = data.rotation[1];
            rotation.z = data.rotation[2];
            myTransform.rotation = rotation;
            
            //Scale
            Vector3 scale;
            scale.x = data.scale[0];
            scale.y = data.scale[1];
            scale.z = data.scale[2];
            myTransform.localScale = scale;
            
            playerRegion = (PikiminRegions) data.playerRegion;

            useBike = data.useBike;
            isSurfing = data.isSurfing;
            beatenDojoChefs = data.beatenDojoChefs;
            lastReha = data.lastReha; 
            
            //Audio
            AudioData audioData = LoadAudioClip();
            for (int i = 0; i < AudioManagerInstance.AreaSounds.Length; i++)
            { 
                if (AudioManagerInstance.AreaSounds[i].name == audioData.backgroundSound)
                {
                    AudioManagerInstance.BackgroundSound.clip = AudioManagerInstance.AreaSounds[i];
                    AudioManagerInstance.BackgroundSound.Play();
                }
            }
        }
    }
}
