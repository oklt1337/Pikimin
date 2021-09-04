using Battle;
using Players;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChampionshipManager : MonoBehaviour
{
    [Header("Trainers")]
    [SerializeField] private TrainerBehaviour firstTrainer;
    [SerializeField] private TrainerBehaviour secondTrainer;
    [SerializeField] private TrainerBehaviour rival;

    [Header("Bools")]
    [SerializeField] private bool beatFirstTrainer;
    [SerializeField] private bool beatSecondTrainer;
    [SerializeField] private bool finishedGame;
    [SerializeField] public bool didSaveGame;

    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private GameObject saveBox;
    
    [SerializeField] private float delay;
    

    private void Start()
    {
        delay = 5f;
        firstTrainer.gameObject.SetActive(true);
    }

    private void Update()
    {
        // After beating the first Trainer.
        if (firstTrainer.HasBeenBeaten && !beatFirstTrainer)
        {
            BattleManager.BattleManagerInstance.ONPlayerDeath.Invoke();
            beatFirstTrainer = true;
            firstTrainer.gameObject.SetActive(false);
            secondTrainer.gameObject.SetActive(true);
        }

        // After beating the second Trainer.
        if (secondTrainer.HasBeenBeaten && !beatSecondTrainer)
        {
            BattleManager.BattleManagerInstance.ONPlayerDeath.Invoke();
            beatSecondTrainer = true;
            secondTrainer.gameObject.SetActive(false);
            rival.gameObject.SetActive(true);
        }

        // After beating the Rival.
        if (rival.HasBeenBeaten && !finishedGame)
        {
            delay -= Time.deltaTime;
            
            victoryScreen.SetActive(true);
            Player.CurrentPlayer.PutToStart();

            if (delay < 0)
            {
                if (!saveBox.activeSelf && !didSaveGame)
                {
                    saveBox.SetActive(true);
                }

                if (Input.anyKey && didSaveGame)
                {
                    SceneManager.LoadScene(3);
                }
            }
        }
    }
}