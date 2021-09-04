using Players;
using UnityEngine;
using static Players.Player;
using static Players.PlayerController;

namespace Triggers
{
    public class TeleporterBehaviour : MonoBehaviour
    {
        [SerializeField] private Transform[] newPosition;
        [SerializeField] private byte indicator;
        [SerializeField] private bool isEntry;
        [SerializeField] private bool isKeyBuilding;
        [SerializeField] private bool isHouse;


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (newPosition.Length == 1)
            {
                if (collision.CompareTag("Player"))
                {
                    // Prevents beeing locked into infinite teleports.
                    if (isEntry)
                    {
                        CurrentPlayer.ChangeState(PlayerState.Idle);
                        CurrentPlayerController.TeleportPlayer(new Vector2(newPosition[0].position.x, newPosition[0].position.y+1) );
                        CurrentPlayer.ChangeState(PlayerState.Idle);
                        if (isHouse)
                        {
                            CurrentPlayerController.isIndoors = true;
                            if (CurrentPlayer.UseBike)
                            {
                                CurrentPlayer.SwapBikeState();
                            }
                        }
                        
                    }
                    else
                    {
                        CurrentPlayer.ChangeState(PlayerState.Idle);
                        CurrentPlayerController.TeleportPlayer(new Vector2(newPosition[0].position.x, newPosition[0].position.y-1) );
                        CurrentPlayer.ChangeState(PlayerState.Idle);
                        CurrentPlayerController.isIndoors = false;
                    }

                    if (isKeyBuilding)
                    {
                        // 0 brain method to save the position to teleport too.
                        CurrentPlayer.lastReha = indicator;
                    }
                }
            }
            else
            {
                if(collision.CompareTag("Player"))
                {
                    // Prevents beeing locked into infinite teleports.
                    if (isEntry)
                    {
                        CurrentPlayer.ChangeState(PlayerState.Idle);
                        CurrentPlayerController.TeleportPlayer(new Vector2(newPosition[CurrentPlayer.lastReha].position.x, newPosition[CurrentPlayer.lastReha].position.y+1) );
                        CurrentPlayerController.isIndoors = true;
                    }
                    else
                    {
                        CurrentPlayer.ChangeState(PlayerState.Idle);
                        CurrentPlayerController.TeleportPlayer(new Vector2(newPosition[CurrentPlayer.lastReha].position.x, newPosition[CurrentPlayer.lastReha].position.y-1) );
                        CurrentPlayerController.isIndoors = false;
                    }
                }
            }
        }
    }
}
