using Battle;
using UnityEngine;
using Pikimins;
using static Players.PikiminInventory;
using static AleiBehaviour;
using static GiftPikiminAndItemManager;

public class PikiminGivingNpc : NpcBehaviour
{
    [Header("Gifted Pikimin")]
    [SerializeField] private Pikimin containedPikiminPrefab;
    [SerializeField] private Pikimin containedPikiminBackupPrefab;
    [SerializeField] private byte indicator;

    private void Update()
    {
        if (GiftPikiminAndItemManagerInstance.wasGiven[indicator] && containedPikiminBackupPrefab != null)
        {
            containedPikiminBackupPrefab = null;
            containedPikiminPrefab = null;
        }
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (containedPikiminPrefab != null)
        {
            GiftPikiminAndItemManagerInstance.wasGiven[indicator] = true;
            if (containedPikiminPrefab.PikiminName != Alei.TakenPikimin)
            {
                var tempPikimin = Instantiate(containedPikiminPrefab, playerPikiminInventory.transform, true);
                tempPikimin.AddMember(tempPikimin);
                tempPikimin.ChangeLevel(5);
                tempPikimin.transform.position = BattleManager.BattleManagerInstance.DumbPikimin.position;
                playerPikiminInventory.OnNewPikimin(tempPikimin);
                containedPikiminPrefab = null;
                containedPikiminBackupPrefab = null;
            }
            else if(containedPikiminPrefab.PikiminName == Alei.TakenPikimin)
            {
                var tempPikimin = Instantiate(containedPikiminBackupPrefab, playerPikiminInventory.transform, true);
                tempPikimin.AddMember(tempPikimin);
                tempPikimin.ChangeLevel(5);
                tempPikimin.transform.position = BattleManager.BattleManagerInstance.DumbPikimin.position;
                playerPikiminInventory.OnNewPikimin(tempPikimin);
                containedPikiminPrefab = null;
                containedPikiminBackupPrefab = null;
            }
        }
    }
}
