using Items;
using TMPro;
using UnityEngine;
using Players;
using static Players.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SaveData;
using UI;
using static UI.DialogueManager;
using static Audio.AudioManager;
using static GameManager;

namespace Players
{
    public class ItemInventory : MonoBehaviour
    {
        public static ItemInventory playerItems;

        public event Action<Item> AddItem;

        [SerializeField] private Item[] allItems = new Item[14];

        [Header("Components")]
        [SerializeField] private Item[] ownedItems = new Item[14];
        [SerializeField] private Shop shop;

        [Header("Item Menu")]
        public GameObject itemBox;
        [SerializeField] private GameObject otherOtherAuthorizeBox;
        [SerializeField] private GameObject MenuBox;
        [SerializeField] private OtherOtherAuthorize Authorize;
        [SerializeField] private TextMeshProUGUI ItemNames;
        [SerializeField] private TextMeshProUGUI ItemAmounts;
        [SerializeField] private byte currentPosition;

        [Header("Misc")]
        [SerializeField] private int ownedMoney = 10000;
        [SerializeField] private bool itemWasAdded;
        [SerializeField] private GameObject dialog;
        [SerializeField] private Item firstStartItem;
        [SerializeField] private Item secondStartItem;
        public int OwnedMoney
        {
            get => ownedMoney;
        }

        public int CurrentPosition
        {
            get => currentPosition;
        }

        public Item[] OwnedItems
        {
            get => ownedItems;
            private set => ownedItems = value;
        }

        private void Awake()
        {
            playerItems = this;
        }

        private void Start()
        {
            LoadData.LoadDataInstance.OnLoad += LoadItemInventory;
            AddItem += AddToArray;

            // First 2 Items.
            if(!Instance.continueGame)
            {
                AddNewToInventory(firstStartItem);
                itemWasAdded = false;
                AddNewToInventory(secondStartItem);
                for (int i = 0; i < 4; i++)
                {
                    itemWasAdded = false;
                    AddExistingToInventory(secondStartItem);
                }
                itemWasAdded = false;
            }
        }

        private void Update()
        {
            if (itemBox.activeSelf)
            {
                Inputs();
                TextManager();
            }
        }

        public virtual void OnNewItem(Item newItem)
        {
            AddItem?.Invoke(newItem);
        }

        /// <summary>
        /// Automatically sorts the Items to prevent holes.
        /// </summary>
        private void AutoSorter()
        {
            for (int i = 0; i < 11; i++)
            {
                if (ownedItems[i] == null)
                {
                    ownedItems[i] = ownedItems[i + 1];
                    ownedItems[i + 1] = null;
                }
            }
        }

        /// <summary>
        /// Applys the appropriate method to add the new Item to the Inventory.
        /// </summary>
        private void AddToArray(Item newItem)
        {
            itemWasAdded = false;

            AddExistingToInventory(newItem);

            if (!itemWasAdded)
            {
                AddNewToInventory(newItem);
            }

            // Checking if there are holes in the owned Items and filling them.
            AutoSorter();
        }

        /// <summary>
        /// Checks if the Player has the Item already and adds the amount if he does.
        /// </summary>
        private void AddExistingToInventory(Item newItem)
        {
            // Checks if Player has the Item already.
            for (int i = 0; i < 12; i++)
            {
                if (ownedItems[i] != null)
                {
                    if (newItem.ItemName == ownedItems[i].ItemName)
                    {
                        ownedItems[i].AddItems(newItem.ItemAmount + ownedItems[i].OwnedCopies);
                        itemWasAdded = true;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Searches for a free spot in the Inventory and fills it with the new Item.
        /// </summary>
        private void AddNewToInventory(Item newItem)
        {
            for (int i = 0; i < 12; i++)
            {
                if (ownedItems[i] == null && !itemWasAdded)
                {
                    ownedItems[i] = newItem;
                    ownedItems[i].AddItems(newItem.ItemAmount);
                    itemWasAdded = true;
                    break;
                }
            }
        }

        /// <summary>
        /// What the Player inputs.
        /// </summary>
        private void Inputs()
        {
            if (!otherOtherAuthorizeBox.activeSelf)
            {
                // UP and DOWN
                if (ownedItems[currentPosition + 1] != null && currentPosition < 12 && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)))
                {
                    AudioManagerInstance.Sfx.PlayOneShot(AudioManagerInstance.ButtonOverSounds[0]);
                    currentPosition++;
                }
                else if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && 0 < currentPosition)
                {
                    AudioManagerInstance.Sfx.PlayOneShot(AudioManagerInstance.ButtonOverSounds[0]);
                    currentPosition--;
                }
            }

            // Checking if the Player is currently fighting
            if(CurrentPlayer.CurrentState == PlayerState.Fight)
            {
                if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Q))
                {
                    Authorize.wantsToRecycle = false;
                    Authorize.wantsToUse = false;
                    otherOtherAuthorizeBox.SetActive(false);
                    itemBox.SetActive(false);
                }
            } 
            else
            {
                // Closing Inventory and going back to the Idle.
                if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.P))
                {
                    CloseInventory();
                }

