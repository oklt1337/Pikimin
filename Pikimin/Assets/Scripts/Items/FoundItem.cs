using UnityEngine;
using static Players.ItemInventory;
using static Players.PlayerController;
using static UI.DialogueManager;
using static Items.ItemManager;

namespace Items
{
    public class FoundItem : MonoBehaviour
    {
        [SerializeField] private Item containedItem;
        [SerializeField] private byte indicator;

        private Item ContainedItem 
        {
            get => containedItem;
            set => containedItem = value;
        }

        private void Update()
        {
            if (ItemManagerInstance.wasTaken[indicator])
            {
                gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                if (ContainedItem != null)
                {
                    playerItems.OnNewItem(ContainedItem);
                    Dialogue.ToggleOnOff();
                    Dialogue.talkingCharacter = "Ash";
                    Dialogue.ManuelDialogue("Oh I found an " + ContainedItem.ItemName + ".");
                    ContainedItem = null;
                    CurrentPlayerController.foundItem = true;
                    ItemManagerInstance.wasTaken[indicator] = true;
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
