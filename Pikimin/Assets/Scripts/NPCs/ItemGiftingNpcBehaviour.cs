using UnityEngine;
using Items;
using static GiftPikiminAndItemManager;
using static Players.ItemInventory;

public class ItemGiftingNpcBehaviour : NpcBehaviour
{
    [SerializeField] private Item containedItem;
    [SerializeField] private byte indicator;

    private Item ContainedItem
    {
        get => containedItem;
        set => containedItem = value;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (GiftPikiminAndItemManagerInstance.wasGiven[indicator] && containedItem != null)
        {
            containedItem = null;
        }
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Player"))
        {
            if (ContainedItem != null)
            {
                GiftPikiminAndItemManagerInstance.wasGiven[indicator] = true;
                playerItems.OnNewItem(ContainedItem);
                ContainedItem = null;
            }
        }
    }
}
