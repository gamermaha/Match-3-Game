using System;
using System.Collections.Generic;
using eeGames.Widget;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region Variables
    public event Action<int> OnRequirementMet;
    public event Action<int> OnTileCountUpdate;
    public event Action OnAllRequirementsMet;
    public LevelInfo currentLevel;
    public List<LevelInfo> levelsInfo;

    [SerializeField] private Timer timerRef;
    [SerializeField] private StatsManager statsManager;
    
    private int _currentLevelValue;
    private Timer timer;
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
    

    #endregion

    #region Unity Functions

    private void Awake()
    {
        _currentLevelValue = GetLevel();
        SetLevelInfoForCurrentLevel();
    }

    private void Start()
    {
        statsManager.OnTileCountUpdate += OnTileUpdateRequirementCheck;
        LoadLevelInfoForCurrentLevel();
    }

    #endregion
    
    #region Level Implementation

     public void CheckLevelNumber(int levelSelected)
    {
        if (levelSelected == 0) // for next level
        {
            OnLevelComplete();
            LoadLevel();
            return;
        }
        if (levelSelected == -1) // for retry
        {
            LoadLevel();
            return;
        }
        if (levelSelected <= _currentLevelValue)
        {
            var widget = WidgetManager.Instance.GetWidget(WidgetName.ScrollView) as Levels;
            if (widget != null)
                widget.LoadPlayGameScreen();

            _currentLevelValue = levelSelected;
            LoadLevel();
            return;
        }
    }
     
    public void StartLevel() => StartTimer();

    public LevelInfo AskForLevelInfo(int currentLevel)
    {
        return levelsInfo[currentLevel-1];
    }
    
    public void UpdateLevelOnLevelsMenu()
    {
        var widget = WidgetManager.Instance.GetWidget(WidgetName.ScrollView) as Levels;
        if (widget != null)
            widget.SetPositionAndInteractable(_currentLevelValue);
    }

    private void LoadLevel()
    {
        SetLevelInfoForCurrentLevel();
        LoadLevelInfoForCurrentLevel();
        GameManager.Instance.StartGamePlay(_currentLevelValue);
    }
    
    private void OnLevelComplete()
    {
        _currentLevelValue++;
        SaveLevel();
    }
    
    private void SaveLevel()
    {
        PlayerPrefs.SetInt("Current Level", _currentLevelValue);
    }

    private int GetLevel()
    {
        return PlayerPrefs.GetInt("Current Level", 1);
    }
    
    private void SetLevelInfoForCurrentLevel()
    {
        if (_currentLevelValue <= 8)
            currentLevel = levelsInfo[_currentLevelValue - 1];
        else if (_currentLevelValue >= 9)
            currentLevel = levelsInfo[0];
    }

    private void LoadLevelInfoForCurrentLevel()
    {
        _collectedTiles = new Dictionary<int, int>()
        {
            {1,0},{2,0},{3,0},{4,0},{5,0},{6,0},{7,0}
        };
        _levelRequirementMet = new Dictionary<string, bool>()
        {
            {"Tile:"+1,false},{"Tile:"+2,false},{"Tile:"+3,false},{"Tile:"+4,false},{"Tile:"+5,false},{"Tile:"+6,false},{"Tile:"+7,false}
        };
        for (int i = 1; i <= _levelRequirementForTiles.Count; i++)
            _levelRequirementForTiles[i] = currentLevel.TilesCount[i - 1].y;
    }
    #endregion
   
    #region Timer Implementation

    public void DestroyTimer() => Destroy(timer.gameObject);
    
    private void StartTimer()
    {
        timer = Instantiate(timerRef);
        timer.timeRemaining = currentLevel.TimerValue;
        GameManager.Instance.timeUp = false;
        timer.OnTimeEnd += TimerOnOnTimeEnd;
        timer.OnTimeUpdate += TimerOnOnTimeUpdate;
    }

    private void TimerOnOnTimeUpdate(float timerValue)
    {
        var widget = WidgetManager.Instance.GetWidget(WidgetName.PlayGame) as PlayGame;
        if (widget != null)
            widget.TimeUpdate(timerValue);
        if (timerValue <= 0.5f)
            GameManager.Instance.timeUp = true; 
    }
    
    private void TimerOnOnTimeEnd() => StartCoroutine(GameManager.Instance.OnTimeUp());

    #endregion

    #region Tile Requirement Check

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

    #endregion
}
