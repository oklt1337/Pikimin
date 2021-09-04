using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static UI.PikidexBehaviour;
using static Players.Player;
using static Players.ItemInventory;

namespace UI
{
    public class TrainerPass : MonoBehaviour
    {
        public static TrainerPass TrainerPassInstance;

        [SerializeField] private GameObject[] childObjects;
        [SerializeField] private Image image;

        [Header("General Trainer Info")]
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private byte caughtDifferentPikiminAmount;
        [SerializeField] private byte seenDifferentPikiminAmount;

        [Header("Dojos")]
        [SerializeField] private Image firstDojo;
        [SerializeField] private Image secondDojo;
        [SerializeField] private Image thirdDojo;
        [SerializeField] private GameObject menu;

        public TextMeshProUGUI Text => text;

        public byte CaughtDifferentPikiminAmount => caughtDifferentPikiminAmount;

        public byte SeenDifferentPikiminAmount => seenDifferentPikiminAmount;

        private void Awake()
        {
            TrainerPassInstance = this;
        }

        private void Start()
        {
            for (int i = 0; i < childObjects.Length; i++)
            {
                childObjects[i].SetActive(true);
            }

            image.enabled = true;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            Inputs();
        }

        /// <summary>
        /// The inputs of the player.
        /// </summary>
        private void Inputs()
        {
            if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.P))
            {
                gameObject.SetActive(false);
                CurrentPlayer.ChangeState(Players.PlayerState.Idle);
            }

            if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Q))
            {
                gameObject.SetActive(false);
                menu.SetActive(true);
            }
        }

        /// <summary>
        /// Updates the amount of optained Dojo Badges.
        /// </summary>
        public void UpdateInfo()
        {
            // Update the images of the Dojo badges.
            if (CurrentPlayer.BeatenDojoChefs == 1)
            {
                firstDojo.gameObject.SetActive(true);
            }
            else if (CurrentPlayer.BeatenDojoChefs == 2)
            {
                firstDojo.gameObject.SetActive(true);
                secondDojo.gameObject.SetActive(true);
            }
            else if (CurrentPlayer.BeatenDojoChefs > 2)
            {
                firstDojo.gameObject.SetActive(true);
                secondDojo.gameObject.SetActive(true);
                thirdDojo.gameObject.SetActive(true);
            }

            // Update the amount of seen Pikimin.
            seenDifferentPikiminAmount = 0;
            for (int i = 0; i < PikidexInstance.HasBeenSeen.Length; i++)
            {
                if (PikidexInstance.HasBeenSeen[i])
                {
                    seenDifferentPikiminAmount++;
                }
            }

            // Update the amount of caught Pikimin.
            caughtDifferentPikiminAmount = 0;
            for (int i = 0; i < PikidexInstance.HasBeenCaught.Length; i++)
            {
                if (PikidexInstance.HasBeenCaught[i])
                {
                    caughtDifferentPikiminAmount++;
                }
            }
            TextManager();
        }

        /// <summary>
        /// Manages what the Text says.
        /// </summary>
        private void TextManager()
        {
            text.text =  "Owned Money: " + playerItems.OwnedMoney + "$";
            text.text += "\n\nName: Ash     ID: 38667";
            text.text += "\n\nUnique Pikimin seen: " + seenDifferentPikiminAmount;
            text.text += "\n\nUnique Pikimin caught: " + caughtDifferentPikiminAmount;
        }
    }
}
