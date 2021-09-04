using static Players.Player;
using static Players.ItemInventory;
using static Players.PlayerController;

namespace Items
{
    public class Bicycle : Item
    {
        public override void OnUse()
        {
            if (!CurrentPlayerController.isIndoors)
            {
                CurrentPlayer.SwapBikeState();
                playerItems.CloseInventory();
            }
        }
    }
}
