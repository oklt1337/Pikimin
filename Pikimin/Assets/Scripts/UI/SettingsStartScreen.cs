using Audio;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingsStartScreen : MonoBehaviour
    {
        [SerializeField] private Slider volume;
        [SerializeField] private Image image;
        [SerializeField] private GameObject settings;

        // Start is called before the first frame update
        void Start()
        {
            if (!AudioManager.AudioManagerInstance.BackgroundSound.isPlaying)
            {
                volume.value = .333f;
                AudioManager.AudioManagerInstance.SetLevel(volume.value);
            }
            
            image.enabled = true;
            settings.SetActive(true);
            gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.RightArrow) ||
                Input.GetKey(KeyCode.UpArrow))
            {
                volume.value += 0.1f * Time.deltaTime;
                AudioManager.AudioManagerInstance.SetLevel(volume.value);
            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.LeftArrow) ||
                     Input.GetKey(KeyCode.DownArrow))
            {
                volume.value -= 0.1f * Time.deltaTime;
                AudioManager.AudioManagerInstance.SetLevel(volume.value);
            }

            if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Backspace))
            {
                gameObject.SetActive(false);
            }
            else if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Tab))
            {
                gameObject.SetActive(false);
            }
        }

        private void OnDisable()
        {
            AudioManager.AudioManagerInstance.audioValue = volume.value;
        }
    }
}
