using UnityEngine;
using DG.Tweening;

public class Board : MonoBehaviour
{
    public static Board Instance;
    private int gridSize;
    
    public GridCell[,] grid;
    public BoardState activeState = BoardState.Init;
    
    [SerializeField] private GridController gridMaker;
    [SerializeField] private TileManager tileManager;

    private int _tileLength;
    private int _tileWidth;
    private Tile[] _tiles;
    private Camera _cam;

    public int GridSize
    {
        get { return gridSize; }
        set { gridSize = value; }
    }
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
        // _gridSize = MetaData.Instance.scriptableInstance.rowsCols;
        // _tileLength = MetaData.Instance.scriptableInstance.length;
        // _tileWidth = MetaData.Instance.scriptableInstance.width;
        
        _tileLength = 1;
        _tileWidth = 1;

        _cam = Camera.main;
        _cam.transform.position = new Vector3(gridSize, gridSize, -60);
        
        CreateBoard();
        activeState = BoardState.Ready;
        StartTakingInputs();
    }

    public void StartTakingInputs()
    {
        tileManager.OnTileClickEnabled();
    }

    public void StopTakingInputs()
    {
        tileManager.OnTileClickDisabled();
    }
    public void PrintGrid()
    {
        int rowLength = gridSize;
        int colLength = gridSize;
        
        string msg = "";
        for (int i = rowLength-1; i >= 0; i--)
        {
            for (int j = 0; j < colLength; j++)
            {
                msg += grid[j, i].TileID + "\t";
            }
            msg += "\n";
        }
        Debug.Log(msg);
    }
    
     public void MoveTilesDown()
    {
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = gridSize-1; j >= 0; j--)
            {
                if (grid[i, j].GetComponentInChildren<Tile>() == null)
                {
                    for (int moveDown = j; moveDown <= gridSize-1; moveDown++)
                    { 
                        if (moveDown < gridSize - 1 && grid[i, moveDown + 1].GetComponentInChildren<Tile>() != null && grid[i, moveDown].GetComponentInChildren<Tile>() == null)
                        {
                            MoveTileDown(grid[i, moveDown + 1].GetComponentInChildren<Tile>(),
                                        grid[i, moveDown]);
                                    grid[i, moveDown].TileID = grid[i, moveDown + 1].TileID;

                        }
                    }
                }
            }
        }
    }
     
     public void AskFromPool()
     {
         for (int i = 0; i < gridSize; i++)
         {
             for (int j = 0; j < gridSize; j++)
             {
                 Tile tileFromPool = tileManager.TakeFromPool();
                 if (grid[i, j].GetComponentInChildren<Tile>() == null)
                 {
                     MoveTileDown(tileFromPool, grid[i, j]);
                 }
             }
         }
     }
     
     private void CreateBoard()
     {
         grid = new GridCell[gridSize, gridSize];
         grid = gridMaker.GridMaker(gridSize, _tileLength, _tileWidth);

         _tiles = new Tile[gridSize * gridSize];
         _tiles = tileManager.InstantiateTileArray(gridSize * gridSize);
        
         tileManager.InstantiateTilesForPooling(100);
         tileManager.InstantiatePowerUpTiles();
        
         for (int i = 0; i< gridSize; i++)
         {
            
             for (int j = 0; j < gridSize; j++)
             {
                 tileManager.SetTilePositionAndIndex(_tiles[(i * gridSize) + j], grid[i, j]);
                 grid[i, j].TileID = grid[i, j].GetComponentInChildren<Tile>().Id;
                 // if (i >= 2 && _tiles[(i * gridSize) + j].Id == grid[i - 1, j].TileID ||
                 //     _tiles[(i * gridSize) + j].Id == grid[i - 2, j].TileID)
                 // {
                 //     tileManager.SetTilePositionAndIndex(_tiles[Random.Range(0, _tiles.Length)], grid[i, j]);
                 //     grid[i, j].TileID = grid[i, j].GetComponentInChildren<Tile>().Id;
                 // }
                 //
                 // if (j >= 2 && _tiles[(i * gridSize) + j].Id == grid[i, j-1].TileID ||
                 //     _tiles[(i * gridSize) + j].Id == grid[i, j-2].TileID)
                 // {
                 //         tileManager.SetTilePositionAndIndex(_tiles[Random.Range(0, _tiles.Length)], grid[i, j]);
                 //         grid[i, j].TileID = grid[i, j].GetComponentInChildren<Tile>().Id;
                 // }
                 // else
                 // {
                 //     tileManager.SetTilePositionAndIndex(_tiles[(i * gridSize) + j], grid[i, j]);
                 //     grid[i, j].TileID = grid[i, j].GetComponentInChildren<Tile>().Id;
                 // }
             }
         }
     }
     
     private void MoveTileDown(Tile tileToMove, GridCell finalDestination)
     {
         tileManager.SetTilePositionAndIndex(tileToMove, finalDestination);
         tileToMove.transform.DOShakePosition(0.4f, strength: new Vector3(0, 0.2f, 0), vibrato: 5, randomness: 1, snapping: false, fadeOut: true);
         finalDestination.TileID = finalDestination.GetComponentInChildren<Tile>().Id;
     }

    


}
