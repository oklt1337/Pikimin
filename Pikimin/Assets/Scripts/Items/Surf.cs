using static Players.PlayerController;
using static Players.ItemInventory;

namespace Items
{
    public class Surf : HiddenMachine
    {
        public override void OnUse()
        {
            if (CheckIfHasPikiminOfType())
            {
                switch (CurrentPlayerController.LooksRight)
                {
                    case null when CurrentPlayerController.LooksUp == true:
                        CurrentPlayerController.surfingBoxes[0].SetActive(true);
                        break;
                    case true when CurrentPlayerController.LooksUp == null:
                        CurrentPlayerController.surfingBoxes[1].SetActive(true);
                        break;
                    case null when CurrentPlayerController.LooksUp == false:
                        CurrentPlayerController.surfingBoxes[2].SetActive(true);
                        break;
                    case false when CurrentPlayerController.LooksUp == null:
                        CurrentPlayerController.surfingBoxes[3].SetActive(true);
                        break;
                }
                CurrentPlayerController.triedSurfing = true;
            }
            playerItems.CloseInventory();
        }
    }
}
