using System.Collections.Generic;
using eeGames.Widget;
using TMPro;
using UnityEngine;

public class PlayGame : Widget
{
    
    [SerializeField] private List<TMP_Text> tilesCount;
    [SerializeField] private TMP_Text timerValue;
    private List<int> _tilesCount;
    
    public void SetLevelRequirements(LevelInfo levelRequirements)
    {
        _tilesCount = new List<int>();
        for (int i = 0; i < tilesCount.Count; i++)
        {
            _tilesCount.Add(levelRequirements.TilesCount[i].y);
            tilesCount[i].text = "" + _tilesCount[i];
        }
        float minutes = Mathf.FloorToInt(levelRequirements.TimerValue / 60);  
        float seconds = Mathf.FloorToInt(levelRequirements.TimerValue % 60);
        timerValue.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    
    public void UpdateTileCount(int tileID)
    {
        if (_tilesCount[tileID-1] > 0)
        {
            _tilesCount[tileID-1] -= 1;
            tilesCount[tileID-1].text = "" + _tilesCount[tileID-1];
        }
        if (_tilesCount[tileID-1] == 0)
        {
            RequirementMet(tileID);
        }
        
    }
    public void TimeUpdate(float time)
    {
        if (time > 0)
        {
            float minutes = Mathf.FloorToInt(time / 60);  
            float seconds = Mathf.FloorToInt(time % 60);
            timerValue.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void RequirementMet(int tileID)
    {
        tilesCount[tileID-1].text = "done";
    }

    public void AllRequirementsMet()
    {
        Hide();
        WidgetManager.Instance.Push(WidgetName.LevelCompleted);
    }

    public void TimeUp()
    {
        Hide();
        WidgetManager.Instance.Push(WidgetName.GameOver);
    }
    
}
