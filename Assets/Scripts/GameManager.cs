using System.Collections;
using eeGames.Widget;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variables

    public static GameManager Instance;
    public bool timeUp;
    
    [SerializeField] private LevelManager levelManagerRef;
    [SerializeField] private Board boardRef;
    
    private LevelInfo currentLevelInfo;
    private int _currentLevel;

    #endregion

    #region Unity Functions

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
        
    }

    private void Start() => WidgetManager.Instance.Push(WidgetName.StartGame);

    #endregion

    #region Level Manager Refs

    public void SetLevelValue(int levelSelected) => levelManagerRef.CheckLevelNumber(levelSelected);
    public void CallLevelManagerToUpdateLevelOnLevelsMenu() => levelManagerRef.UpdateLevelOnLevelsMenu();
    private void LevelManagerRefOnOnTileCountUpdate(int tileID)
    {
        var widget = WidgetManager.Instance.GetWidget(WidgetName.PlayGame) as PlayGame;
        if (widget != null)
            widget.UpdateTileCount(tileID);
    }
    
    private void LevelManagerRefOnOnAllRequirementsMet()
    {
        timeUp = true;
        StartCoroutine(OnLevelComplete());
    }
    
    private void LevelManagerRefOnOnRequirementMet(int tileID)
    {
        var widget = WidgetManager.Instance.GetWidget(WidgetName.PlayGame) as PlayGame;
        if (widget != null)
            widget.RequirementMet(tileID);
    }

    #endregion

    #region Board Settings
    public int GiveGridSize()
    {
        return currentLevelInfo.GridSize;
    }
    
    private void BoardSetInactive() => boardRef.gameObject.SetActive(false);
    #endregion

    #region Own Methods

    public void StartGamePlay(int levelSelected)
    {
        var widget = WidgetManager.Instance.GetWidget(WidgetName.PlayGame) as PlayGame;
        if (widget != null)
        {
            currentLevelInfo = levelManagerRef.AskForLevelInfo(levelSelected);
            widget.SetLevelRequirements(currentLevelInfo);
        }
        boardRef.gameObject.SetActive(true);
        levelManagerRef.StartLevel();
    }
    
    public IEnumerator OnTimeUp()
    {
        timeUp = true;
        boardRef.activeState = BoardState.Init;
        yield return new WaitForSeconds(2f);
        BoardSetInactive();
        levelManagerRef.DestroyTimer();
        var widget = WidgetManager.Instance.GetWidget(WidgetName.PlayGame) as PlayGame;
        if (widget != null)
            widget.TimeUp();

    }
    
    private IEnumerator OnLevelComplete()
    {
        boardRef.activeState = BoardState.Init;
        yield return new WaitForSeconds(2f);
        BoardSetInactive();
        levelManagerRef.DestroyTimer();
        var widget = WidgetManager.Instance.GetWidget(WidgetName.PlayGame) as PlayGame;
        if (widget != null)
            widget.AllRequirementsMet();
        timeUp = false;
    }

    #endregion
    
}
