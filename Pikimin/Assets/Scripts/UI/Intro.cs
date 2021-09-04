using Audio;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class Intro : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private string[] introText;
        [SerializeField] private byte currentPosition;

        private void Start()
        {
            AudioManager.AudioManagerInstance.BackgroundSound.clip = AudioManager.AudioManagerInstance.LeftOverSounds[7];
            AudioManager.AudioManagerInstance.BackgroundSound.Play();
        }

        private void Update()
        {
            text.text = introText[currentPosition];
            Inputs();
            if (currentPosition == 3)
            {
                AudioManager.AudioManagerInstance.BackgroundSound.Stop();
                SceneManager.LoadScene(2);
            }
        }

        private void Inputs()
        {
            if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E))
            {
                if(currentPosition < introText.Length)
                {
                    currentPosition++;
                }
            }
        }
    }
}
