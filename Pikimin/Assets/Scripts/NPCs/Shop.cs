using Items;
using Players;
using TMPro;
using UI;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [Header("Components")]
    public GameObject authorizeBox;
    [SerializeField] private TextMeshProUGUI itemNames;
    [SerializeField] private TextMeshProUGUI itemPrizes;
    [SerializeField] private TextMeshProUGUI playerMoney;
    [SerializeField] private ItemInventory inventory;
    [SerializeField] private AuthorizeMenu authorize;


    [Header("Controlls")]
    private byte currentPosition;

    [Header("Items")] [SerializeField] private Item[] shopInventory;

    private void Start()
    {
        for (int i = 0; i < shopInventory.Length; i++)
        {
            itemPrizes.text += "\n" + shopInventory[i].ItemValue + "$" + "\n\n";

            if(i == currentPosition)
            {
                itemNames.text += "\n > " + shopInventory[i].ItemName + "\n\n";
            }
            else
            {
                itemNames.text += "\n" + shopInventory[i].ItemName + "\n\n";
            }
        }
    }

    private void Update()
    {
        if (Player.CurrentPlayer.CurrentState != PlayerState.Interact)
        {
            Player.CurrentPlayer.ChangeState(PlayerState.Interact);
        }

        PlayerInput();

        playerMoney.text = "You currently have " + ItemInventory.playerItems.OwnedMoney + "$";
    }

    /// <summary>
    /// The Players Input.
    /// </summary>
    private void PlayerInput()
    {
        // Moves the choosen Item.
        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && currentPosition < shopInventory.Length - 1 && !authorizeBox.activeSelf)
        {
            currentPosition++;

            // Changing Text accordingly.
            itemNames.text = "";
            for (int i = 0; i < shopInventory.Length; i++)
            {
                if (i == currentPosition)
                {
                    itemNames.text += "\n > " + shopInventory[i].ItemName + "\n\n";
                }
                else
                {
                    itemNames.text += "\n" + shopInventory[i].ItemName + "\n\n";
                }

            }
        }
        else if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && currentPosition > shopInventory.Length - shopInventory.Length && !authorizeBox.activeSelf)
        {
            currentPosition--;

            // Changing Text accordingly.
            itemNames.text = "";
            for (int i = 0; i < shopInventory.Length; i++)
            {
                if (i == currentPosition)
                {
                    itemNames.text += "\n > " + shopInventory[i].ItemName + "\n\n";
                }
                else
                {
                    itemNames.text += "\n" + shopInventory[i].ItemName + "\n\n";
                }

            }
        }

        // Makes the Player authorize his choice.
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E)))
        {
            authorize.playerHasAuthorized = false;
            authorizeBox.SetActive(true);
        } 
       
        if (authorize.playerHasAuthorized)
        {
            authorize.playerHasAuthorized = false;
            if (authorize.WantsHisChoice)
            {
                SellsItemToPlayer();
            }
        }

        // Closes shop.
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Q))
        {
            Player.CurrentPlayer.ChangeState(PlayerState.Idle);
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Adds the bought items to the ItemInventory and takes the Players money.
    /// </summary>
    private void SellsItemToPlayer()
    {
        // Checking if the player has enough money.
        if (inventory.OwnedMoney >= shopInventory[currentPosition].ItemValue)
        {
            ItemInventory.playerItems.AddMoney(-shopInventory[currentPosition].ItemValue);
            ItemInventory.playerItems.OnNewItem(shopInventory[currentPosition]);
            authorizeBox.SetActive(false);
        }
    }
}