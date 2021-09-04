using UnityEngine;
using static Players.PlayerController;

namespace Virtual_Machine
{
    public class CutObject : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Cut") && other.isTrigger)
            {
                CurrentPlayerController.CutTree();
            }
        }
    }
}
