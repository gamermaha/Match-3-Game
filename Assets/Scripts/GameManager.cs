using System.Collections;
using eeGames.Widget;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private LevelManager levelManagerRef;
    [SerializeField] private Board boardRef;
    
    private LevelInfo levelInfo;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);

        levelManagerRef.OnTileCountUpdate += LevelManagerRefOnOnTileCountUpdate;
        levelManagerRef.OnRequirementMet += LevelManagerRefOnOnRequirementMet;
        levelManagerRef.OnAllRequirementsMet += LevelManagerRefOnOnAllRequirementsMet;
        levelInfo = levelManagerRef.levelInfo;
    }

    private void Start()
    {
        WidgetManager.Instance.Push(WidgetName.StartGame);
    }

    public void StartGamePlay()
    {
        var widget = WidgetManager.Instance.GetWidget(WidgetName.PlayGame) as PlayGame;
        if (widget != null)
            widget.SetLevelRequirements(levelInfo);
        
        boardRef.gameObject.SetActive(true);
        levelManagerRef.StartLevel();
    }
    public int GiveGridSize()
    {
        return levelInfo.GridSize;
    }

    public void TimeUp()
    {
        boardRef.gameObject.SetActive(false);
        //StartCoroutine(boardRef.Griddd());
    }
    private void LevelManagerRefOnOnTileCountUpdate(int tileID)
    {
        var widget = WidgetManager.Instance.GetWidget(WidgetName.PlayGame) as PlayGame;
        if (widget != null)
        {
            widget.UpdateTileCount(tileID);
        }
    }
    
    private void LevelManagerRefOnOnAllRequirementsMet()
    {
        var widget = WidgetManager.Instance.GetWidget(WidgetName.PlayGame) as PlayGame;
        if (widget != null)
            widget.AllRequirementsMet();
        boardRef.gameObject.SetActive(false);
    }

    private void LevelManagerRefOnOnRequirementMet(int tileID)
    {
        var widget = WidgetManager.Instance.GetWidget(WidgetName.PlayGame) as PlayGame;
        if (widget != null)
        {
            widget.RequirementMet(tileID);
        }
    }
}
