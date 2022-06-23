using System;
using System.IO;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public LevelInfo LoadLevelInfo()
    { 
        LevelInfo levelInfo = new LevelInfo();

        string levelInfoString = File.ReadAllText("D:/Match-3 Game/Assets/Levels/Level01.txt");
        string[] levelInfoStringContents = levelInfoString.Split(new [] {"#Save-Value#"}, System.StringSplitOptions.None);
        string[] tilesCountSplit = levelInfoStringContents[1].Split(new [] {"-"}, System.StringSplitOptions.None);
        
        levelInfo.TimerValue = Int32.Parse(levelInfoStringContents[0]);
        levelInfo.TilesCount = new Vector2Int(Int32.Parse(tilesCountSplit[0]), Int32.Parse(tilesCountSplit[1]));
        levelInfo.GridSize = Int32.Parse(levelInfoStringContents[2]);
        return levelInfo;
    }
}