                // Closing Inventory and opening the normal Menu.
                if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Q))
                {
                    Authorize.wantsToRecycle = false;
                    Authorize.wantsToUse = false;
                    otherOtherAuthorizeBox.SetActive(false);
                    itemBox.SetActive(false);
                    MenuBox.SetActive(true);
                }
            }

            if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E)))
            {
                // Opens the other other Authorize Box to let the Player choose what to do with the Item.
                if(!Authorize.wantsToUse && !Authorize.wantsToRecycle && !Authorize.wantsToSeeDescription)
                {
                    AudioManagerInstance.Sfx.PlayOneShot(AudioManagerInstance.ButtonOverSounds[0]);
                    otherOtherAuthorizeBox.SetActive(true);
                } 
               
            }
            // Makes the Player use the Item.
            else if (Authorize.wantsToUse)
            {
                Authorize.wantsToUse = false;
                ownedItems[currentPosition].OnUse();

                DeleteMissingItem();
            }
            // Recycles the Item.
            else if (Authorize.wantsToRecycle)
            {
                Authorize.wantsToRecycle = false;
                
                if (ownedItems[currentPosition].IsRecyclable)
                {
                    ownedItems[currentPosition] = null;
                }

                DeleteMissingItem();
            }
            else if (Authorize.wantsToSeeDescription)
            {
                dialog.SetActive(true);
                ownedItems[currentPosition].ShowDescription();
                Authorize.wantsToSeeDescription = false;
            }
            
            if (Dialogue.chattingHasEnded)
            {
                dialog.SetActive(false);
                Dialogue.chattingHasEnded = false;
                CurrentPlayer.ChangeState(PlayerState.Interact);
            }
        }

        /// <summary>
        /// Gives Player money after beating a Trainer.
        /// </summary>
        /// <param name="money"> The Trainers money stat. </param>
        public void AddMoney(int money)
        {
            ownedMoney += money;

            // Preventing negative money values.
            if(ownedMoney < 0)
            {
                ownedMoney = 0;
            }
        }

        /// <summary>
        /// Takes care of the text.
        /// </summary>
        private void TextManager()
        {
            ItemNames.text = "";
            ItemAmounts.text = "";
            for (int i = 0; i < OwnedItems.Length; i++)
            {
                if(ownedItems[i] != null)
                {
                    if(i == currentPosition)
                    {
                        ItemNames.text += "\n > " + ownedItems[i].ItemName + "\n";
                    }
                    else
                    {
                        ItemNames.text += "\n" + ownedItems[i].ItemName + "\n";
                    }
                    ItemAmounts.text += "\n" + ownedItems[i].OwnedCopies + "x\n";
                }
                else
                {
                    ItemNames.text += "\n" + "-------------------------------------------" + "\n";
                    ItemAmounts.text += "\n" + "---------" + "\n";
                }
            }
        }

        /// <summary>
        /// Deletes the item from the Inventory.
        /// </summary>
        public void DeleteMissingItem()
        {
            for (int i = 0; i < ownedItems.Length; i++)
            {
                if(ownedItems[i] != null)
                {
                    if (ownedItems[i].OwnedCopies < 1)
                    {
                        ownedItems[currentPosition] = null;
                        if(ownedItems[currentPosition] == null && currentPosition > 0)
                        {
                            currentPosition--;
                        }
                    }
                }
            }
            AutoSorter();
        }

        /// <summary>
        /// Closes the Item Inventory.
        /// </summary>
        public void CloseInventory()
        {
            Authorize.wantsToRecycle = false;
            Authorize.wantsToUse = false;
            otherOtherAuthorizeBox.SetActive(false);
            itemBox.SetActive(false);
            CurrentPlayer.ChangeState(PlayerState.Idle);
            Dialogue.gameObject.SetActive(false);
        }

        private void LoadItemInventory()
        {
            ItemInventoryData data = SaveManager.LoadItemInventory();

            ownedMoney = data.money;
            Item[] items = new Item[14];

            for (int i = 0; i < allItems.Length; i++)
            {
                for (int j = 0; j < data.items.Length; j++)
                {
                    if (allItems[i].ItemName == data.items[j]) 
                    {
                        items[j] = allItems[i];
                        items[j].AddItems(data.ownedCopies[j]);
                    }
                }
            }
            ownedItems = items;
        }
    }
}
