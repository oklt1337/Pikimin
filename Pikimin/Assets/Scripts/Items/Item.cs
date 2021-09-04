using UnityEngine;
using static Battle.BattleManager;
using static UI.DialogueManager;

namespace Items
{
    public class Item : MonoBehaviour
    {
        [Header("Item Information")]
        [SerializeField] private string itemName;
        [SerializeField] private string itemDescription;
        [SerializeField] private int ownedCopies;
        [SerializeField] private int itemAmount = 1;
        public SpriteRenderer spriteRenderer;

        [Header("Shop Information")]
        [SerializeField] private int itemValue;
        [SerializeField] private bool isRecyclable;

        public string ItemName => itemName;

        public bool IsRecyclable => isRecyclable;

        public int ItemValue => itemValue;

        private string ItemDescription => itemDescription;

        public int OwnedCopies
        {
            get => ownedCopies;
            private set => ownedCopies = value;
        }

        public int ItemAmount => itemAmount;

        public void ShowDescription()
        {
            Dialogue.ManuelDialogue(ItemDescription);
            Dialogue.talkingCharacter = Dialogue.playerName;
        }

        /// <summary>
        /// What the Item does.
        /// </summary>
        public virtual void OnUse()
        {
            OwnedCopies--;

            if(BattleManagerInstance.BattleState == UI.BattleState.PlayerTurn)
            {
                BattleManagerInstance.PlayerWantToUseItem = true;
                BattleManagerInstance.UsedItem = this;
            }
        }

        /// <summary>
        /// Changes the owned copies.
        /// </summary>
        /// <param name="amount"> How many copies are added. </param>
        public void AddItems(int amount)
        {
            OwnedCopies = amount;
        }
    }
}
