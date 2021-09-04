using Pikimins;
using Virtual_Machine;
using static Players.ItemInventory;
using static Players.PlayerController;

namespace Items
{
    public class Cut : HiddenMachine
    {   
        public override void OnUse()
        {
            if (CheckIfHasPikiminOfType())
            {
                switch (CurrentPlayerController.LooksRight)
                {
                    case null when CurrentPlayerController.LooksUp == true:
                        CurrentPlayerController.cuttingBoxed[0].SetActive(true);
                        break;
                    case true when CurrentPlayerController.LooksUp == null:
                        CurrentPlayerController.cuttingBoxed[1].SetActive(true);
                        break;
                    case null when CurrentPlayerController.LooksUp == false:
                        CurrentPlayerController.cuttingBoxed[2].SetActive(true);
                        break;
                    case false when CurrentPlayerController.LooksUp == null:
                        CurrentPlayerController.cuttingBoxed[3].SetActive(true);
                        break;
                }
                CurrentPlayerController.triedCutting = true;
            }
            playerItems.CloseInventory();
        }
    }
}
