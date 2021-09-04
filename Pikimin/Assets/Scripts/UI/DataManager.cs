using System;
using UnityEngine;
using TMPro;
using static Players.Player;
using static UI.TrainerPass;
using static Camera.Timer;

namespace UI 
{
    public class DataManager : MonoBehaviour
    {
    public static DataManager DataManagerInstance;

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private int hoursPlayed;
    [SerializeField] private int minutesPlayed;
    [SerializeField] private float tempFloat;
    public bool infoWasUpdated;
    public float time;

    private void Awake()
    {
        DataManagerInstance = this;
    }

        private void Update()
        {
            if (infoWasUpdated)
            {
                CalculateTime();
                TrainerPassInstance.UpdateInfo();
                text.text = "Pikimin seen: " + TrainerPassInstance.SeenDifferentPikiminAmount;
                text.text += "\n\nPikimin caught: " + TrainerPassInstance.CaughtDifferentPikiminAmount;
                text.text += "\n\nDojo Chefs beaten: " + CurrentPlayer.BeatenDojoChefs;
                if(minutesPlayed > 9)
                {
                    text.text += "\n\nTime played: " + hoursPlayed + ":" + minutesPlayed + " hours";
                }
                else
                {
                    text.text += "\n\nTime played: " + hoursPlayed + ":0" + minutesPlayed + " hours";
                }

                infoWasUpdated = false;
            }
        }

        /// <summary>
        /// Calculates the amount of hours played.
        /// </summary>
        private void CalculateTime()
        {
            time = TimerInstance.Playtime;

            hoursPlayed = (int) (time / 3600);
            float temp = time % 3600;
            minutesPlayed = (int) temp / 60;
        }
    }
}
