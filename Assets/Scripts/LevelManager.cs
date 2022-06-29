using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public event Action<string> OnRequirementMet;
    public LevelInfo levelInfo;
    [SerializeField] private Timer timerRef;
    [SerializeField] private StatsManager statsManager;
    private Dictionary<int, int> _levelRequirementForTiles = new Dictionary<int, int>()
    {
        {1,0}, {2,0}, {3,0}, {4,0}, {5,0}, {6,0}, {7,0}
    };
    private Dictionary<int, int> _collectedTiles = new Dictionary<int, int>()
    {
        {1,0},{2,0},{3,0},{4,0},{5,0},{6,0},{7,0}
    };

    private Dictionary<string, bool> _levelRequirementMet = new Dictionary<string, bool>()
    {
        {"Timer", false},{"Tile:"+1,false},{"Tile:"+2,false},{"Tile:"+3,false},{"Tile:"+4,false},{"Tile:"+5,false},{"Tile:"+6,false},{"Tile:"+7,false}
    };

    private void Start()
    {
        statsManager.OnTileCountUpdate += OnTileUpdateRequirementCheck;
        
        for (int i = 1; i <= _levelRequirementForTiles.Count; i++)
            _levelRequirementForTiles[i] = levelInfo.TilesCount[i - 1].y;
    }
    
    public void StartLevel()
    {
        StartTimer();
    }
    
    private void StartTimer()
    {
        Timer timer = Instantiate(timerRef);
        timer.timeRemaining = levelInfo.TimerValue;
        timer.OnTimeEnd += TimerOnOnTimeEnd;
    }

    private void TimerOnOnTimeEnd()
    {
        OnRequirementMet?.Invoke("Timer Ended");
    }

    private void OnTileUpdateRequirementCheck(int tileId)
    {
        _collectedTiles[tileId] += 1;
        if (_collectedTiles[tileId] >= _levelRequirementForTiles[tileId] && !_levelRequirementMet["Tile:" + tileId])
        {
            _levelRequirementMet["Tile:" + tileId] = true;
            OnRequirementMet?.Invoke("Tile:" + tileId);
        }
    }
    
    
    
    
}
