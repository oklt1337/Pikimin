using UnityEngine;
using static Players.PlayerController;
using static Players.Player;

namespace Virtual_Machine
{
    public class WaterObject : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Surf") && other.isTrigger)
            {
                CurrentPlayerController.Surf();
            }
            else if (other.CompareTag("Rod") && other.isTrigger && CurrentPlayer.PlayerRegion != Map.Regions.PikiminRegions.Route4)
            {
                CurrentPlayerController.Fish();
            }
        }
    }
}
