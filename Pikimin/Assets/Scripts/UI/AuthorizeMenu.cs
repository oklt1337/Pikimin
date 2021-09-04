using TMPro;
using UnityEngine;

namespace UI
{
    public class AuthorizeMenu : MonoBehaviour
    {
        public TextMeshProUGUI text;
        public bool playerHasAuthorized;
        private bool wantsHisChoice;

        public bool WantsHisChoice => wantsHisChoice;

        private void Start()
        {
            playerHasAuthorized = false;
        }

        private void Update()
        {
            // Changes the Text according to the state of the bool.
            if (wantsHisChoice)
            {
                text.text = " > Yes \n No";
            }
            else
            {
                text.text = " Yes \n > No";
            }

            Inputs();
        }

        /// <summary>
        /// Checks Players Inputs and 
        /// </summary>
        private void Inputs()
        {
            // Players Inputs.
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                wantsHisChoice = false;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                wantsHisChoice = true;
            }

            if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E))
            {
                playerHasAuthorized = true;
                gameObject.SetActive(false);
            }

            if(Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Backspace))
            {
                playerHasAuthorized = true;
                wantsHisChoice = false;
                gameObject.SetActive(false);
            }
        }
    }
}
