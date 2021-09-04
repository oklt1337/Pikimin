using Audio;
using Players;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Audio.AudioManager;
using static Players.Player;
using static UI.PikidexBehaviour;
using static UI.TrainerPass;
using static UI.DataManager;

namespace UI
{
    public class Menu : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private GameObject[] otherMenus = new GameObject[5];
        [SerializeField] private TextMeshProUGUI text;
        private byte currentPosition;

        private void Update()
        {
            Inputs();

            // Updates the Text Live.
            if(currentPosition == 0)
            {
                text.text = " \n > Pikimin \n\n\n Items \n\n\n Pikidex \n\n\n Trainer Pass \n\n\n Settings \n\n\n Save \n\n\n Exit";
            }
            else if(currentPosition == 1)
            {
                text.text = " \n Pikimin \n\n\n > Items \n\n\n Pikidex \n\n\n Trainer Pass \n\n\n Settings \n\n\n Save \n\n\n Exit";
            }
            else if(currentPosition == 2)
            {
                text.text = " \n Pikimin \n\n\n Items \n\n\n > Pikidex \n\n\n Trainer Pass \n\n\n Settings \n\n\n Save \n\n\n Exit";
            }
            else if(currentPosition == 3)
            {
                text.text = " \n Pikimin \n\n\n Items \n\n\n Pikidex \n\n\n > Trainer Pass \n\n\n Settings \n\n\n Save \n\n\n Exit";
            }
            else if(currentPosition == 4)
            {
                text.text = " \n Pikimin \n\n\n Items \n\n\n Pikidex \n\n\n Trainer Pass \n\n\n > Settings \n\n\n Save \n\n\n Exit";
            }
            else if (currentPosition == 5)
            {
                text.text = " \n Pikimin \n\n\n Items \n\n\n Pikidex \n\n\n Trainer Pass \n\n\n Settings \n\n\n > Save \n\n\n Exit";
            }
            else if (currentPosition == 6)
            {
                text.text = " \n Pikimin \n\n\n Items \n\n\n Pikidex \n\n\n Trainer Pass \n\n\n Settings \n\n\n Save \n\n\n > Exit";
            }
        }

        /// <summary>
        /// Players Inputs.
        /// </summary>
        private void Inputs()
        {
            // Up and Down Movement.
            if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && currentPosition < 6)
            {
                AudioManagerInstance.Sfx.PlayOneShot(AudioManagerInstance.ButtonOverSounds[0]);
                currentPosition++;
            }
            else if((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && currentPosition > 0)
            {
                AudioManagerInstance.Sfx.PlayOneShot(AudioManagerInstance.ButtonOverSounds[0]);
                currentPosition--;
            }
        
            if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E))
            {
                // Activates Pikimin Inventory and closes itself.
                AudioManagerInstance.Sfx.PlayOneShot(AudioManagerInstance.ButtonOverSounds[0]);
                if(currentPosition == 0)
                {
                    otherMenus[0].SetActive(true);
                    gameObject.SetActive(false);
                } 
                else if (currentPosition == 1)
                {
                    otherMenus[1].SetActive(true);
                    gameObject.SetActive(false);
                }
                else if (currentPosition == 2)
                {
                    otherMenus[2].SetActive(true);
                    PikidexInstance.TextManager();
                    gameObject.SetActive(false);
                }
                else if (currentPosition == 3)
                {
                    otherMenus[3].SetActive(true);
                    TrainerPassInstance.UpdateInfo();
                    gameObject.SetActive(false);
                }
                else if (currentPosition == 4)
                {
                    otherMenus[4].SetActive(true);
                    gameObject.SetActive(false);
                }
                else if (currentPosition == 5)
                {
                    otherMenus[5].SetActive(true);
                    DataManagerInstance.infoWasUpdated = true;
                    gameObject.SetActive(false);
                }
                // Closes Game, since it is a beta.
                else if (currentPosition == 6)
                {
                    AudioManagerInstance.Sfx.Stop();
                    AudioManagerInstance.BackgroundSound.Stop();
                    SceneManager.LoadScene(0);
                }
            }

            // Closes Menu by inputting the corresponding button. 
            if(Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Tab))
            {
                CloseMenu();
            }
        }

        public void CloseMenu()
        {
            currentPosition = 0;
            CurrentPlayer.ChangeState(PlayerState.Idle);
            gameObject.SetActive(false);
        }
    }
}
