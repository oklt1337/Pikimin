using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Players.Player;
using static Players.ItemInventory;
using static Audio.AudioManager;


namespace UI
{
    public class OtherOtherAuthorize : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Image itemPicture;
        [SerializeField] private byte currentPosition;
        public bool wantsToUse;
        public bool wantsToRecycle;
        public bool wantsToSeeDescription;


        private void Update()
        {
            TextManager();
            Inputs();
        }

        /// <summary>
        /// Checks Inputs of the Player.
        /// </summary>
        private void Inputs()
        {
            if(CurrentPlayer.CurrentState == Players.PlayerState.Interact)
            {
                // Up and Down Movement.
                if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && currentPosition < 3)
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
            else if (CurrentPlayer.CurrentState == Players.PlayerState.Fight)
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
        

            // Players Choice.
            if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E))
            {
                AudioManagerInstance.Sfx.PlayOneShot(AudioManagerInstance.ButtonOverSounds[0]);
                if(currentPosition == 0) 
                {
                    wantsToUse = true;
                    gameObject.SetActive(false);
                }
                else if(currentPosition == 1)
                {
                    wantsToRecycle = true;
                    currentPosition = 0;
                    gameObject.SetActive(false);
                }
                else if (currentPosition == 2 && CurrentPlayer.CurrentState == Players.PlayerState.Interact)
                {
                    currentPosition = 0;
                    wantsToSeeDescription = true;
                    gameObject.SetActive(false);
                }
                else if (currentPosition == 2 && CurrentPlayer.CurrentState == Players.PlayerState.Fight)
                {
                    currentPosition = 0;
                    gameObject.SetActive(false);
                }
                else if(currentPosition == 3)
                {
                    currentPosition = 0;
                    gameObject.SetActive(false);
                }
            }
        }

        private void TextManager()
        {
            if(CurrentPlayer.CurrentState == Players.PlayerState.Interact)
            {
                if (currentPosition == 0)
                {
                    text.text = " > Use \n Recycle \n Description \n Exit";
                }
                else if (currentPosition == 1)
                {
                    text.text = " Use \n > Recycle \n Description \n Exit";
                }
                else if (currentPosition == 2)
                {
                    text.text = " Use \n Recycle \n > Description \n Exit";
                }
                else if (currentPosition == 3)
                {
                    text.text = " Use \n Recycle \n Description \n > Exit";
                }
            }
            else if(CurrentPlayer.CurrentState == Players.PlayerState.Fight)
            {
                if (currentPosition == 0)
                {
                    text.text = " > Use \n Recycle \n Exit";
                }
                else if (currentPosition == 1)
                {
                    text.text = " Use \n > Recycle \n Exit";
                }
                else if (currentPosition == 2)
                {
                    text.text = " Use \n Recycle \n > Exit";
                }
            }

            itemPicture.sprite = playerItems.OwnedItems[playerItems.CurrentPosition].spriteRenderer.sprite;
        }
    }
}
