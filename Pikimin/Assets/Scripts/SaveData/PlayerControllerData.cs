using System;
using Players;

namespace SaveData
{
    [Serializable]
    public class PlayerControllerData
    {
        public float[] direction;

        public PlayerControllerData(PlayerController playerController)
        {
            direction = new float[2];
            direction[0] = playerController.Direction.x;
            direction[1] = playerController.Direction.y;
        }
    }
}
