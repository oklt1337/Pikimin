using Audio;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Players.PikiminInventory;
using static UI.TrainerPass;

namespace UI
{
    public class EndScreen : MonoBehaviour
    {
        [SerializeField] private Image[] usedPikimin;
        [SerializeField] private TextMeshProUGUI text;

        private void Start()
        {
            InfoUpdater();
        }

        /// <summary>
        /// Updates the Info of the endscreen.
        /// </summary>
        private void InfoUpdater()
        {
            for (int i = 0; i < playerPikiminInventory.OwnedPikimin.Length; i++)
            {
                if(playerPikiminInventory.OwnedPikimin[i] != null)
                {
                    usedPikimin[i].enabled = true;
                    usedPikimin[i].sprite = playerPikiminInventory.OwnedPikimin[i].FrontSprite;
                }
            }

            TrainerPassInstance.UpdateInfo();
            text.text = TrainerPassInstance.Text.text;
        }
    }
}
