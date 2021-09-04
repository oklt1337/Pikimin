using SaveData;
using UnityEngine;

namespace Camera
{
    public class Timer : MonoBehaviour
    {
        public static Timer TimerInstance;
        [SerializeField] private float playtime;

        public float Playtime => playtime;

        private void Awake()
        {
            if (TimerInstance != null)
            {
                Destroy(this);
            }
            else
            {
                TimerInstance = this;
            }
        }

        private void Start()
        {
            LoadData.LoadDataInstance.OnLoad += LoadTime;
        }

        private void Update()
        {
            playtime += Time.deltaTime;
        }

        private void LoadTime()
        {
            TimerData data = SaveManager.LoadTime();

            playtime = data.time;
        }
    }
}
