using Players;
using UnityEngine;
using static Audio.AudioManager;
using static UI.DialogueManager;
using static Players.PlayerController;

public class NpcVision : MonoBehaviour
{
    [SerializeField] private TrainerBehaviour trainer;


    private void OnTriggerEnter2D(Collider2D collison)
    {
        CurrentPlayerController.StopPlayer();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!trainer.HasBeenBeaten && !trainer.IsBeingChallenged && collision.isTrigger && collision.CompareTag("PlayerTrigger"))
        {
            if (collision.gameObject.transform.position == gameObject.transform.position)
            {
                Dialogue.chattingHasEnded = false;
                if (!Player.CurrentPlayer.IsSurfing)
                {
                    CurrentPlayerController.ResetAnimations();
                }
                
                trainer.IsBeeingChallenged();
                
                if (trainer.rivalChapterSound) 
                {
                    AudioManagerInstance.PlayAudioClip(false, AudioManagerInstance.RivalSounds[0]);

                    if (Dialogue.chattingHasEnded)
                    {
                        AudioManagerInstance.BackgroundSound.Pause();
                    }
                }
            }
        }
    }
}
