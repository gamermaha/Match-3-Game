using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public event Action<int> OnTileCountUpdate;
    private Dictionary<int, int> _countsOfTiles = new Dictionary<int, int>()
    {
        {1,0},{2,0},{3,0},{4,0},{5,0},{6,0},{7,0}
    };
    
    public void UpdateTileCount(int tileID)
    {
        _countsOfTiles[tileID] += 1;
        OnTileCountUpdate?.Invoke(tileID);
    }

    public void PrintStats()
    {
        string msg = "";
        
        for (int j = 0; j < _countsOfTiles.Count; j++)
        {
            msg += _countsOfTiles.ElementAt(j).Key + ":" + _countsOfTiles.ElementAt(j).Value + "\t";
        }
        msg += "\n";
        
        Debug.Log(msg);
    }
}
