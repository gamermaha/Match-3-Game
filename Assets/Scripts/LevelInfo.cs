using UnityEngine;

public struct LevelInfo
{
   private int timerValue;
   private Vector2Int tilesCount;
   private int gridSize;

   public int TimerValue
   {
      get { return timerValue; }
      set { timerValue = value; }
   }

   public Vector2Int TilesCount
   {
      get { return tilesCount; }
      set { tilesCount = value; }
   }

   public int GridSize
   {
      get { return gridSize; }
      set { gridSize = value; }
   }
   
   
}
