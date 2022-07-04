using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Video;

public class Board : MonoBehaviour
{
    #region Variables

    private int gridSize;

    public GridCell[,] grid;
    public BoardState activeState = BoardState.Init;

    [SerializeField] private GridController gridMaker;
    [SerializeField] private TileManager tileManager;

    private int _tileLength;
    private int _tileWidth;
    private Tile[] _tiles;
    private Camera _cam;

    #endregion


    #region Properties

    public int GridSize
    { 
        get { return gridSize; }
        set { gridSize = value; }
    }
    #endregion

    private void Start()
    {
        GridSize = GameManager.Instance.GiveGridSize();
        _tileLength = 1;
        _tileWidth = 1;

        _cam = Camera.main;
        _cam.transform.position = new Vector3(15.5f, 11, -36);

        CreateBoard();
        PopulateBoardWithTiles();
        activeState = BoardState.Ready;
        StartTakingInputs();
    }

    private void OnDisable()
    {
        PopulateWithLockedTiles();
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
        for (int i = rowLength - 1; i >= 0; i--)
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
        bool lockedTileInNextCol = false;
        bool lockedTileInCol = false;
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = gridSize-1; j >= 0; j--)
            {
                if (!lockedTileInCol && grid[i, j].GetComponentInChildren<Tile>() != null && grid[i, j].GetComponentInChildren<Tile>().Locked)
                    lockedTileInCol = true;
                
                if (!lockedTileInNextCol && i < gridSize-1 && grid[i+1, j].GetComponentInChildren<Tile>() != null && grid[i+1, j].GetComponentInChildren<Tile>().Locked)
                    lockedTileInNextCol = true;
                
                if (grid[i, j].GetComponentInChildren<Tile>() == null)
                    MoveTileDownOneByOne(i,j);
                
                if (lockedTileInNextCol &&
                    i < gridSize-1 && 
                    j < gridSize-1 && 
                    grid[i+1, j].GetComponentInChildren<Tile>() == null &&
                    grid[i, j+1].GetComponentInChildren<Tile>() != null &&
                    !grid[i, j+1].GetComponentInChildren<Tile>().Locked)
                {
                    if (!lockedTileInCol)
                    {
                        MoveTileDown(grid[i, j + 1].GetComponentInChildren<Tile>(),
                            grid[i + 1, j]);
                        grid[i + 1, j].TileID = grid[i, j + 1].TileID;
                        MoveTileDownOneByOne(i, j + 1);
                    }
                    else if(i < gridSize-2)
                    {
                        MoveTileDown(grid[i+2, j+1].GetComponentInChildren<Tile>(),
                            grid[i+1, j]);
                        grid[i + 1, j].TileID = grid[i, j + 1].TileID;
                        MoveTileDownOneByOne(i+2, j + 1);
                    }
                }
            }
            lockedTileInCol = false;
            lockedTileInNextCol = false;
        }
    }

    private void MoveTileDownOneByOne(int i, int j)
    {
        for (int moveDown = j; moveDown <= gridSize - 1; moveDown++)
        {
            if (moveDown < gridSize-1 && grid[i, moveDown+1].GetComponentInChildren<Tile>() != null &&
                grid[i, moveDown].GetComponentInChildren<Tile>() == null && !grid[i, moveDown+1].GetComponentInChildren<Tile>().Locked)
            {
                MoveTileDown(grid[i, moveDown + 1].GetComponentInChildren<Tile>(),
                    grid[i, moveDown]);
                grid[i, moveDown].TileID = grid[i, moveDown + 1].TileID;
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

        gridMaker.gameObject.transform.SetParent(transform);
    }

    private void PopulateBoardWithTiles()
    {
        _tiles = new Tile[gridSize * gridSize];
        _tiles = tileManager.InstantiateTileArray(gridSize * gridSize);

        tileManager.InstantiateTilesForPooling(100);
        tileManager.InstantiatePowerUpTiles();

        for (int i = 0; i < gridSize; i++)
        {

            for (int j = 0; j < gridSize; j++)
            {
                
                tileManager.SetTilePositionAndIndex(_tiles[(i * gridSize) + j], grid[i, j]);
                grid[i, j].TileID = grid[i, j].GetComponentInChildren<Tile>().Id;
            }
        }
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
                tileManager.SetTileData(grid[i,j].GetComponentInChildren<Tile>());
        }
        PopulateWithLockedTiles();
    }

    private void PopulateWithLockedTiles()
    {
        tileManager.SetLockedTiles(0,4);
        tileManager.SetLockedTiles(1,4);
        tileManager.SetLockedTiles(2,4);
        tileManager.SetLockedTiles(3,4);
        tileManager.SetLockedTiles(4,4);
        tileManager.SetLockedTiles(5,4);
        tileManager.SetLockedTiles(6,4);
        tileManager.SetLockedTiles(7,4);
        tileManager.SetLockedTiles(8,4);
        //tileManager.SetLockedTiles(9,4);
        tileManager.SetLockedTiles(4,0);
        tileManager.SetLockedTiles(4,1);
        tileManager.SetLockedTiles(4,2);
        tileManager.SetLockedTiles(4,3);
        tileManager.SetLockedTiles(4,4);
        tileManager.SetLockedTiles(4,5);
        tileManager.SetLockedTiles(4,6);
        tileManager.SetLockedTiles(4,7);
        tileManager.SetLockedTiles(4,8);
        //tileManager.SetLockedTiles(4,9);
    }
    private void EmptyBoard()
    {
        for (int i = 0; i < gridSize; i++)
        {

            for (int j = 0; j < gridSize; j++)
            {
                Destroy(grid[i,j].transform.GetChild(0).gameObject);
            }
        }
    }

    private void MoveTileDown(Tile tileToMove, GridCell finalDestination)
    {
        tileManager.SetTilePositionAndIndex(tileToMove, finalDestination);
        tileToMove.transform.DOShakePosition(0.4f, strength: new Vector3(0, 0.2f, 0), vibrato: 5, randomness: 1,
            snapping: false, fadeOut: true);
        finalDestination.TileID = finalDestination.GetComponentInChildren<Tile>().Id;
    }
}
