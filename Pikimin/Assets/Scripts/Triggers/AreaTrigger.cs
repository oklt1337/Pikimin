using UI;
using UnityEngine;
using static Audio.AudioManager;
using static Players.Player;

namespace Triggers
{
    public class AreaTrigger : MonoBehaviour
    {
        public byte areaIndex;
        public string areaName;
        public AreaManager manager;
        public GameObject box;
        [SerializeField] private Map.Regions.PikiminRegions thisRegion;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Sets the upper left box active and overwrites its information.
            if(manager.currentAreaIndex != areaIndex && collision.CompareTag("Player") && CurrentPlayer.PlayerRegion != thisRegion)
            {
                manager.activeDuration = 2f;
                manager.currentAreaIndex = areaIndex;
                manager.currentArea = areaName;
            
                box.SetActive(true);
                UpdateRegion();
                
                AudioManagerInstance.PlayAudioClip(false, AudioManagerInstance.AreaSounds[(int) CurrentPlayer.PlayerRegion]);
            }
        }

        private void UpdateRegion()
        {
            CurrentPlayer.ChangeRegion(thisRegion);
        }
    }
}
