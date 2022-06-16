using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Board : MonoBehaviour
{
    public static Board Instance;
    public int gridSize;
    public GridCell[,] grid;
    
    [SerializeField] private GridController gridMaker;
    [SerializeField] private TileManager tileManager;

    
    private int _tileLength;
    private int _tileWidth;
    
    private Tile[] tiles;
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

        tiles = new Tile[gridSize * gridSize];
        tiles = tileManager.InstantiateTileArray(gridSize * gridSize);

        tileManager.InstantiateTilesForPooling(10);
        
        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                tileManager.SetTilePositionAndIndex(tiles[(row * gridSize) + col], grid[row, col]);
                grid[row, col].TileID = grid[row, col].GetComponentInChildren<Tile>().Id;
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

   
}
