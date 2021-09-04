using Items;
using Players;
using UnityEngine;

public class DojoChefBehaviour : TrainerBehaviour
{
    [SerializeField] private Item hm;

    public override void WasBeenBeaten()
    {
        if (!IsBeingChallenged) return;
        Player.CurrentPlayer.AfterBeatingDojoChef();
        ItemInventory.playerItems.OnNewItem(hm);
        base.WasBeenBeaten();
    }
}