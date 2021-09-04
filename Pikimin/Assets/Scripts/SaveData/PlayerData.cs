using System;
using Players;

namespace SaveData
{
   [Serializable]
   public class PlayerData
   {
      public float[] position;
      public float[] rotation;
      public float[] scale;
   
      public int playerRegion;
      public bool useBike;
      public bool isSurfing;
      public byte beatenDojoChefs;

      public byte lastReha;

      public PlayerData(Player player)
      {
         position = new float[3];
         var transform = player.transform;
         var newPosition = transform.position;
         position[0] = newPosition.x;
         position[1] = newPosition.y;
         position[2] = newPosition.z;
      
         rotation = new float[3];
         var newRotation = transform.rotation;
         rotation[0] = newRotation.x;
         rotation[1] = newRotation.y;
         rotation[2] = newRotation.z;
      
         scale = new float[3];
         var newScale = transform.localScale;
         scale[0] = newScale.x;
         scale[1] = newScale.y;
         scale[2] = newScale.z;
      
         playerRegion = (int) player.PlayerRegion;

         useBike = player.UseBike;
         isSurfing = player.IsSurfing;
         beatenDojoChefs = player.BeatenDojoChefs;

         lastReha = player.lastReha;
      }
   }
}
