using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Battle;
using Map;
using Pikimins;
using Players;
using SaveData;
using TMPro;
using UI;
using UnityEngine;

public class PikiminBox : MonoBehaviour
{
    public static PikiminBox PikiBox;

    [Header("Components")]
    [SerializeField] private Pikimin[] boxPikimin;
    [SerializeField] private GameObject pcBox;
    [SerializeField] private TextMeshProUGUI text;

    [Header("Values")]
    [SerializeField] private byte currentPosition;
    [SerializeField] private byte upperBenchmark;
    [SerializeField] private byte lowerBenchmark;

    [Header("Authorizing")]
    [SerializeField] private GameObject authorizeBox;
    [SerializeField] private TextMeshProUGUI authorizeText;
    [SerializeField] private byte currentAuthorizePosition;

    [Header("Misc")]
    private bool? wantsToTake;
    private byte boxPikiminAmount;

    public byte BoxPikiminAmount => boxPikiminAmount;

    public Pikimin[] BoxPikimin1 => boxPikimin;

    private void Awake()
    {
        PikiBox = this;
    }

    private void Start()
    {
        LoadData.LoadDataInstance.OnLoad += LoadPikiminBoxData; 
    }

    private void Update()
    {
        if (pcBox.activeSelf)
        {
            Inputs();
        }
    }

