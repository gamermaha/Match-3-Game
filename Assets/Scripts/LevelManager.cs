using System;
using System.Collections.Generic;
using eeGames.Widget;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public event Action<int> OnRequirementMet;
    public event Action<int> OnTileCountUpdate;
    public event Action OnAllRequirementsMet;
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
        {"Tile:"+1,false},{"Tile:"+2,false},{"Tile:"+3,false},{"Tile:"+4,false},{"Tile:"+5,false},{"Tile:"+6,false},{"Tile:"+7,false}
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
        timer.OnTimeUpdate += TimerOnOnTimeUpdate;
    }

    private void TimerOnOnTimeUpdate(float timerValue)
    {
        var widget = WidgetManager.Instance.GetWidget(WidgetName.PlayGame) as PlayGame;
        if (widget != null)
        {
            widget.TimeUpdate(timerValue);
        }
    }
    
    private void TimerOnOnTimeEnd()
    {
        var widget = WidgetManager.Instance.GetWidget(WidgetName.PlayGame) as PlayGame;
        if (widget != null)
            widget.TimeUp();
        GameManager.Instance.TimeUp();
    }
    
    private void OnTileUpdateRequirementCheck(int tileId)
    {
        _collectedTiles[tileId] += 1;
        OnTileCountUpdate?.Invoke(tileId);
        if (_collectedTiles[tileId] >= _levelRequirementForTiles[tileId] && !_levelRequirementMet["Tile:" + tileId])
        {
            _levelRequirementMet["Tile:" + tileId] = true;
            CheckForRequirements(tileId);
        }
    }

    private void CheckForRequirements(int tileID)
    {
        OnRequirementMet?.Invoke(tileID);
        if (!_levelRequirementMet.ContainsValue(false))
            OnAllRequirementsMet?.Invoke();
    }
}
