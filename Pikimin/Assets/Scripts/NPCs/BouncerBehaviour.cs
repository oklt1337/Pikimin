using Players;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BouncerBehaviour : NpcBehaviour
{
    [Header("Bouncer Stuff")]
    [SerializeField] private bool mayEnter;
    [SerializeField] private string[] mayEnterDialogue;
    [SerializeField] private string[] afterEnterDialogue;
    [SerializeField] private GameObject championshipManager;

    [Header("Walk Stuff")]
    [SerializeField] private Animator anim;
    [SerializeField] private Tilemap colliderTilemap;
    [SerializeField] private Tile colliderTile;
    [SerializeField] private Vector3Int startPosition;
    [SerializeField] private Vector3 startPositionVector;
    [SerializeField] private Vector3 targetPositionVector;
    [SerializeField] private float lerpTime;
    [SerializeField] private bool isWalking;
    private static readonly int Walk = Animator.StringToHash("walk");
    private static readonly int MoveX = Animator.StringToHash("moveX");
    private static readonly int MoveY = Animator.StringToHash("moveY");

    private void Start()
    {
        var position = transform.position;
        startPosition = colliderTilemap.WorldToCell(position);
        startPositionVector = position;
        targetPositionVector = position - Vector3.right;
        anim.SetBool(Walk, false);
        anim.SetFloat(MoveX, 0);
        anim.SetFloat(MoveY, -1);
    }

    private void Update()
    {
        if (isWalking)
        {
            ChangePosition();
        }
    }

    /// <summary>
    /// Checks if the Player is allowed to enter the Championship.
    /// </summary>
    private void CheckBeatenDojos()
    {
        if(Player.CurrentPlayer.BeatenDojoChefs > 2 && SaidDialogue != afterEnterDialogue)
        {
            mayEnter = true;
            SaidDialogue = mayEnterDialogue;
        }
    }

    /// <summary>
    /// Makes the NPC go one step to the left.
    /// </summary>
    private void ChangePosition()
    {
        anim.SetBool(Walk, true);
        anim.SetFloat(MoveX, -1);
        anim.SetFloat(MoveY, 0);
        lerpTime += Time.deltaTime;
        transform.position = Vector2.Lerp(startPositionVector, targetPositionVector, lerpTime);
        if(transform.position == targetPositionVector)
        {
            anim.SetBool(Walk, false); 
            anim.SetFloat(MoveX, 0);
            anim.SetFloat(MoveY, -1);
            lerpTime = 0;
            isWalking = false;
            championshipManager.SetActive(true);
        }
        colliderTilemap.SetTile(startPosition, null);
        colliderTilemap.SetTile(colliderTilemap.WorldToCell(transform.position), colliderTile);
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        CheckBeatenDojos();
        base.OnTriggerEnter2D(collision);

        if (mayEnter)
        {
            isWalking = true;
            mayEnter = false;
            SaidDialogue = afterEnterDialogue;
        }
    }
}