    /// <summary>
    /// The inputs of the Player.
    /// </summary>
    private void Inputs()
    {
        // UP and DOWN
        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)))
        {
            if (authorizeBox.activeSelf)
            {
                if(boxPikimin[currentPosition] == null && currentAuthorizePosition < 1)
                {
                    currentAuthorizePosition++;
                }
                else if (boxPikimin[currentAuthorizePosition] != null && currentAuthorizePosition < 2)
                {
                    currentAuthorizePosition++;
                }
                    
            }
            else if (!authorizeBox.activeSelf)
            {
                if (wantsToTake == true && boxPikimin[currentPosition] != null)
                {
                    currentPosition++;
                    if (currentPosition > upperBenchmark)
                    {
                        upperBenchmark++;
                        lowerBenchmark++;
                    }
                }
                else if(wantsToTake == false && currentPosition < 4)
                {
                    currentPosition++;
                }
                else if (wantsToTake == null && currentPosition < 1)
                {
                    currentPosition++;
                }
            }
            TextManager();
        }
        else if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            if (authorizeBox.activeSelf)
            {
                if (boxPikimin[currentPosition] == null && currentAuthorizePosition > 0)
                {
                    currentAuthorizePosition--;
                }
                else if (boxPikimin[currentAuthorizePosition] != null && currentAuthorizePosition > 0)
                {
                    currentAuthorizePosition--;
                }

            }
            else if (!authorizeBox.activeSelf && 0 < currentPosition)
            {
                currentPosition--;
                if (currentPosition < lowerBenchmark)
                {
                    upperBenchmark--;
                    lowerBenchmark--;
                }
            }
            TextManager();
        }

        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E))
        {
            if (wantsToTake == null)
            {
                if(currentPosition == 0)
                {
                    wantsToTake = true;
                    TextManager();
                }
                else
                {
                    wantsToTake = false;
                    currentPosition = 0;
                    TextManager();
                }
            }
            else if (!authorizeBox.activeSelf && wantsToTake != null)
            {
                authorizeBox.SetActive(true);
                TextManager();
            } 
            else if (authorizeBox.activeSelf && wantsToTake == false)
            {
                if(currentAuthorizePosition == 0 && PikiminInventory.playerPikiminInventory.pikiminAmount > 1)
                {
                    PikiminInventory.playerPikiminInventory.GivePikiminToBox(currentPosition);
                    boxPikiminAmount++;
                    if(boxPikiminAmount == boxPikimin.Length)
                    {
                        Array.Resize(ref boxPikimin, boxPikiminAmount + 10);
                    }
                    TextManager();
                    authorizeBox.SetActive(false);
                }
                else
                {
                    authorizeBox.SetActive(false);
                    currentAuthorizePosition = 0;
                }
            }
            else if(authorizeBox.activeSelf && wantsToTake == true)
            {
                if(currentAuthorizePosition == 0) 
                {
                    GivePlayerPikimin();
                    authorizeBox.SetActive(false);
                    currentAuthorizePosition = 0;
                }
                else if(currentAuthorizePosition == 1) 
                {
                    boxPikimin[currentPosition] = null;
                    boxPikiminAmount--;
                    authorizeBox.SetActive(false);
                    currentAuthorizePosition = 0;
                    PikiminSorter();
                }
                else if(currentAuthorizePosition == 2)
                {
                    authorizeBox.SetActive(false);
                    currentAuthorizePosition = 0;
                }
            }
        }

        // Go back to Decision of Giving/Taking.
        if(Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Backspace))
        {
            authorizeBox.SetActive(false);
            lowerBenchmark = 0;
            upperBenchmark = 7;
            currentPosition = 0;
            currentAuthorizePosition = 0;
            wantsToTake = null;
            TextManager();
        }

        // Leave the Box.
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P) || wantsToTake == null && (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Backspace)))
        {
            pcBox.SetActive(false);
            authorizeBox.SetActive(false);
            lowerBenchmark = 0;
            upperBenchmark = 7;
            currentPosition = 0;
            currentAuthorizePosition = 0;
            wantsToTake = null;
            TextManager();
            Player.CurrentPlayer.ChangeState(PlayerState.Idle);
        }
    }

    /// <summary>
    /// Manages the text of the box.
    /// </summary>
    private void TextManager()
    {
        if(wantsToTake == true)
        {
            text.text = "";
            for (int i = 0; i < boxPikimin.Length; i++)
            {
                if (lowerBenchmark <= i && upperBenchmark >= i)
                {
                    if (i == currentPosition && boxPikimin[i] != null)
                    {
                        text.text += " > " + boxPikimin[i].PikiminName + " Lv" + boxPikimin[i].Level + "\n\n";
                    }
                    else if (i != currentPosition && boxPikimin[i] != null)
                    {
                        text.text += boxPikimin[i].PikiminName + " Lv" + boxPikimin[i].Level + "\n\n";
                    }
                    else if (i == currentPosition && boxPikimin[i] == null)
                    {
                        text.text += " > -----------" + "\n\n";
                    }
                    else if (boxPikimin[i] == null)
                    {
                        text.text += "-------------" + "\n\n";
                    }
                }
            }
        }
        else if(wantsToTake == false)
        {
            text.text = "";
            for (int i = 0; i < PikiminInventory.playerPikiminInventory.OwnedPikimin.Length; i++)
            {
                if (lowerBenchmark <= i && upperBenchmark >= i)
                {
                    if (i == currentPosition && PikiminInventory.playerPikiminInventory.OwnedPikimin[i] != null)
                    {
                        text.text += " > " + PikiminInventory.playerPikiminInventory.OwnedPikimin[i].PikiminName + " Lv" + PikiminInventory.playerPikiminInventory.OwnedPikimin[i].Level + "\n\n";
                    }
                    else if (i != currentPosition && PikiminInventory.playerPikiminInventory.OwnedPikimin[i] != null)
                    {
                        text.text += PikiminInventory.playerPikiminInventory.OwnedPikimin[i].PikiminName + " Lv" + PikiminInventory.playerPikiminInventory.OwnedPikimin[i].Level + "\n\n";
                    }
                    else if (i == currentPosition && PikiminInventory.playerPikiminInventory.OwnedPikimin[i] == null)
                    {
                        text.text += " > -----------" + "\n\n";
                    }
                    else if (PikiminInventory.playerPikiminInventory.OwnedPikimin[i] == null)
                    {
                        text.text += "-------------" + "\n\n";
                    }
                }
            }
        }
        else if (wantsToTake == null)
        {
            if (currentPosition == 0)
            {
                text.text = " > Take a Pikimin \n\n Give a Pikimin";
            }
            else
            {
                text.text = " Take a Pikimin \n\n > Give a Pikimin";
            }
        }

        if (authorizeBox.activeSelf)
        {
            if (wantsToTake == false)
            {
                if(currentAuthorizePosition == 0)
                {
                    authorizeText.text = " > Give \n Exit";
                }
                else
                {
                    authorizeText.text = " Give \n > Exit";
                }
            }
            else
            {
                if(currentAuthorizePosition == 0)
                {
                    authorizeText.text = " > Take \n Release \n Exit";
                }
                else if(currentAuthorizePosition == 1)
                {
                    authorizeText.text = " Take \n > Release \n Exit";
                }
                else if(currentAuthorizePosition == 2)
                {
                    authorizeText.text = " Take \n Release \n > Exit";
                }
            }
        }
    }

    /// <summary>
    /// Adds an Pikimin to the Box when Player has full Pikimin.
    /// </summary>
    /// <param name="newPikimin"> The added Pikimin. </param>
    public void AddPikimin(Pikimin newPikimin)
    {
        if (PikiminInventory.playerPikiminInventory.pikiminAmount >= 4)
        {
            for (int i = 0; i < boxPikimin.Length; i++)
            {
                if (boxPikimin[i] == null)
                {
                    boxPikimin[i] = BattleManager.BattleManagerInstance.InstantiatePikimin(newPikimin, PikiminInventory.playerPikiminInventory.transform);
                    boxPikimin[i].SetIvs(newPikimin.PikiminIVs.HpIV, newPikimin.PikiminIVs.AtkIV, newPikimin.PikiminIVs.DefIV, newPikimin.PikiminIVs.SpeedIV);
                    boxPikimin[i].CalculateStats();
                    
                    boxPikimin[i].gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    boxPikimin[i].IsPlayerPikimin = true;
                    boxPikimin[i].IsWild = false;
                    boxPikiminAmount++;
                    PikidexBehaviour.PikidexInstance.CaughtPikiminUpdater(boxPikimin[i]);
                    boxPikimin[i].Heal(99999, true, true);

                    if (boxPikiminAmount == boxPikimin.Length)
                    {
                        Array.Resize(ref boxPikimin, boxPikiminAmount + 10);
                    }
                    break;
                }
            }
        }
    }

    public void AddManualPikimin(Pikimin newPikimin)
    {
        if (PikiminInventory.playerPikiminInventory.pikiminAmount >= 2)
        {
            for (int i = 0; i < boxPikimin.Length; i++)
            {
                if (boxPikimin[i] == null)
                {
                    boxPikimin[i] = newPikimin;
                    boxPikimin[i].GetComponent<SpriteRenderer>().enabled = false;
                    boxPikimin[i].Heal(99999, true, true);
                    boxPikiminAmount++;
                    if (boxPikiminAmount == boxPikimin.Length)
                    {
                        Array.Resize(ref boxPikimin, boxPikiminAmount + 10);
                    }
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Gives the Player a Pikimin from the Box.
    /// </summary>
    private void GivePlayerPikimin()
    {
        if(PikiminInventory.playerPikiminInventory.pikiminAmount < 4)
        {
            PikiminInventory.playerPikiminInventory.OnNewPikimin(boxPikimin[currentPosition]);
            boxPikimin[currentPosition] = null;
            boxPikiminAmount--;
            PikiminSorter();
        }
    }

    /// <summary>
    /// Sorts the Position of all Pikimin.
    /// </summary>
    private void PikiminSorter()
    {
        for (int i = 0; i < boxPikimin.Length; i++)
        {
            if(boxPikimin[i] == null && i < boxPikimin.Length - 1)
            {
                boxPikimin[i] = boxPikimin[i + 1];
                boxPikimin[i + 1] = null;
            }
        }

        TextManager();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Lets Players talk to the box.
        if (collision.CompareTag("Player"))
        {
            TextManager();
            pcBox.SetActive(true);
            Player.CurrentPlayer.ChangeState(PlayerState.Interact);
        }
    }

    private void LoadPikiminBoxData()
    {
        //Read SaveData
        PikiminBoxData data = SaveManager.LoadPikiminBox();
        
        if (data.pikiminBoxAmount > 0)
        {
            if (boxPikimin.Length < data.pikimin.Length)
            {
                Array.Resize(ref boxPikimin, data.pikimin.Length);
            }
            
            for (int i = 0; i < data.pikimin.Length; i++)
            {
                if (data.pikimin[i] != 999)
                {
                    boxPikimin[i] = BattleManager.BattleManagerInstance.InstantiatePikimin(BattleManager.BattleManagerInstance.AllPikimin[data.pikimin[i]], PikiminInventory.playerPikiminInventory.transform);
                    boxPikimin[i].SetIvs(data.hp[i], data.atk[i], data.def[i], data.speed[i]);
                    boxPikimin[i].CalculateStats();
                    boxPikimin[i].AddExp(data.exp[i]);

                    boxPikimin[i].gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    boxPikimin[i].IsPlayerPikimin = true;
                }
            }
        }
    }
}