using System;
using System.Collections.Generic;
using System.Linq;
using Battle;
using Map;
using static Map.Regions;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using static UI.PikidexBehaviour;

namespace Pikimins
{
    public sealed class Pikimin : MonoBehaviour
    {
        public enum PikiminState
        {
            Dead,
            Alive
        }

        public enum PikiminType
        {
            Normal,
            Fire,
            Water,
            Electric,
            Grass,
            Ice,
            Fighting,
            Poison,
            Ground,
            Flying,
            Psychic,
            Bug,
            Rock,
            Ghost,
            Dragon,
        }

        public enum MaxExp
        {
            VeryFast = 600000,
            Fast = 800000,
            Medium = 1000000,
            Slow = 1059860,
            VerySlow = 1250000,
            ExtremelySlow = 1640000
        }

        public enum Pikidex
        {
            Bulbasaur   = 001,
            Ivysaur     = 002,
            Venusaur    = 003,
            Charmander  = 004,
            Charmeleon  = 005,
            Charizard   = 006,
            Squirtle    = 007,
            Wartortle   = 008,
            Blastoise   = 009,
            Caterpie    = 010,
            Metapod     = 011,
            Butterfree  = 012,
            Weedle      = 013,
            Kakuna      = 014,
            Beedrill    = 015,
            Pidgey      = 016,
            Pidgeotto   = 017,
            Pidgeot     = 018,
            Rattata     = 019,
            Raticate    = 020,
            Spearow     = 021,
            Fearow      = 022,
            Ekans       = 023,
            Arbok       = 024,
            Pikachu     = 025,
            Raichu      = 026,
            Sandshrew   = 027,
            Sandslash   = 028,
            NidoranW    = 029,
            Nidorana    = 030,
            Nidoqueen   = 031,
            NidoranM    = 032,
            Nidorino    = 033,
            Nidoking    = 034,
            Clefairy    = 035,
            Clefable    = 036,
            Vulpix      = 037,
            Ninetales   = 038,
            Jigglypuff  = 039,
            Wigglytuff  = 040,
            Zubat       = 041,
            Globat      = 042,
            Oddish      = 043,
            Gloom       = 044,
            Vileplume   = 045,
            Paras       = 046,
            Parasect    = 047,
            Venonat     = 048,
            Venomoth    = 049,
            Diglett     = 050,
            Dugtrio     = 051,
            Meowth      = 052,
            Persian     = 053,
            Psyduck     = 054,
            Golduck     = 055,
            Geodude     = 056,
            Graveler    = 057,
            Golem       = 058,
            Onix        = 059,
            Rhyhorn     = 060,
            Rhydon      = 061,
            Magikarp    = 062,
            Gyarados    = 063,
            Lapras      = 064,
            Dratini     = 065,
            Dragonair   = 066,
            Dragonite   = 067,
            Kyogre      = 068,
            Groudon     = 069,
            Rayquaza    = 070
        }

        public enum Rarity
        {
            Npc,
            VeryCommon,
            Common,
            Uncommon,
            Rare,
            VeryRare
        }

        [Header("General")]
        [SerializeField] private string pikiminName;
        [SerializeField] private string description;
        [SerializeField] private byte level = 1;
        [SerializeField] private int exp;
        [SerializeField] private int expForNextLevel;
        [SerializeField] private MaxExp lvl100Exp;
        [SerializeField] private PikiminType pikiType;
        [SerializeField] private PikiminState currentState;
        [SerializeField] private Pikidex pikidexID;
        [SerializeField] private Moves[] currentMoves = new Moves[4];
        [SerializeField] private MoveOnLevelPrefabs moveOnLevelPrefabs;
        [SerializeField] private bool isPlayerPikimin;

        [Header("Stats")]
        [SerializeField] private PikiminBaseStats baseStats;
        [SerializeField] private PikiminIVs pikiminIVs;
        [SerializeField] private PikiminStats pikiminStats;

        [Header("Wild")]
        [SerializeField] private int baseExp;
        [SerializeField] private Rarity rarity;
        [SerializeField] private bool isWild;

        [Header("Evolving")]
        [SerializeField] private bool canEvolve;
        [SerializeField] private PikiminEvolutions evolveTo;

