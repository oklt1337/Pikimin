using UnityEngine;
using static Players.Player;

namespace Items
{
    public class Protect : Item
    {
        [SerializeField] private float timer = 60f;
        private bool startTimer;

        #region Overrides of Item

        public override void OnUse()
        {
            if (!CurrentPlayer.IsProtected)
            {
                base.OnUse();
                CurrentPlayer.SwapProtected();
                startTimer = true;
            }
            else
            {
                timer += 60f;
                base.OnUse();
            }
        }

        private void Update()
        {
            if (startTimer)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    CurrentPlayer.SwapProtected();
                    timer = 60;
                    startTimer = false;
                }
            }
        }

        #endregion
    }
}
