using Pikimins;
using Players;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Players.Player;
using static UI.BattleUIManager;


namespace UI
{
    public class CurrentPikiminStatus : MonoBehaviour
    {
        [Header("Components")]
        public Pikimin currentPikimin;
        [SerializeField] private GameObject pikiminInventory;

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI pikiminName;
        [SerializeField] private TextMeshProUGUI pikiminLevel;
        [SerializeField] private TextMeshProUGUI pikiminDescription;
        [SerializeField] private TextMeshProUGUI pikiminHp;
        [SerializeField] private TextMeshProUGUI pikiminExp;
        [SerializeField] private TextMeshProUGUI pikiminStats;
        [SerializeField] private TextMeshProUGUI pikiminType;
        [SerializeField] private TextMeshProUGUI pikiminMovesName;
        [SerializeField] private TextMeshProUGUI pikiminMovesType;
        [SerializeField] private TextMeshProUGUI pikiminMovesPp;
        [SerializeField] private Image pikiSprite;
        [SerializeField] private Slider hpBar;
        [SerializeField] private Slider expBar;

        private void Update()
        {
            pikiminName.text = currentPikimin.PikiminName;
            pikiminLevel.text = "Lv. " + currentPikimin.Level;
            pikiminDescription.text = currentPikimin.Description;
            pikiminHp.text = currentPikimin.PikiminStats.CurrentHp + "/" + currentPikimin.PikiminStats.FullHp;
            pikiminExp.text = (currentPikimin.Exp - currentPikimin.CalculateExpForLevelUp(currentPikimin.Level - 1)) + "/" + (currentPikimin.ExpForNextLevel - currentPikimin.CalculateExpForLevelUp(currentPikimin.Level - 1));
            pikiminStats.text = "Attack:    " + currentPikimin.PikiminStats.AttackStat
                                              + "\n\nDefense:   " + currentPikimin.PikiminStats.DefenseStat
                                              + "\n\nSpeed:     " + currentPikimin.PikiminStats.SpeedStat;
            pikiminType.text = "Type: " + currentPikimin.PikiType;
            pikiSprite.sprite = currentPikimin.FrontSprite;
            hpBar.maxValue = currentPikimin.PikiminStats.FullHp;
            hpBar.minValue = 0;
            hpBar.value = currentPikimin.PikiminStats.CurrentHp;
            expBar.maxValue = currentPikimin.ExpForNextLevel;
            expBar.minValue = currentPikimin.CalculateExpForLevelUp(currentPikimin.Level - 1);
            expBar.value = currentPikimin.Exp;
            pikiminMovesName.text = "Moves\n";
            pikiminMovesType.text = "Types\n";
            pikiminMovesPp.text = "PP\n";


            for (int i = 0; i < 4; i++)
            {
                if (currentPikimin.CurrentMoves[i] != null)
                {
                    pikiminMovesName.text += currentPikimin.CurrentMoves[i].MoveName + "\n\n";
                    pikiminMovesType.text += currentPikimin.CurrentMoves[i].MoveType + "\n\n";
                    pikiminMovesPp.text += currentPikimin.CurrentMoves[i].CurrentPp + "/" + currentPikimin.CurrentMoves[i].MAXPp + "\n\n";
                }
                else
                {
                    pikiminMovesName.text += "------------" + "\n\n";
                    pikiminMovesType.text += "--------" + "\n\n";
                    pikiminMovesPp.text += "-----" + "\n\n";
                }
            }



            if (CurrentPlayer.CurrentState != PlayerState.Fight)
            {
                if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.P))
                {
                    CurrentPlayer.ChangeState(PlayerState.Idle);
                    gameObject.SetActive(false);
                }
                else if(Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Q))
                {
                    pikiminInventory.SetActive(true);
                    gameObject.SetActive(false);
                }
            }
            else if (CurrentPlayer.CurrentState == PlayerState.Fight)
            { 
                if(Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Q))
                {
                    CurrentBattleUIManager.ChangeMenu(BattleMenu.Pikimin);
                }
            }
        }
    }
}
