using Audio;
using Players;
using SaveData;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Settings : MonoBehaviour
    {
        [SerializeField] private Slider volume;
        [SerializeField] private GameObject menu;
        [SerializeField] private GameObject settings;
        [SerializeField] private Image image;

        public Slider Volume => volume;

        // Start is called before the first frame update
        void Start()
        {
            volume.value = AudioManager.AudioManagerInstance.audioValue;
            AudioManager.AudioManagerInstance.SetLevel(volume.value);
            gameObject.SetActive(false);
            image.enabled = true;
            settings.SetActive(true);

            LoadData.LoadDataInstance.OnLoad += LoadSettings;
            GameManager.Instance.OnSave += SaveSettings;
        }

        private void Update()
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
                menu.SetActive(true);
                gameObject.SetActive(false);
            }
            else if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape))
            {
                Player.CurrentPlayer.ChangeState(PlayerState.Idle);
                gameObject.SetActive(false);
            }
        }

        private void LoadSettings()
        {
            SettingsData data = SaveManager.LoadSettings();

            volume.value = data.volume;
            AudioManager.AudioManagerInstance.SetLevel(volume.value);
        }

        private void SaveSettings()
        {
            SaveManager.SaveSettings(this);
        }
    }
}
