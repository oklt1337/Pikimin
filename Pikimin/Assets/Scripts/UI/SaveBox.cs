using Audio;
using UnityEngine;

namespace UI
{
    public class SaveBox : MonoBehaviour
    {
        [SerializeField]private float timer;
        [SerializeField] private Menu menu;
        [SerializeField] private GameObject victoryScreen;
        [SerializeField] private ChampionshipManager manager;

        private void Start()
        {
            timer = 2.5f;
        }

        // Update is called once per frame
        void Update()
        {
            timer -= Time.deltaTime;
            if(timer < 0)
            {
                timer = 2.5f;
                GameManager.Instance.OnSave?.Invoke();
                AudioManager.AudioManagerInstance.Sfx.PlayOneShot(AudioManager.AudioManagerInstance.ButtonOverSounds[1]);
                menu.CloseMenu();
                
                if (victoryScreen.activeSelf)
                {
                    manager.didSaveGame = true;
                }
                
                gameObject.SetActive(false);
            }
        }
    }
}
