using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Board : MonoBehaviour
{
    #region Variables

    private int gridSize;

    public GridCell[,] grid;
    public BoardState activeState = BoardState.Init;
    public CameraController cameraController;
    [SerializeField] private GridController gridMaker;
    [SerializeField] private TileManager tileManager;

    private int _tileLength;
    private int _tileWidth;
    private Tile[] _tiles;
    

    #endregion


    #region Properties

    public int GridSize
    { 
        get { return gridSize; }
        set { gridSize = value; }
    }
    #endregion

    private void OnEnable()
    {
        GridSize = GameManager.Instance.GiveGridSize();
        _tileLength = 1;
        _tileWidth = 1;
        
        cameraController.SetCamera(GridSize);
        

        CreateBoard();
        PopulateBoardWithTiles();
        activeState = BoardState.Ready;
        StartTakingInputs();
    }

    private void OnDisable()
    {
        StopCoroutine(tileManager.FindAllMatches(new List<Tile>()));
        //EmptyBoard();
        for (int i = 0; i < GridSize*GridSize; i++)
        {
            Destroy(transform.GetChild(0).transform.GetChild(i).gameObject);
        }
    }

    public void StartTakingInputs()
    {
        if (activeState == BoardState.Ready)
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
        if (GridSize == 6)
            return;
        
        else if (GridSize == 7)
        {
            tileManager.SetLockedTiles(3,0);
            tileManager.SetLockedTiles(3,1);
            tileManager.SetLockedTiles(3,2);
            tileManager.SetLockedTiles(3,3);
            tileManager.SetLockedTiles(3,4);
            tileManager.SetLockedTiles(3,5);
            tileManager.SetLockedTiles(3,6);
        }
        else if (GridSize == 8)
        {
            tileManager.SetLockedTiles(1,0);
            tileManager.SetLockedTiles(1,1);
            tileManager.SetLockedTiles(1,2);
            tileManager.SetLockedTiles(1,3);
            tileManager.SetLockedTiles(1,4);
            tileManager.SetLockedTiles(1,5);
            tileManager.SetLockedTiles(1,6);
            tileManager.SetLockedTiles(1,7);
           

            tileManager.SetLockedTiles(6,0);
            tileManager.SetLockedTiles(6,1);
            tileManager.SetLockedTiles(6,2);
            tileManager.SetLockedTiles(6,3);
            tileManager.SetLockedTiles(6,4);
            tileManager.SetLockedTiles(6,5);
            tileManager.SetLockedTiles(6,6);
            tileManager.SetLockedTiles(6,7);
           
        }
        else if (GridSize == 9)
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
            
            tileManager.SetLockedTiles(4,0);
            tileManager.SetLockedTiles(4,1);
            tileManager.SetLockedTiles(4,2);
            tileManager.SetLockedTiles(4,3);
            tileManager.SetLockedTiles(4,4);
            tileManager.SetLockedTiles(4,5);
            tileManager.SetLockedTiles(4,6);
            tileManager.SetLockedTiles(4,7);
            tileManager.SetLockedTiles(4,8);
            return;
        }
        
        
    }
    
    private void EmptyBoard()
    {
        for (int i = 0; i < GridSize; i++)
        {

            for (int j = 0; j < GridSize; j++)
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
