using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; 
    [SerializeField] private LevelManager levelManagerRef;
    [SerializeField] private Board boardRef;
    // [SerializeField] private GridController gridController;
    // [SerializeField] private MenuManager menuManager;
    
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
        
        levelManagerRef.OnRequirementMet += LevelManagerRefOnOnRequirementMet;
        levelInfo = levelManagerRef.levelInfo;
    }

    private void LevelManagerRefOnOnRequirementMet(string obj)
    {
        Debug.Log(obj+ " requirement met");
    }

    private void Start()
    { 
        levelManagerRef.StartLevel();
    }

    public int GiveGridSize()
    {
        return levelInfo.GridSize;
    }
    
    public void TimeEndedIsTrue()
    {
        Debug.Log("GameOver");
    }
}
