using Players;
using TMPro;
using UnityEngine;
using static Audio.AudioManager;
using static Battle.BattleManager;
using static Players.Player;
using static UI.BattleUIManager;

namespace UI
{
    public class OtherAuthorizeMenu : MonoBehaviour
    {
        public TextMeshProUGUI text;
        public bool wantsToGetStatus;
        public bool wantsToSwap;
        [SerializeField]private byte currentPosition; 

        private void Update()
        {
            if (!wantsToSwap)
            {
                if (currentPosition == 0)
                {
                    text.text = " > Status \n Swap \n Exit";
                }
                else if (currentPosition == 1)
                {
                    text.text = " Status \n > Swap \n Exit";
                }
                else if (currentPosition == 2)
                {
                    text.text = " Status \n Swap \n > Exit";
                }
            } 
            else if (wantsToSwap)
            {
                text.text = "Please choose the Pikimin you want to swap with.";
            }

            Inputs();
        }

        /// <summary>
        /// Checks the Inputs of the Player.
        /// </summary>
        private void Inputs()
        {
            if (!wantsToSwap)
            {
                // Up and Down Movement.
                if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && currentPosition < 2)
                {
                    AudioManagerInstance.Sfx.PlayOneShot(AudioManagerInstance.ButtonOverSounds[0]);
                    currentPosition++;
                }
                else if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && currentPosition > 0)
                {
                    AudioManagerInstance.Sfx.PlayOneShot(AudioManagerInstance.ButtonOverSounds[0]);
                    currentPosition--;
                }
            }

            if (CurrentPlayer.CurrentState != PlayerState.Fight)
            {
                // Choosing the option.
                if((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E)) && currentPosition == 0)
                {
                    AudioManagerInstance.Sfx.PlayOneShot(AudioManagerInstance.ButtonOverSounds[0]);
                    wantsToGetStatus = true;
                    gameObject.SetActive(false);
                }
                else if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E)) && currentPosition == 1)
                {
                    AudioManagerInstance.Sfx.PlayOneShot(AudioManagerInstance.ButtonOverSounds[0]);
                    wantsToSwap = true;
                }
                else if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E)) && currentPosition == 2)
                {
                    AudioManagerInstance.Sfx.PlayOneShot(AudioManagerInstance.ButtonOverSounds[0]);
                    currentPosition = 0;
                    gameObject.SetActive(false);
                }
            }
            else if (CurrentPlayer.CurrentState == PlayerState.Fight && BattleManagerInstance.PlayerWantToSwapPikimin == false)
            {
                // Choosing the option.
                if((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E)) && currentPosition == 0)
                {
                    AudioManagerInstance.Sfx.PlayOneShot(AudioManagerInstance.ButtonOverSounds[0]);
                    CurrentBattleUIManager.ChangeMenu(BattleMenu.PikiminStatus);
                }
                else if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E)) && currentPosition == 1)
                {
                    AudioManagerInstance.Sfx.PlayOneShot(AudioManagerInstance.ButtonOverSounds[0]);
                    CurrentBattleUIManager.ChangePikimin.Invoke(true);
                    currentPosition = 0;
                }
                else if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E)) && currentPosition == 2)
                {
                    AudioManagerInstance.Sfx.PlayOneShot(AudioManagerInstance.ButtonOverSounds[0]);
                    currentPosition = 0;
                    CurrentBattleUIManager.ChangeMenu(BattleMenu.Pikimin);
                }
            }
        }
    }
}
