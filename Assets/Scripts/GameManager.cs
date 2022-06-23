using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; 
    [SerializeField] private LevelManager levelManagerRef;
    [SerializeField] private Board boardRef;
    // [SerializeField] private GridController gridController;
    // [SerializeField] private MenuManager menuManager;
    [SerializeField] private Timer timerRef;
    //
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    { 
       LevelInfo levelInfo = levelManagerRef.LoadLevelInfo();
       boardRef.GridSize = levelInfo.GridSize;
       Timer timer = Instantiate(timerRef);
       timer.timeRemaining = levelInfo.TimerValue;
    }

    
    


}
