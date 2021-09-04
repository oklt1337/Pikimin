using Players;
using static Players.ItemInventory;
using static Players.PlayerController;

namespace Items
{
    public class FishingRod : Item
    {
        public override void OnUse()
        {
            if (Player.CurrentPlayer.CurrentState != PlayerState.Walk)
            {
                switch (CurrentPlayerController.LooksRight)
                {
                    case null when CurrentPlayerController.LooksUp == true:
                        CurrentPlayerController.fishingBoxes[0].SetActive(true);
                        break;
                    case true when CurrentPlayerController.LooksUp == null:
                        CurrentPlayerController.fishingBoxes[1].SetActive(true);
                        break;
                    case null when CurrentPlayerController.LooksUp == false:
                        CurrentPlayerController.fishingBoxes[2].SetActive(true);
                        break;
                    case false when CurrentPlayerController.LooksUp == null:
                        CurrentPlayerController.fishingBoxes[3].SetActive(true);
                        break;
                }
                CurrentPlayerController.triedFishing = true;
            
                playerItems.CloseInventory();
            }
        }
    }
}
