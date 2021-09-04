using Players;
using static Map.Regions;
using static Pikimins.Pikimin;
using UnityEngine;
using static Audio.AudioManager;
using Random = UnityEngine.Random;
using static Players.Player;
using static Battle.BattleManager;

namespace Triggers
{
    public class LongGrass : MonoBehaviour
    {
        public BiomeList biomeList;
        public bool isSpecial;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (CurrentPlayer.IsProtected ||
                (CurrentPlayer.CurrentState != PlayerState.Idle && CurrentPlayer.CurrentState != PlayerState.Walk) ||
                other.isTrigger || CurrentPlayer.CurrentState == PlayerState.Fight) return;

            RegionsInstance.PikiminLvlAtRegion(CurrentPlayer.PlayerRegion, out var minLvl, out var maxLvl);
            Rarity rarity = CalculateRarity(isSpecial);

            if (rarity != Rarity.Npc)
            {
                BattleManagerInstance.OnBattleStart?.Invoke(null, null, rarity, biomeList, minLvl,
                    maxLvl, AudioManagerInstance.TrainerSounds[1]);
            }
        }

        public static Rarity CalculateRarity(bool special)
        {
            const float veryCommon = 10f / 187.5f;
            const float common = 8.5f / 187.5f;
            const float unCommon = 6.75f / 187.5f;
            const float rare = 3.33f / 187.5f;
            const float veryRare = .63f / 187.5f;

            var p = Random.Range(0.0f, 100.0f);
            
            switch (special)
            {
                case false when p < veryRare * 100f:
                {
                    if (BattleManagerInstance != null)
                    {
                        return Rarity.VeryRare;
                    }
                    break;
                }
                case false when p < rare * 100f:
                {
                    if (BattleManagerInstance != null)
                    {
                        return Rarity.Rare;
                    }
                    break;
                }
                case false when p < unCommon * 100f:
                {
                    if (BattleManagerInstance != null)
                    {
                        return Rarity.Uncommon;
                    }
                    break;
                }
                case false when p < common * 100f:
                {
                    if (BattleManagerInstance != null)
                    {
                        return Rarity.Common;
                    }
                    break;
                }
                case false:
                {
                    if (p < veryCommon * 100f)
                    {
                        if (BattleManagerInstance != null)
                        {
                            return Rarity.VeryCommon;
                        }
                    }
                    break;
                }
                case true when p < veryRare * 100f * 5f:
                {
                    if (BattleManagerInstance != null)
                    {
                        return Rarity.VeryRare;
                    }
                    break;
                }
                case true when p < rare * 100f * 5f:
                {
                    if (BattleManagerInstance != null)
                    {
                        return Rarity.Rare;
                    }
                    break;
                }
            }
            return Rarity.Npc;
        }
    }
}
