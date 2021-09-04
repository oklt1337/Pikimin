using System;
using Audio;
using Battle;
using Map;
using Pikimins;
using Players;
using UI;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class TrainerBehaviour : NpcBehaviour
{
    [Header("Trainer Pikimin Values")]
    [SerializeField] private Pikimin[] ownedPikiminPrefabs;
    [SerializeField] private Pikimin[] ownedPikimin;
    [SerializeField] private int maxLevel;
    [SerializeField] private int minLevel;

    [Header("Trainer Info")] 
    [SerializeField] private byte indicator;
    [SerializeField] private bool hasBeenBeaten;
    [SerializeField] private bool isBeingChallenged;
    [SerializeField] public bool isWalking;
    [SerializeField] private string[] afterLoseDialogue;
    [SerializeField] private int money;

    [Header("Movement Stuff")]
    [SerializeField] private Tilemap colliderTilemap;
    [SerializeField] private Tile colliderTile;
    [SerializeField] private GameObject[] visionTiles;
    public bool looksRight;
    public bool looksLeft;
    public bool looksUp;
    public bool looksDown;
    [SerializeField] private Vector3Int startPosition;

    private Vector2 targetPos;
    private Vector2 oldPos;
    private Vector2 direction;
    [SerializeField] private float speed;
    [SerializeField] private float lerpTime;

    [SerializeField] private bool isDojoChef;

    [Header("Animation")] 
    [SerializeField] private Animator animator;
        
    [Header("Audio")]
    [SerializeField] private bool girlChapterSound;
    [SerializeField] private bool boyChapterSound;
    [SerializeField] private bool badGuyChapterSound;
    [SerializeField] private bool dojoChefChapterSound;
    [SerializeField] public bool rivalChapterSound;
    private static readonly int MoveX = Animator.StringToHash("moveX");
    private static readonly int MoveY = Animator.StringToHash("moveY");
    private static readonly int Walk = Animator.StringToHash("walk");

    public Pikimin[] OwnedPikimin => ownedPikimin;
        
    public int Money => money;

    public bool IsBeingChallenged
    {
        get => isBeingChallenged;
        set => isBeingChallenged = value;
    }

    public bool HasBeenBeaten => hasBeenBeaten;

    private void Start()
    {
        NpcManager.NpcManagerInstance.OnLoadNpcManager += UpdateBeatenStatus;
        Array.Resize(ref ownedPikimin, ownedPikiminPrefabs.Length);

        startPosition = colliderTilemap.WorldToCell(transform.position);

        if (ownedPikiminPrefabs == null) return;
            
        for (var i = 0; i < ownedPikiminPrefabs.Length; i++)
        {
            ownedPikimin[i] = BattleManager.BattleManagerInstance.InstantiatePikimin(ownedPikiminPrefabs[i], transform);
            ownedPikimin[i].GetComponent<SpriteRenderer>().enabled = false;
        }

        foreach (var t in ownedPikimin)
        {
            if (t != null)
            {
                var tempLevel = Random.Range(minLevel, maxLevel);
                t.ChangeLevel(tempLevel);
            }
        }
            
        if (looksRight)
        {
            direction = Vector2.right;
        }
        else if (looksLeft)
        {
            direction = Vector2.left;
        }
        else if (looksUp)
        {
            direction = Vector2.up;
        }
        else if (looksDown)
        {
            direction = Vector2.down;
        }
        UpdateAnimation();

        hasBeenBeaten = NpcManager.NpcManagerInstance.hasBeenBeaten[indicator];
        if (hasBeenBeaten)
        {
            for (int i = 0; i < visionTiles.Length; i++)
            {
                visionTiles[i].SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (isWalking && !isDojoChef)
        {
            MoveCharacter();
            Player.CurrentPlayer.ChangeState(PlayerState.Interact);
        }
        else
        {
            if (DialogueManager.Dialogue.chattingHasEnded && isBeingChallenged && !hasBeenBeaten && Player.CurrentPlayer.CurrentState != PlayerState.Fight)
            {
                DialogueManager.Dialogue.chattingHasEnded = false;
                UpdateOwnTile();
                if (boyChapterSound)
                {
                    BattleManager.BattleManagerInstance.OnBattleStart?.Invoke(null, this, Pikimin.Rarity.Npc, Regions.BiomeList.Npc, 0, 0, AudioManager.AudioManagerInstance.TrainerSounds[3]);
                }
                else if (girlChapterSound)
                {
                    BattleManager.BattleManagerInstance.OnBattleStart?.Invoke(null, this, Pikimin.Rarity.Npc, Regions.BiomeList.Npc, 0, 0, AudioManager.AudioManagerInstance.TrainerSounds[0]);
                }
                else if (badGuyChapterSound)
                {
                    BattleManager.BattleManagerInstance.OnBattleStart?.Invoke(null, this, Pikimin.Rarity.Npc, Regions.BiomeList.Npc, 0, 0, AudioManager.AudioManagerInstance.TrainerSounds[4]);
                }
                else if (dojoChefChapterSound)
                {
                    BattleManager.BattleManagerInstance.OnBattleStart?.Invoke(null, this, Pikimin.Rarity.Npc, Regions.BiomeList.Npc, 0, 0, AudioManager.AudioManagerInstance.DojoChefSounds[1]);
                }
                else
                {
                    BattleManager.BattleManagerInstance.OnBattleStart?.Invoke(null, this, Pikimin.Rarity.Npc, Regions.BiomeList.Npc, 0, 0, AudioManager.AudioManagerInstance.TrainerSounds[3]);
                }
            }
        }

        if (!isWalking)
        {
            var position = transform.position;
            oldPos = position;
        }

        if (hasBeenBeaten)
        {
            SaidDialogue = afterLoseDialogue;
        }
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            base.OnTriggerEnter2D(collision);
            if (!hasBeenBeaten)
            {
                isBeingChallenged = true;
            }
        }
    }

    /// <summary>
    /// Sets bool hasBeenBeaten true.
    /// </summary>
    public virtual void WasBeenBeaten()
    {
        if (!isBeingChallenged) return;
        isBeingChallenged = false;
        hasBeenBeaten = true;
        NpcManager.NpcManagerInstance.hasBeenBeaten[indicator] = hasBeenBeaten;
        PlayerController.CurrentPlayerController.Look();
        for (int i = 0; i < visionTiles.Length; i++)
        {
            visionTiles[i].SetActive(false);
        }
    }

    /// <summary>
    /// Reset if Player lost to them.
    /// </summary>
    public void Reset()
    {
        if (!isBeingChallenged) return;
            
        foreach (var pikimin in ownedPikimin)
        {
            if (pikimin != null)
            {
                pikimin.Heal(9999, true , true);
            }
        }
        isBeingChallenged = false;
        
        colliderTilemap.SetTile(colliderTilemap.WorldToCell(transform.position), null);

        transform.position = new Vector3(startPosition.x + .5f, startPosition.y + .5f);

        colliderTilemap.SetTile(startPosition, colliderTile);
    }

    /// <summary>
    /// Tells the trainer that he is beeing Challanged.
    /// </summary>
    public void IsBeeingChallenged()
    {
        isBeingChallenged = true;
        isWalking = true;

        DialogueManager.Dialogue.ToggleOnOff();
        DialogueManager.Dialogue.talkingCharacter = CharacterName;
        DialogueManager.Dialogue.currentDialogue = SaidDialogue;
            
    }
        
    private void MoveCharacter()
    {
        if(isWalking)
        {
            lerpTime += Time.deltaTime * speed;
            SetTargetPos();
            transform.position = Vector2.Lerp(oldPos, targetPos, lerpTime);
            if ((Vector2)transform.position == targetPos)
            {
                lerpTime = 0;
                isWalking = false;
                UpdateAnimation();
            }
            UpdateAnimation();
        }
    }

    private void SetTargetPos()
    {
        if (looksRight)
        {
            targetPos = Player.CurrentPlayer.transform.position - Vector3.right;
        }
        else if (looksLeft)
        {
            targetPos = Player.CurrentPlayer.transform.position - Vector3.left;
        }
        else if (looksUp)
        {
            targetPos = Player.CurrentPlayer.transform.position - Vector3.up;
        }
        else if (looksDown)
        {
            targetPos = Player.CurrentPlayer.transform.position - Vector3.down;
        }
    }

    private void UpdateAnimation()
    {
        animator.SetFloat(MoveX, direction.x);
        animator.SetFloat(MoveY, direction.y);

        if (!isWalking)
        {
            animator.SetBool(Walk, false);
        }
        else
        {
            animator.SetBool(Walk, true);
        }
            
    }
    
    /// <summary>
    /// Updates the Tilemap after walking.
    /// </summary>
    private void UpdateOwnTile()
    {
        colliderTilemap.SetTile(startPosition, null);
        colliderTilemap.SetTile(colliderTilemap.WorldToCell(transform.position), colliderTile);
    }

    /// <summary>
    /// Setting the prefabs of the trainer manually. 
    /// </summary>
    /// <param name="prefab"> The Pikimin he should have. </param>
    /// <param name="position"> The positoin of the Pikimin he should have. </param>
    public void SetPrefabs(Pikimin prefab, byte position)
    {
        ownedPikiminPrefabs[position] = prefab;
    }

    /// <summary>
    /// Updates if the trainer has been beaten.
    /// </summary>
    public void UpdateBeatenStatus() 
    {
        hasBeenBeaten = NpcManager.NpcManagerInstance.hasBeenBeaten[indicator];
        if (hasBeenBeaten)
        {
            for (int i = 0; i < visionTiles.Length; i++)
            {
                visionTiles[i].SetActive(false);
            }
        }
    }
}