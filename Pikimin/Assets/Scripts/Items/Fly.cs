using Players;
using UnityEngine;
using static Map.PikiMap;
using static Players.PlayerController;
using static Players.Player;

namespace Items
{
    public class Fly : HiddenMachine
    {
        public override void OnUse()
        {
            if (CheckIfHasPikiminOfType())
            {
                CurrentMap.SwapMapOnOff();
                CurrentPlayer.wantToFly = true;
            }
        }

        public void Flying(Transform telePos)
        {
            CurrentPlayer.ChangeState(PlayerState.Idle);
            var position = telePos.position;
            CurrentPlayerController.TeleportPlayer(new Vector2(position.x, position.y-1));
            CurrentPlayerController.LookDown();
            
            if (CurrentPlayer.UseBike)
            {
                CurrentPlayer.SwapBikeState();
            }

            if (CurrentPlayer.IsSurfing)
            {
                CurrentPlayer.SwapSurfState();
            }
        }
    }
}
