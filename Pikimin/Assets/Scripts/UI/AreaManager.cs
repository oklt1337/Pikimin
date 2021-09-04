using TMPro;
using UnityEngine;

namespace UI
{
    public class AreaManager : MonoBehaviour
    {
        public byte currentAreaIndex;
        public string currentArea;
        public float activeDuration = 2;
        public TextMeshProUGUI areaName;
        public GameObject parent;

        void Update()
        {
            ShowArea();
        }

        /// <summary>
        /// Shows the Area for a set amount of time.
        /// </summary>
        private void ShowArea()
        {
            areaName.text = currentArea;

            activeDuration -= Time.deltaTime;

            if (activeDuration < 0)
            {
                activeDuration = 2;
                parent.SetActive(false);
            }
        }
    }
}
