using System.Collections;
using SaveData;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Audio.AudioManager;

namespace UI
{
    public class StartScreen : MonoBehaviour
    {
        [SerializeField] private byte currentPosition;
        [SerializeField] private TextMeshProUGUI[] text;
        [SerializeField] private GameObject settings;
        [SerializeField] private float delay;

        private void Start()
        {
            if (!AudioManagerInstance.BackgroundSound.isPlaying && AudioManagerInstance.BackgroundSound.clip != AudioManagerInstance.LeftOverSounds[0])
            {
                AudioManagerInstance.BackgroundSound.clip = AudioManagerInstance.LeftOverSounds[0];
                AudioManagerInstance.BackgroundSound.Play();
            }
            
            currentPosition = 0;
            text[currentPosition].color = Color.white;
            
            if (!SaveManager.CanLoadSaveData())
            {
                text[1].color = Color.red;
            }
        }

        void Update()
        {
            Inputs();
        }

        private void Inputs()
        {
            // Going up.
            if((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && currentPosition > 0)
            {
                
                text[currentPosition].color = Color.black;
                currentPosition--;
                text[currentPosition].color = Color.white;
                if (!SaveManager.CanLoadSaveData())
                {
                    text[1].color = Color.red;
                }
            }

            // Going Down.
            if((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && currentPosition < 5)
            {
               
                
                text[currentPosition].color = Color.black;
                currentPosition++;
                text[currentPosition].color = Color.white;
                if (!SaveManager.CanLoadSaveData())
                {
                    text[1].color = Color.red;
                }
            }

            if(Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
            {
                if (currentPosition == 0 && !SaveManager.CanLoadSaveData())
                {
                    SceneManager.LoadScene(4);
                }
                else if (currentPosition == 1 && SaveManager.CanLoadSaveData())
                {
                    AudioManagerInstance.BackgroundSound.Stop();
                    GameManager.Instance.continueGame = true;
                    SceneManager.LoadScene(4);
                }
                else if (currentPosition == 2) 
                {
                    if (SaveManager.CanLoadSaveData())
                    {
                        StartCoroutine(DeletedFiles(delay));
                    }
                    SaveManager.DeleteSaveFiles();
                    if (!SaveManager.CanLoadSaveData())
                    {
                        text[1].color = Color.red;
                    }
                }
                else if (currentPosition == 3) 
                {
                    settings.SetActive(true);
                }
                else if (currentPosition == 4) 
                {
                    SceneManager.LoadScene(3);
                }
                else if (currentPosition == 5)
                {
                    Application.Quit();
                }
            }
        }

        private IEnumerator DeletedFiles(float delay)
        {
            text[6].gameObject.SetActive(true);

            yield return new WaitForSeconds(delay);
            
            text[6].gameObject.SetActive(false);
        }
    }
}
