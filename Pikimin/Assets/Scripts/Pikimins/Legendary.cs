using Items;
using Players;
using UnityEngine;
using static Audio.AudioManager;
using static Battle.BattleManager;
using static Map.Regions;
using static Players.Player;
using static UI.DialogueManager;

namespace Pikimins
{
    public class Legendary : MonoBehaviour
    {
        [SerializeField] private GameObject legendary;
        [SerializeField] private Pikimin legendaryPikiminPrefab;
        [SerializeField] private SpriteRenderer legendaryPikiminSprite;
        
        [SerializeField] private string saidDialogue;

        [SerializeField] private BiomeList biomeList;
        
        [SerializeField] private bool isBeingChallenged;

        private void OnTriggerEnter2D(Collider2D collison)
        {
            PlayerController.CurrentPlayerController.StopPlayer();
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("PlayerTrigger") && collision.isTrigger && !isBeingChallenged)
            {
                if (collision.gameObject.transform.position == gameObject.transform.position)
                {
                    Dialogue.talkingCharacter = legendaryPikiminPrefab.PikiminName;
                    Dialogue.ManuelDialogue(saidDialogue);
                    Dialogue.gameObject.SetActive(true);
                    
                    if (CurrentPlayer.CurrentState != PlayerState.Fight)
                    {
                        isBeingChallenged = true;
                    }
                }
            }
        }

        private void Start()
        {
            BattleManagerInstance.OnLegendaryBattleEnd += Deactivate;
            legendaryPikiminSprite.sprite = legendaryPikiminPrefab.FrontSprite;
        }

        private void Update()
        {
            if (Dialogue.chattingHasEnded && isBeingChallenged)
            {
                Dialogue.chattingHasEnded = false;
                isBeingChallenged = false;
                BattleManagerInstance.OnBattleStart?.Invoke(legendaryPikiminPrefab, null, Pikimin.Rarity.Npc, biomeList, 0, 0, AudioManagerInstance.TrainerSounds[1]);
            }
        }
        
        private void Deactivate()
        {
            if (legendaryPikiminPrefab.PikiminName == "Groudon")
            {
                ItemManager.ItemManagerInstance.wasTaken[LegendaryManager.LegendaryManagerInstance.groudonIndi] = true;
            }
            else if (legendaryPikiminPrefab.PikiminName == "Kyogre")
            {
                ItemManager.ItemManagerInstance.wasTaken[LegendaryManager.LegendaryManagerInstance.kyogreIndi] = true;
            }
            else if (legendaryPikiminPrefab.PikiminName == "Rayquaza")
            {
                ItemManager.ItemManagerInstance.wasTaken[LegendaryManager.LegendaryManagerInstance.rayIndi] = true;
            }
            legendary.SetActive(false);
        }
    }
}
