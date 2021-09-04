using UnityEngine;
using static UI.DialogueManager;

public class NpcBehaviour : MonoBehaviour
{
    [Header("Dialogue Values")]
    [SerializeField] private string[] saidDialogue;
    [SerializeField] private string characterName;

    public string CharacterName => characterName;

    protected string[] SaidDialogue
    {
        get => saidDialogue;
        set => saidDialogue = value; 
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.isTrigger)
        {
            Dialogue.chattingHasEnded = false;
            Dialogue.ToggleOnOff();
            Dialogue.talkingCharacter = CharacterName;
            Dialogue.currentDialogue = SaidDialogue;
        }
    }
}