        [Header("Pikidex Stats")]
        [SerializeField] private float height;
        [SerializeField] private float weight;

        [Header("Region")]
        [SerializeField] private BiomeList biomeFound;
        [SerializeField] private PikiminRegions[] regions;

        [Header("Components")]
        [SerializeField] private Sprite frontSprite;
        [SerializeField] private Sprite backSprite;
        [SerializeField] private Sprite footprint;

        public string PikiminName => pikiminName;

        public string Description => description;

        public MaxExp Lvl100Exp => lvl100Exp;

        public PikiminType PikiType => pikiType;

        public PikiminState CurrentState => currentState;

        public Pikidex PikidexID => pikidexID;

        public Rarity MyRarity => rarity;

        public byte Level => level;

        public int Exp => exp;

        public int ExpForNextLevel => expForNextLevel;

        public Moves[] CurrentMoves
        {
            get => currentMoves;
            set => currentMoves = value;
        }

        public bool IsPlayerPikimin
        {
            get => isPlayerPikimin;
            set => isPlayerPikimin = value;
        }

        public bool IsWild
        {
            get => isWild;
            set => isWild = value;
        }

        private bool IsFighting { get; set; }

        public int BaseExp => baseExp;

        private PikiminEvolutions EvolveTo => evolveTo;

        public float Height => height;

        public float Weight => weight;

        public PikiminIVs PikiminIVs => pikiminIVs;

        public PikiminStats PikiminStats => pikiminStats;

        public BiomeList BiomeFound => biomeFound;

        public PikiminRegions[] Regions => regions;

        public Sprite FrontSprite => frontSprite;
        
        public Sprite BackSprite => backSprite;

        public Sprite Footprint => footprint;

        /// <summary>
        /// adds stats from member to this.
        /// </summary>
        /// <param name="pikimin"></param>
        public void AddMember(Pikimin pikimin)
        {
            InstantiateMoves();
            SetStats(pikimin);
            
            pikiminName = pikimin.pikiminName;
            description = pikimin.description;
            level = pikimin.level;
            lvl100Exp = pikimin.lvl100Exp;
            pikiType = pikimin.pikiType;
            currentState = pikimin.currentState;
            moveOnLevelPrefabs = pikimin.moveOnLevelPrefabs;
            isWild = pikimin.isWild;
            IsFighting = pikimin.IsFighting;
            baseExp = pikimin.baseExp;
            rarity = pikimin.rarity;
            canEvolve = pikimin.canEvolve;
            evolveTo = pikimin.evolveTo;
            height = pikimin.height;
            weight = pikimin.weight;
            biomeFound = pikimin.biomeFound;
            pikiminStats = pikimin.pikiminStats;
            frontSprite = pikimin.frontSprite;
            backSprite = pikimin.backSprite;
            pikidexID = pikimin.pikidexID;
        }

        /// <summary>
        /// Copy Stats.
        /// </summary>
        /// <param name="pikimin">Pikimin</param>
        private void SetStats(Pikimin pikimin)
        {
            pikimin.currentState = PikiminState.Alive;
            
            baseStats = pikimin.baseStats;
            pikimin.pikiminIVs.SetIVs();
            pikimin.CalculateStats();
            pikimin.PikiminStats.CurrentHp = pikimin.PikiminStats.FullHp;
            pikiminIVs = pikimin.pikiminIVs;
            pikiminStats = pikimin.PikiminStats;
        }

        private void InstantiateMoves()
        {
            for (var i = 0; i < moveOnLevelPrefabs.MovesPrefabsList.Count; i++)
            {
                if (moveOnLevelPrefabs.MovesPrefabsList[i] == null) continue;
                if (moveOnLevelPrefabs.MovesPrefabsLevel[i] > level) continue;
                if (CurrentMoves[i] != null) continue;
                var tempMove = Instantiate(moveOnLevelPrefabs.MovesPrefabsList[i], transform, true);
                CurrentMoves[i] = tempMove;
                CurrentMoves[i].CurrentPp = currentMoves[i].MAXPp;
            }
        }

