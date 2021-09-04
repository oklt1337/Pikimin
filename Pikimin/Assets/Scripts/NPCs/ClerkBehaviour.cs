using UI;
using UnityEngine;

public class ClerkBehaviour : NpcBehaviour
{
    [SerializeField] private GameObject shop;
    [SerializeField] private GameObject authorizeMenu;
    [SerializeField] private AuthorizeMenu authorize;

    private void Update()
    {
        // Ends the Dialogue and opens the Shop if choosen so.
        if (authorize.playerHasAuthorized)
        {
            if (authorize.WantsHisChoice)
            {
                shop.gameObject.SetActive(true);
            }
        }
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        authorize.playerHasAuthorized = false;
        authorizeMenu.SetActive(true);
    }
}