using Audio;
using Players;
using UI;
using UnityEngine;

public class NurseBehaviour : NpcBehaviour
{
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.isTrigger)
        {
            DialogueManager.Dialogue.chattingHasEnded = false;
            DialogueManager.Dialogue.ToggleOnOff();
            DialogueManager.Dialogue.talkingCharacter = CharacterName;
            DialogueManager.Dialogue.currentDialogue = SaidDialogue;

            AudioManager.AudioManagerInstance.PlayAudioClip(true, AudioManager.AudioManagerInstance.LeftOverSounds[2]);

            // Revives and full heals Players Pikimin.
            foreach (var t in PikiminInventory.playerPikiminInventory.OwnedPikimin)
            {
                if (t != null)
                {
                    t.Heal(9999, true, true);
                }
            }
        }
    }
}