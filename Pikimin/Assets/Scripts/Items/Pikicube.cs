using UnityEngine;
using Pikimins;
using static Players.PikiminInventory;
using static Battle.BattleManager;
using static Players.Player;

namespace Items
{
    public class Pikicube : Item
    {
        [Header("Catching")]
        [SerializeField] private float catchStrength;
        [SerializeField] private float catchChance;
        
        [Header("Enemy Animation")]
        [SerializeField] private RuntimeAnimatorController animatorControllerPikicube;
        [SerializeField] public AnimationClip caught;
        [SerializeField] public AnimationClip failedCatch;

        public RuntimeAnimatorController AnimatorControllerPikicube => animatorControllerPikicube;
        
        
        
        /// <summary>
        /// Catches the Pikimin.
        /// </summary>
        public override void OnUse()
        {
            // Checks status of domestication. 
            if (!BattleManagerInstance.IsNpcFight && CurrentPlayer.CurrentState == Players.PlayerState.Fight)
            {
                CalculateCatchChance();

                // Only catches when works.
                if (Random.Range(1,100) < catchChance)
                {  
                    playerPikiminInventory.OnNewPikimin(BattleManagerInstance.ActiveEnemyPikimin);
                }
                else
                {
                    BattleManagerInstance.WildPikiminBrokeOut = true;
                }
                base.OnUse();
            }
        }

        /// <summary>
        /// Calculates the chance of Catching the Pikimin. 
        /// </summary>
        private void CalculateCatchChance()
        {
            if (BattleManagerInstance.ActiveEnemyPikimin.MyRarity == Pikimin.Rarity.VeryCommon)
            {
                // ReSharper disable once PossibleLossOfFraction
                catchChance = catchStrength + (BattleManagerInstance.ActiveEnemyPikimin.PikiminStats.FullHp / BattleManagerInstance.ActiveEnemyPikimin.PikiminStats.CurrentHp);
            }
            else if (BattleManagerInstance.ActiveEnemyPikimin.MyRarity == Pikimin.Rarity.Common)
            {
                // ReSharper disable once PossibleLossOfFraction
                catchChance = (catchStrength + (BattleManagerInstance.ActiveEnemyPikimin.PikiminStats.FullHp / BattleManagerInstance.ActiveEnemyPikimin.PikiminStats.CurrentHp))*4/5;
            }
            else if (BattleManagerInstance.ActiveEnemyPikimin.MyRarity == Pikimin.Rarity.Uncommon)
            {
                // ReSharper disable once PossibleLossOfFraction
                catchChance = (catchStrength + (BattleManagerInstance.ActiveEnemyPikimin.PikiminStats.FullHp / BattleManagerInstance.ActiveEnemyPikimin.PikiminStats.CurrentHp))*3/5;
            }
            else if (BattleManagerInstance.ActiveEnemyPikimin.MyRarity == Pikimin.Rarity.Rare)
            {
                // ReSharper disable once PossibleLossOfFraction
                catchChance = (catchStrength + (BattleManagerInstance.ActiveEnemyPikimin.PikiminStats.FullHp / BattleManagerInstance.ActiveEnemyPikimin.PikiminStats.CurrentHp)) * 2 / 5;
            }
            else if (BattleManagerInstance.ActiveEnemyPikimin.MyRarity == Pikimin.Rarity.VeryRare)
            {
                // ReSharper disable once PossibleLossOfFraction
                catchChance = (catchStrength + (BattleManagerInstance.ActiveEnemyPikimin.PikiminStats.FullHp / BattleManagerInstance.ActiveEnemyPikimin.PikiminStats.CurrentHp)) / 5;
            }
        }
    }
}