        /// <summary>
        /// Calculates all stats.
        /// </summary>
        public void CalculateStats()
        {
            pikiminStats.FullHp =
                Convert.ToInt32(((baseStats.BaseHp + pikiminIVs.HpIV) * 2 + pikiminIVs.HpIV / 4) * level / 100) +
                level + 10;
            pikiminStats.AttackStat =
                Convert.ToInt32(((baseStats.BaseAtk + pikiminIVs.AtkIV) * 2 + pikiminIVs.AtkIV / 4) * level / 100) + 5;
            pikiminStats.DefenseStat =
                Convert.ToInt32(((baseStats.BaseDef + pikiminIVs.DefIV) * 2 + pikiminIVs.DefIV / 4) * level / 100) + 5;
            pikiminStats.SpeedStat =
                Convert.ToInt32(((baseStats.BaseSpeed + pikiminIVs.SpeedIV) * 2 + pikiminIVs.SpeedIV / 4) * level / 100) + 5;
        }

        /// <summary>
        /// Adds Exp to Pikimin.
        /// </summary>
        /// <param name="gainedExp">int gainedExp</param>
        public void AddExp(int gainedExp)
        {
            exp += gainedExp;
            
            if (exp >= expForNextLevel && level < 100)
            {
                LevelUp();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public int CalculateExpForLevelUp(int n)
        {
            var lvlExp = 0;
            switch (lvl100Exp)
            {
                case MaxExp.VeryFast:
                    if (level <= 50)
                    {
                       lvlExp = Convert.ToInt32(Math.Pow(n, 3) * ((100f - n) / 50f));
                    }
                    else if (level <= 68)
                    {
                        lvlExp = Convert.ToInt32(Math.Pow(n, 3) * ((150f - n) / 100f));
                    }
                    else if (level <= 98)
                    {
                        lvlExp = Convert.ToInt32(Math.Pow(n, 3) * ((1911f - 10f) * n / 3) / 500);
                    }
                    else if (level <= 100)
                    {
                        lvlExp = Convert.ToInt32(Math.Pow(n, 3) * ((160f - n) / 100f));
                    }
                    break;
                case MaxExp.Fast:
                    lvlExp = Convert.ToInt32(4 * Math.Pow(n, 3) / 5);
                    break;
                case MaxExp.Medium:
                    lvlExp = Convert.ToInt32(Math.Pow(n, 3));
                    break;
                case MaxExp.Slow:
                    lvlExp = Convert.ToInt32(6f / 5f * Math.Pow(n, 3) - (15 * Math.Pow(n, 2)) + 100 * n - 140);
                    break;
                case MaxExp.VerySlow:
                    lvlExp = Convert.ToInt32(5f / 4f * Math.Pow(n, 3));
                    break;
                case MaxExp.ExtremelySlow:
                    if (level <= 15)
                    {
                        lvlExp = Convert.ToInt32(Math.Pow(n, 3) * (((n + 1) / 3f + 24) / 50f));
                    }
                    else if (level <= 36)
                    {
                        lvlExp = Convert.ToInt32(Math.Pow(n, 3) * (n + 14) / 50);
                    }
                    else if (level <= 100)
                    {
                        lvlExp = Convert.ToInt32(Math.Pow(n, 3) * (n / 2f + 32) / 50);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return lvlExp;
        }
        
        /// <summary>
        /// Calculates how much exp a Pikimin should give.
        /// </summary>
        /// <param name="amountUsedPikimin">int</param>
        /// <returns>int exp to gain</returns>
        public int GainedExpCalculation(int amountUsedPikimin)
        {
            var a = 1f;
            var b = baseExp;
            var l = level;
            var s = amountUsedPikimin;

            a = isWild ? 1f : 1.5f;

            var result = Convert.ToInt32(a * b * l * s / (7 * s));

            return result;
        }
        
        /// <summary>
        /// Level up Pikimin
        /// </summary>
        private void LevelUp()
        {
            while (exp > expForNextLevel)
            {
                level++;
                expForNextLevel = CalculateExpForLevelUp(level);
                IncreaseStats();
                
                if (moveOnLevelPrefabs != null)
                {
                    for (var i = 0; i < moveOnLevelPrefabs.MovesPrefabsLevel.Count; i++)
                    {
                        if (moveOnLevelPrefabs.MovesPrefabsLevel[i] == level)
                        {
                            LearnMove(moveOnLevelPrefabs.MovesPrefabsList[i]);
                        } 
                    }
                }

                if (canEvolve && Level >= EvolveTo.evolvingLevel && IsPlayerPikimin)
                {
                    Evolve();
                }
            }
        }
        
        /// <summary>
        /// Pikimin Evolves.
        /// </summary>
        private void Evolve()
        {
            if (evolveTo == null) return;
            
            // Adding the Evolved Pikimin to the Pikidex.
            PikidexInstance.CaughtPikiminUpdater(evolveTo.nextEvolution);
                
            pikiminName = evolveTo.nextEvolution.PikiminName;
            description = evolveTo.nextEvolution.Description;
            weight = evolveTo.nextEvolution.Weight;
            height = evolveTo.nextEvolution.Height;
            frontSprite = evolveTo.nextEvolution.frontSprite;
            backSprite = evolveTo.nextEvolution.backSprite;
            baseStats = evolveTo.nextEvolution.baseStats;
            pikidexID = evolveTo.nextEvolution.PikidexID;
            moveOnLevelPrefabs = evolveTo.nextEvolution.moveOnLevelPrefabs;
            canEvolve = evolveTo.nextEvolution.canEvolve;
            evolveTo.evolvingLevel = evolveTo.nextEvolution.evolveTo.evolvingLevel;
            evolveTo.nextEvolution = evolveTo.nextEvolution.evolveTo.nextEvolution;

            IncreaseStats();
        }

        /// <summary>
        /// Increase Stats.
        /// </summary>
        private void IncreaseStats()
        {
            var oldMaxHp = PikiminStats.FullHp;

            CalculateStats();

            var newMaxHp = PikiminStats.FullHp;

            PikiminStats.CurrentHp += newMaxHp - oldMaxHp;
        }

        /// <summary>
        /// Takes Damage.
        /// </summary>
        /// <param name="damage">int damage</param>
        public void TakeDamage(int damage)
        {
            PikiminStats.CurrentHp -= damage;
            
            if (PikiminStats.CurrentHp <= 0)
            {
                Dead();
            }
        }

        /// <summary>
        /// Heals Pikimin.
        /// </summary>
        /// <param name="healedAmount"> The amount a Pot/Attack health. </param>
        /// <param name="revives">bool</param>
        /// <param name="restoresPp">bool</param>
        public void Heal(int healedAmount, bool revives, bool restoresPp)
        {
            if (revives)
            {
                ChangeState(PikiminState.Alive);
            }

            if (restoresPp)
            {
                foreach (var t in CurrentMoves)
                {
                    if (t != null)
                    {
                        t.CurrentPp = t.MAXPp;
                    }
                }
            }

            pikiminStats.CurrentHp += healedAmount;
            CheckForOverHeal();
        }

        /// <summary>
        /// Sets currentHp to FullHp if current > full.
        /// </summary>
        private void CheckForOverHeal()
        {
            if (pikiminStats.CurrentHp > pikiminStats.FullHp)
            {
                pikiminStats.CurrentHp = pikiminStats.FullHp;
            }
        }

        /// <summary>
        /// Pikimin is Dead;
        /// </summary>
        public void Dead()
        {
            if(pikiminStats.CurrentHp <= 0)
            {
                ChangeState(PikiminState.Dead);
            }
        }

        /// <summary>
        /// Use a Move.
        /// </summary>
        /// <returns>int power, bool didHit</returns>
        public (int,bool) UseMove(Moves move)
        {
            if (CurrentMoves.All(tMove => tMove != move)) return (0, false);
            move.CurrentPp--;

            if (!(Random.Range(0f, 100f) < move.Accuracy)) return (0, false);

            return (move.Power, true);

        }
        
        public void LearnMove(Moves newMove)
        {
            for (var i = 0; i < currentMoves.Length; i++)
            {
                if (newMove == null) return;

                if (currentMoves[i] == newMove) return;
                
                if (currentMoves[i] == null)
                {
                    InstantiateMoves();
                    return;
                }
            }
        }

        public void ChangeLevel(int lvl)
        {
            AddExp(CalculateExpForLevelUp(lvl));
        }

        public void SetIvs(int hp, int atk, int def, int speed)
        {
            pikiminIVs.SetIVsManual(hp,atk,def,speed);
        }

        /// <summary>
        /// Use Random move.
        /// </summary>
        /// <returns>int power,bool didMiss</returns>
        public (int, bool, Moves move) UseRandomMove()
        {
            var amount = CurrentMoves.Count(t => t != null);
            var rand = Random.Range(0, amount);
            
            var (power, didHit) = Random.Range(0f, 100f) < CurrentMoves[rand].Accuracy ? (CurrentMoves[rand].Power, true) : (CurrentMoves[rand].Power, false);
            return (power, didHit, CurrentMoves[rand]);
        }

        private void ChangeState(PikiminState newState)
        {
            currentState = newState;
        }

        public void OnDestroy()
        {
            for (int i = 0; i < CurrentMoves.Length; i++)
            {
                CurrentMoves[i] = null;
            }
        }
    }

    [Serializable]
    public sealed class PikiminEvolutions
    {
        public Pikimin nextEvolution;
        public int evolvingLevel;
    }

    [Serializable]
    public class PikiminIVs
    {
        // ReSharper disable once InconsistentNaming
        [SerializeField]private int hpIV;
        // ReSharper disable once InconsistentNaming
        [SerializeField]private int atkIV;
        // ReSharper disable once InconsistentNaming
        [SerializeField]private int defIV;
        // ReSharper disable once InconsistentNaming
        [SerializeField]private int speedIV;
        
        // ReSharper disable once InconsistentNaming
        public int AtkIV => atkIV;

        // ReSharper disable once InconsistentNaming
        public int DefIV => defIV;

        // ReSharper disable once InconsistentNaming
        public int SpeedIV => speedIV;

        // ReSharper disable once InconsistentNaming
        public int HpIV => hpIV;

        /// <summary>
        /// Setting IVs
        /// </summary>
        public void SetIVs()
        {
            atkIV = Random.Range(0, 15);
            defIV = Random.Range(0, 15);
            speedIV = Random.Range(0, 15);
            hpIV = Random.Range(0 ,15);
        }

        public void SetIVsManual(int hp, int atk, int def, int speed)
        {
            atkIV = atk;
            defIV = def;
            speedIV = speed;
            hpIV = hp;
        }
    }
    
    [Serializable]
    public class PikiminBaseStats
    {
        [SerializeField]private int baseHp;
        [SerializeField]private int baseAtk;
        [SerializeField]private int baseDef;
        [SerializeField]private int baseSpeed;

        public int BaseHp => baseHp;
        public int BaseAtk => baseAtk;
        public int BaseDef => baseDef;
        public int BaseSpeed => baseSpeed;
    }

    [Serializable]
    public class PikiminStats
    {
        [SerializeField] private int attackStat;
        [SerializeField] private int defenseStat;
        [SerializeField] private int speedStat;
        [SerializeField] private int currentHp;
        [SerializeField] private int fullHp;

        public int AttackStat
        {
            get => attackStat;
            set => attackStat = value;
        }

        public int DefenseStat
        {
            get => defenseStat;
            set => defenseStat = value;
        }

        public int SpeedStat
        {
            get => speedStat;
            set => speedStat = value;
        }

        public int CurrentHp
        {
            get => currentHp;
            set
            {
                currentHp = value;
                if (currentHp > fullHp)
                {
                    currentHp = fullHp;
                }

                if (currentHp < 0)
                {
                    currentHp = 0;
                }
            }
        }

        public int FullHp
        {
            get => fullHp;
            set => fullHp = value;
        }
    }

    [Serializable]
    public class MoveOnLevelPrefabs
    {
        [SerializeField]private List<Moves> movesPrefabsList;
        [SerializeField]private List<int> movesPrefabsLevel;

        public List<Moves> MovesPrefabsList => movesPrefabsList;
        public List<int> MovesPrefabsLevel => movesPrefabsLevel;
    }

    public class LoadedPikimin : MonoBehaviour
    {
        public static Pikimin InstantiateLoadedPikimin(Pikimin prefab, Transform myParent)
        {
            var tempPikimin = Instantiate(prefab, myParent, true);
            tempPikimin.AddMember(tempPikimin);

            tempPikimin.transform.position = BattleManager.BattleManagerInstance.DumbPikimin.position;

            return tempPikimin;
        }
    }
}
