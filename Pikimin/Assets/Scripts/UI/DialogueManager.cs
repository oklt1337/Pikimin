using Players;
using System;
using TMPro;
using UnityEngine;
using static Battle.BattleManager;
using static Players.Player;
using UnityEngine.UI;

namespace UI
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Dialogue;

        [Header("Values for the Dialogue taking place")]
        public string talkingCharacter;
        public string[] currentDialogue;
        [SerializeField] private string[] emptyDialogue;
        [SerializeField] private int currentDialogueBox;

        [Header("Important Names")]
        public string playerName;

        [Header("Components")]
        public TextMeshProUGUI dialogue;
        public Image image;
        public GameObject textBox;
        public bool chattingHasEnded;

        private void Awake()
        {
            Dialogue = this;
        }

        private void Start()
        {
            // Preventing infinite errors.
            textBox.SetActive(true);
            image.enabled = true;
            ToggleOnOff();
            CurrentPlayer.ChangeState(PlayerState.Idle);
        }

        private void Update()
        {
            if (CurrentPlayer.CurrentState == PlayerState.Interact)
            {
                // Updating the dialogue instantly.
                dialogue.text = talkingCharacter + ": " + currentDialogue[currentDialogueBox];

                chattingHasEnded = false;
                Inputs();
            }
        }

        private void FixedUpdate()
        {
            // Changes State after checking players state.
            if (CurrentPlayer.CurrentState != PlayerState.Interact && CurrentPlayer.CurrentState != PlayerState.Fight)
            {
                CurrentPlayer.ChangeState(PlayerState.Interact);
            }
        }

        /// <summary>
        /// What Buttons the Player presses.
        /// </summary>
        private void Inputs()
        {
            chattingHasEnded = false;
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Q))
            {
                if (currentDialogue.Length > currentDialogueBox + 1)
                {
                    currentDialogueBox++;
                }
                else
                {
                    EndDialogue();
                }
            }
        }

        /// <summary>
        /// Finishes the Dialogue and resets every important value.
        /// </summary>
        private void EndDialogue()
        {
            if(CurrentPlayer.CurrentState == PlayerState.Interact)
            {
                CurrentPlayer.ChangeState(PlayerState.Idle);
                ToggleOnOff();
            }
            currentDialogueBox = 0;
            talkingCharacter = "Dummy";
            currentDialogue = emptyDialogue;
            chattingHasEnded = true;

            //Workaround for menu fix in npc battle.
            if (BattleManagerInstance.BattleState != BattleState.Idle)
            {
                CurrentPlayer.ChangeState(PlayerState.Fight);
            }
        }

        /// <summary>
        /// Manages the dialogue in battle based on the battle state.
        /// </summary>
        public void BattleDialogueManager()
        {
            Array.Resize(ref currentDialogue, 2);

            if (BattleManagerInstance.BattleState == BattleState.Start)
            {
                OnBattleStart();
            } 
            else if(BattleManagerInstance.BattleState == BattleState.Won)
            {
                OnWin();
            }
            else if(BattleManagerInstance.BattleState == BattleState.Lost)
            {
                OnLoose();
            }

            dialogue.text = currentDialogue[currentDialogueBox];
            Inputs();
        }

        /// <summary>
        /// Manages the dialogue in battle based on battle state and attack values.
        /// </summary>
        /// <param name="didHit"> Did the Attack hit? </param>
        /// <param name="isEffective"> The effectivity of the attack. </param>
        public void BattleDialogueManager(bool didHit, bool? isEffective)
        {
            Array.Resize(ref currentDialogue, 2);

            if (BattleManagerInstance.BattleState == BattleState.PlayerTurn)
            {
                OnPlayerTurn(didHit, isEffective);
            }
            else if (BattleManagerInstance.BattleState == BattleState.EnemyTurn)
            {
                OnEnemyTurn(didHit, isEffective);
            }

            dialogue.text = currentDialogue[currentDialogueBox];
            Inputs();
        }

        /// <summary>
        /// Dialogue for when the battle starts.
        /// </summary>
        private void OnBattleStart()
        {
            if(!BattleManagerInstance.IsNpcFight)
            {
                currentDialogue[0] = "A wild " + BattleManagerInstance.ActiveEnemyPikimin.PikiminName + " appeared.";
                currentDialogue[1] = "You are using " + BattleManagerInstance.ActivePlayerPikimin.PikiminName + ".";
            }
            else
            {
                currentDialogue[0] = BattleManagerInstance.Npc.CharacterName + " challenges you to a fight." + BattleManagerInstance.Npc.CharacterName + " starts using " + BattleManagerInstance.ActiveEnemyPikimin.PikiminName + ".";
                currentDialogue[1] = "You are using " + BattleManagerInstance.ActivePlayerPikimin.PikiminName + ".";
            }
        }

       

        private void OnPlayerTurn(bool didHit, bool? isEffective)
        {
            if (!BattleManagerInstance.PlayerHasNoPp)
            {
                currentDialogue[0] = BattleManagerInstance.ActivePlayerPikimin.PikiminName + " uses " + BattleManagerInstance.PlayerMove.MoveName + ".";
            }
            else
            {
                currentDialogue[0] = BattleManagerInstance.ActivePlayerPikimin.PikiminName + " uses Struggle.";
            }

            if(didHit && isEffective == true) 
            {
                currentDialogue[1] = BattleManagerInstance.ActiveEnemyPikimin.PikiminName + " was hit and the move was very effective!";
            }
            else if (didHit && isEffective == false)
            {
                currentDialogue[1] = BattleManagerInstance.ActiveEnemyPikimin.PikiminName + " was hit, but the move wasn't very effective!";
            }
            else if(didHit && isEffective == null)
            {
                currentDialogue[1] = BattleManagerInstance.ActiveEnemyPikimin.PikiminName + " was hit!";
            }
            else if (!didHit)
            {
                currentDialogue[1] = "The attack missed.";
            }
        }

        private void OnEnemyTurn(bool didHit, bool? isEffective)
        {
            if (!BattleManagerInstance.IsNpcFight)
            {
                currentDialogue[0] = "The wild " + BattleManagerInstance.ActiveEnemyPikimin.PikiminName + " uses " + BattleManagerInstance.NpcMove.MoveName + ".";
            }
            else
            {
                currentDialogue[0] = BattleManagerInstance.Npc.CharacterName + "s " + BattleManagerInstance.ActiveEnemyPikimin.PikiminName + " uses " + BattleManagerInstance.NpcMove.MoveName + ".";
            }

            if (didHit && isEffective == true)
            {
                currentDialogue[1] = "Your " + BattleManagerInstance.ActivePlayerPikimin.PikiminName + " was hit and the move was very effective!";
            }
            else if (didHit && isEffective == false)
            {
                currentDialogue[1] = "Your " + BattleManagerInstance.ActivePlayerPikimin.PikiminName + " was hit, but the move wasn't very effective!";
            }
            else if (didHit && isEffective == null)
            {
                currentDialogue[1] = "Your " + BattleManagerInstance.ActivePlayerPikimin.PikiminName + " was hit!";
            }
            else if (!didHit)
            {
                currentDialogue[1] = "The attack missed.";
            }
        }

        private void OnWin() 
        {
            if (!BattleManagerInstance.IsNpcFight)
            {
                currentDialogue[0] = "You beat the wild " + BattleManagerInstance.ActiveEnemyPikimin.PikiminName + ".";
                currentDialogue[1] = "Your " + BattleManagerInstance.ActivePlayerPikimin.PikiminName + " got " + BattleManagerInstance.GainedExp + " Exp."; 
            }
            else
            {
                currentDialogue[0] = "You beat " + BattleManagerInstance.Npc.CharacterName + ".";
                currentDialogue[1] = "They give you " + BattleManagerInstance.Npc.Money + " $ and your " + BattleManagerInstance.ActivePlayerPikimin.PikiminName + " got " + BattleManagerInstance.GainedExp + " Exp.";
            }
        }

        private void OnLoose() 
        {
            if (!BattleManagerInstance.IsNpcFight)
            {
                currentDialogue[0] = "The wild " + BattleManagerInstance.ActiveEnemyPikimin.PikiminName + " beat you. You have no Pikimin left...";
            }
            else
            {
                currentDialogue[0] = BattleManagerInstance.Npc.CharacterName + " beat you with their " + BattleManagerInstance.ActiveEnemyPikimin.PikiminName + " and took " + Convert.ToInt32(BattleManagerInstance.Npc.Money * 0.5) + "$. You have no Pikimin left...";
            }
        }
        
        /// <summary>
        /// Shows manually input dialogue.
        /// </summary>
        /// <param name="text"> The Dialogue you want to be shown. </param>
        public void ManuelDialogue(string text)
        {
            Array.Resize(ref currentDialogue, 1);
            currentDialogue[0] = text;
            dialogue.text = currentDialogue[0];
            Inputs();
        }

        /// <summary>
        /// (De-) Activates the dialogue box.
        /// </summary>
        public void ToggleOnOff()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}
