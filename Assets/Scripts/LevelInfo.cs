using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelInfo", menuName = "ScriptableObjects/LevelInfo", order = 1)]
public class LevelInfo : ScriptableObject
{
   public int timerValue;
   public Vector2Int[] tilesCount = new Vector2Int[7];
   public int gridSize;

   public int TimerValue
   {
      get { return timerValue; }
   }

   public Vector2Int[] TilesCount
   {
      get { return tilesCount; }
   }

   public int GridSize
   {
      get { return gridSize; }
   }
   
   
}
