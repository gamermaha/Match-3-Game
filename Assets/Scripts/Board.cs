using System.Collections;
using UnityEngine;
using DG.Tweening;
public class Board : MonoBehaviour
{
    public static Board Instance;
    public int gridSize;
    public GridCell[,] grid;
    
    [SerializeField] private GridController gridMaker;
    [SerializeField] private TileManager tileManager;

    
    private int _tileLength;
    private int _tileWidth;
    private Tile[] _tiles;
    private Camera _cam;
    
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

        gridSize = 10;
        _tileLength = 1;
        _tileWidth = 1;

        _cam = Camera.main;
        _cam.transform.position = new Vector3(gridSize, gridSize, -60);
        
        CreateBoard();
    }

    private void CreateBoard()
    {
        grid = new GridCell[gridSize, gridSize];
        grid = gridMaker.GridMaker(gridSize, _tileLength, _tileWidth);

        _tiles = new Tile[gridSize * gridSize];
        _tiles = tileManager.InstantiateTileArray(gridSize * gridSize);

        tileManager.InstantiateTilesForPooling(10);
        
        for (int i = 0; i< gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                tileManager.SetTilePositionAndIndex(_tiles[(i * gridSize) + j], grid[i, j]);
                grid[i, j].TileID = grid[i, j].GetComponentInChildren<Tile>().Id;
            }
        }
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
    
     public IEnumerator MoveTilesDown()
    {
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = gridSize-1; j >= 0; j--)
            {
                if (grid[i, j].GetComponentInChildren<Tile>() == null)
                {
                    for (int moveDown = j; moveDown <= gridSize-1; moveDown++)
                    {
                        Tile tileFromPool = tileManager.TakeFromPool();

                        if (moveDown < gridSize - 1)
                        {
                            if (grid[i, moveDown + 1].GetComponentInChildren<Tile>() != null)
                            {
                                MoveTileDown(grid[i, moveDown + 1].GetComponentInChildren<Tile>(),grid[i, moveDown]);
                                grid[i, moveDown].TileID = grid[i, moveDown + 1].TileID;
                                yield return new WaitForSeconds(0.000001f);
                            }

                            else
                            {
                                MoveTileDown(tileFromPool, grid[i, moveDown]);
                                yield return new WaitForSeconds(0.000001f);
                            }
                        }
                        else if (moveDown == gridSize - 1)
                        {
                            MoveTileDown(tileFromPool, grid[i, moveDown]);
                            yield return new WaitForSeconds(0.000001f);
                        }
                    }
                }
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
