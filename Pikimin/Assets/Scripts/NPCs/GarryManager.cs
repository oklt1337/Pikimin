using Battle;
using UnityEngine;

public class GarryManager : MonoBehaviour
{
    [SerializeField] private TrainerBehaviour[] garrys;


    private void Awake()
    {
        if(AleiBehaviour.Alei.TakenPikimin == "Bulbasaur") 
        {
            garrys[0].SetPrefabs(BattleManager.BattleManagerInstance.AllPikimin[3], 0);
            garrys[1].SetPrefabs(BattleManager.BattleManagerInstance.AllPikimin[3], 2);
            garrys[2].SetPrefabs(BattleManager.BattleManagerInstance.AllPikimin[4], 3);
        }
        else if(AleiBehaviour.Alei.TakenPikimin == "Charmander")
        {
            garrys[0].SetPrefabs(BattleManager.BattleManagerInstance.AllPikimin[6], 0);
            garrys[1].SetPrefabs(BattleManager.BattleManagerInstance.AllPikimin[6], 2);
            garrys[2].SetPrefabs(BattleManager.BattleManagerInstance.AllPikimin[7], 3);
        }
        else if(AleiBehaviour.Alei.TakenPikimin == "Squirtle")
        {
            garrys[0].SetPrefabs(BattleManager.BattleManagerInstance.AllPikimin[0], 0);
            garrys[1].SetPrefabs(BattleManager.BattleManagerInstance.AllPikimin[0], 2);
            garrys[2].SetPrefabs(BattleManager.BattleManagerInstance.AllPikimin[1], 3);
        }
    }

    private void Start()
    {
        
        garrys[0].OwnedPikimin[0].SetIvs(1,1,1,1);
    }

    private void Update()
    {
        if (garrys[1].isWalking)
        {
            garrys[0].gameObject.SetActive(false);
        }

        if (garrys[2].isWalking)
        {
            garrys[1].gameObject.SetActive(false);
        }
    }
}