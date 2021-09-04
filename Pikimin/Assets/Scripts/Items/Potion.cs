using UnityEngine;
using static Players.PikiminInventory;
using static Players.ItemInventory;

namespace Items
{
    public class Potion : Item
    {
        [Header("Pot Info")]
        [SerializeField] private int healStrength;
        [SerializeField] private bool canRevive;

        public bool CanRevive => canRevive;

        public override void OnUse()
        {
            if (!playerPikiminInventory.hasHealed)
            {
                playerItems.itemBox.SetActive(false);
                playerPikiminInventory.isHealing = true;
                playerPikiminInventory.healStrength = healStrength;
                playerPikiminInventory.canRevive = canRevive;
                playerPikiminInventory.inventoryBox.SetActive(true);
            }
            else
            {
                playerPikiminInventory.hasHealed = false;
                playerPikiminInventory.isHealing = false;
                base.OnUse();
            }
        }
    }
}
