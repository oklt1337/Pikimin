using System.Linq;
using Items;
using Players;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using static Players.Player;
using static Players.ItemInventory;
using static Players.PlayerController;

namespace Map
{
    public class PikiMap : MonoBehaviour
    {
        public static PikiMap CurrentMap;
        [SerializeField] private Transform[] city;
        [SerializeField] private Fly fly;
        [SerializeField] private Image image;
        [SerializeField] private GameObject imageMap;
        [SerializeField] private Button[] cityButtons;

        [SerializeField] private int currentSelection;

        private void Awake()
        {
            CurrentMap = this;
        }

        private void Start()
        {
            gameObject.SetActive(false);
            image.enabled = true;
            imageMap.SetActive(true);

            cityButtons[0].onClick.AddListener(Altair);
            cityButtons[1].onClick.AddListener(Deneb);
            cityButtons[2].onClick.AddListener(Unukal);
            cityButtons[3].onClick.AddListener(Vega);
            cityButtons[4].onClick.AddListener(Triverr);
            cityButtons[5].onClick.AddListener(Maeus);
        }

        private void Update()
        {
            if (gameObject.activeSelf)
            {
                PlayerInput();
                FixSelection();
                UpdateHighlights();
            }
        }

        private void PlayerInput()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                currentSelection++;
                
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                currentSelection--;
            }

            if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return)) && CurrentPlayer.wantToFly)
            {
                ChooseCity();
            }
            
            if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Tab))
            {
                SwapMapOnOff();
                CurrentPlayer.wantToFly = false;
            }

            if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Backspace))
            {
                gameObject.SetActive(!gameObject.activeSelf);
                playerItems.itemBox.SetActive(true);
            }
        }
        
        /// <summary>
        /// Provokes selection value to be out of range.
        /// </summary>
        private void FixSelection()
        {
            if (currentSelection < 1)
            {
                currentSelection = 0;
            }
            else if (currentSelection > 5)
            {
                currentSelection = 0;
            }
        }

        private void UpdateHighlights()
        {
            for (int i = 0; i < city.Length; i++)
            {
                cityButtons[i].image.color = Color.white;
            }
            cityButtons[currentSelection].image.color = Color.red;
        }

        private void ChooseCity()
        {
            cityButtons[currentSelection].onClick?.Invoke();
        }

        public void SwapMapOnOff()
        {
            gameObject.SetActive(!gameObject.activeSelf);
            playerItems.CloseInventory();
            if (gameObject.activeSelf)
            {
                CurrentPlayer.ChangeState(PlayerState.Interact);
                currentSelection = 0;
            }
        }

        private void Altair()
        {
            if (playerItems.OwnedItems.Contains(fly))
            {
                fly.Flying(city[0]);
                CurrentPlayer.wantToFly = false;
                SwapMapOnOff();
                CurrentPlayerController.isIndoors = false;
            }
        }

        private void Deneb()
        {
            if (playerItems.OwnedItems.Contains(fly))
            {
                fly.Flying(city[1]);
                CurrentPlayer.wantToFly = false;
                SwapMapOnOff();
                CurrentPlayerController.isIndoors = false;
            }
        }

        private void Unukal()
        {
            if (playerItems.OwnedItems.Contains(fly))
            {
                fly.Flying(city[2]);
                CurrentPlayer.wantToFly = false;
                SwapMapOnOff();
                CurrentPlayerController.isIndoors = false;
            }
        }

        private void Vega()
        {
            if (playerItems.OwnedItems.Contains(fly))
            {
                fly.Flying(city[3]);
                CurrentPlayer.wantToFly = false;
                SwapMapOnOff();
                CurrentPlayerController.isIndoors = false;
            }
        }

        private void Triverr()
        {
            if (playerItems.OwnedItems.Contains(fly))
            {
                fly.Flying(city[4]);
                CurrentPlayer.wantToFly = false;
                SwapMapOnOff();
                CurrentPlayerController.isIndoors = false;
            }
        }

        private void Maeus()
        {
            if (playerItems.OwnedItems.Contains(fly))
            {
                fly.Flying(city[5]);
                CurrentPlayer.wantToFly = false;
                SwapMapOnOff();
                CurrentPlayerController.isIndoors = false;
            }
        }
    }
}
