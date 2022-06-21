using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;

public class TileManager : MonoBehaviour
{
    [SerializeField] private GameObject poolingContainer;
    [SerializeField] private Tile tileRef;
    [SerializeField] private List<Sprite> sprites = new List<Sprite>();

    private bool _isSelected = false;
    private Tile _prevSelected = null;
    private Tile _currentSelected = null;
    private int _gridSize = 0;
    private GridCell[,] _grid;


    public Tile[] InstantiateTileArray(int arraySize)
    {
        Tile[] tileArray = new Tile[arraySize];
        for (int index = 0; index < arraySize; index++)
        {
            Tile newTile = Instantiate(tileRef);
            //newTile.OnClick += OnTileClicked;
            SetTileData(newTile);
            tileArray[index] = newTile;
        }
        return tileArray;
    }

    public void OnTileClickEnabled()
    {
        for (int i = 0; i < Board.Instance.gridSize; i++)
        {
            for (int j = 0; j < Board.Instance.gridSize; j++)
            {
                Board.Instance.grid[i,j].GetComponentInChildren<Tile>().OnClick += OnTileClicked;
            }
        }
    }

    public void OnTileClickDisabled()
    {
        for (int i = 0; i < Board.Instance.gridSize; i++)
        {
            for (int j = 0; j < Board.Instance.gridSize; j++)
            {
                Board.Instance.grid[i,j].GetComponentInChildren<Tile>().OnClick -= OnTileClicked;
            }
        } 
    }

    public void InstantiateTilesForPooling(int arraySize)
    {
        for (int index = 0; index < arraySize; index++)
        {
            Tile newTile = Instantiate(tileRef);
            newTile.transform.SetParent(poolingContainer.transform, false);
            newTile.OnClick += OnTileClicked;
            SetTileData(newTile);
        }
    }
    
    public void SetTilePositionAndIndex(Tile tile, GridCell gridCell)
    {
        tile.transform.SetParent(gridCell.transform, false);
        tile.transform.localPosition = new Vector3(0, 0, 0);
        tile.Index = gridCell.Index;
    }
    public Tile TakeFromPool()
    {
        return poolingContainer.GetComponentInChildren<Tile>();
    }
    
    private void SetTileData(Tile tile)
    { 
        int index = Random.Range(0, sprites.Count);
        tile.GetComponent<SpriteRenderer>().sprite = sprites[index];
        tile.Id = index + 1;
    }

    private void ShrinkTileSizeToZero(Vector2Int index, TweenCallback onComplete)
    {
        Tile tileToDestroy = Board.Instance.grid[index.x, index.y].GetComponentInChildren<Tile>();
        tileToDestroy.transform.DOScale(0, 0.5f)
            .SetEase(Ease.OutBack)
            .OnComplete(onComplete);
    }

    private void Select(Tile selectedTile)
    {
        _isSelected = true;
        _prevSelected = selectedTile;
        selectedTile.GetComponent<SpriteRenderer>().color = new Color(.5f, .5f, .5f, 1f);
    }

    private void Deselect(Tile selectedTile)
    {
        _isSelected = false;
        selectedTile.GetComponent<SpriteRenderer>().color = Color.white;
        _prevSelected = null;
    }

    private void OnTileClicked(Tile selectedTile)
    {
        if (_prevSelected == null)
            Select(selectedTile);
        else
        {
            if (selectedTile.Index.x == _prevSelected.Index.x)
            {
                if (selectedTile.Index.y == _prevSelected.Index.y + 1 ||
                    selectedTile.Index.y == _prevSelected.Index.y - 1)
                    SwapTiles(selectedTile, _prevSelected);
            }
            else if (selectedTile.Index.y == _prevSelected.Index.y)
            {
                if (selectedTile.Index.x == _prevSelected.Index.x + 1 ||
                    selectedTile.Index.x == _prevSelected.Index.x - 1)
                    SwapTiles(selectedTile, _prevSelected);
            }
            Deselect(_prevSelected);
        }
    }

    private void SwapTiles(Tile tile1, Tile tile2)
    {
        Vector2Int tempIndex = tile1.Index;
        SetTilePositionAndIndex(tile1, Board.Instance.grid[tile2.Index.x, tile2.Index.y]);
        SetTilePositionAndIndex(tile2, Board.Instance.grid[tempIndex.x, tempIndex.y]);
        Board.Instance.activeState = BoardState.MakingAMatch;
        CheckForMatches(tile1, tile2);
    }

    private void CheckForMatches(Tile tile1, Tile tile2)
    { 
        Board.Instance.StopTakingInputs();
        FindMatches(tile1);
        FindMatches(tile2);
        StartCoroutine(FindAllMatches());
    }

    IEnumerator FindAllMatches()
    {
        List<bool> matchNotFound = new List<bool>();
        while (true)
        {
            
            matchNotFound.Clear();
            List<Tile> matchFound = new List<Tile>();
            for (int i = 0; i < Board.Instance.gridSize; i++)
            {
                for (int j = 0; j < Board.Instance.gridSize; j++)
                {
                    StartCoroutine(Board.Instance.MoveTilesDown());
                    if (Board.Instance.grid[i, j].GetComponentInChildren<Tile>() != null)
                    {
                        matchFound.AddRange(FindMatches(Board.Instance.grid[i, j].GetComponentInChildren<Tile>()));
                        Debug.Log(matchFound.Count);
                        yield return new WaitForSeconds(0.0001f);
                    }
                }
            }
            Board.Instance.activeState = BoardState.RefillFromPool;
            Board.Instance.AskFromPool();
            if (matchFound.Count == 0)
            {
                Board.Instance.activeState = BoardState.Ready;
                Board.Instance.StartTakingInputs();
                yield break;
            }
        }
    }

    private List<Tile> FindMatches(Tile tile)
    {
        List<Tile> tilesMatchedInRow = FindMatchesInRow(tile);
        List<Tile> tilesMatchedInCol = FindMatchesInCol(tile);
        return MatchFound(tilesMatchedInRow, tilesMatchedInCol, tile);
    }

    private List<Tile> FindMatchesInRow(Tile tile)
    {
        List<Tile> tilesMatched = new List<Tile>();
        Tile tileToBeChecked;
        for (int i = tile.Index.y - 1; i >= 0; i--)
        {
            if (Board.Instance.grid[tile.Index.x, i].GetComponentInChildren<Tile>() != null)
            {
                tileToBeChecked = Board.Instance.grid[tile.Index.x, i].GetComponentInChildren<Tile>();
                if (Equals(tileToBeChecked.Id, tile.Id))
                    tilesMatched.Add(tileToBeChecked);
                else
                    break;
            }
        }
        for (int i = tile.Index.y + 1; i < Board.Instance.gridSize; i++)
        {
            if (Board.Instance.grid[tile.Index.x, i].GetComponentInChildren<Tile>() != null)
            {
                tileToBeChecked = Board.Instance.grid[tile.Index.x, i].GetComponentInChildren<Tile>();
                if (Equals(tileToBeChecked.Id, tile.Id))
                    tilesMatched.Add(tileToBeChecked);
                else
                    break;
            }
        }
        return tilesMatched;
    }

    private List<Tile> FindMatchesInCol(Tile tile)
    {
        List<Tile> tilesMatched = new List<Tile>();
        Tile tileToBeChecked = null;
        for (int i = tile.Index.x - 1; i >= 0; i--)
        {
            if (Board.Instance.grid[i, tile.Index.y].GetComponentInChildren<Tile>() != null)
            {
                tileToBeChecked = Board.Instance.grid[i, tile.Index.y].GetComponentInChildren<Tile>();
                if (tileToBeChecked.Id == tile.Id)
                    tilesMatched.Add(tileToBeChecked);
                else
                    break;
            }
        }
        for (int i = tile.Index.x + 1; i < Board.Instance.gridSize; i++)
        {
            if (Board.Instance.grid[i, tile.Index.y].GetComponentInChildren<Tile>() != null)
            {
                tileToBeChecked = Board.Instance.grid[i, tile.Index.y].GetComponentInChildren<Tile>();
                if (tileToBeChecked.Id == tile.Id)
                    tilesMatched.Add(tileToBeChecked);
                else
                    break;
            }
        }

        return tilesMatched;
    }

    private List<Tile> MatchFound(List<Tile> matchesInRow, List<Tile> matchesInCol, Tile tile)
    {
        
        List<Tile> matchFoundInRow = CheckTileListForRemoval(matchesInRow);
        List<Tile> matchFoundInCol = CheckTileListForRemoval(matchesInCol);

        if (matchFoundInRow.Count != 0 )
        {
            RemoveTile(tile);
            return matchFoundInRow;
        }
        else if (matchFoundInCol.Count != 0)
        {
            RemoveTile(tile);
            return matchFoundInCol;
        }
        else
        {
            return new List<Tile>();
        }
    }

    private List<Tile> CheckTileListForRemoval(List<Tile> listToBeChecked)
    {
        if (listToBeChecked.Count >= 2)
        {
            for (int l = 0; l < listToBeChecked.Count; l++)
                RemoveTile(listToBeChecked[l]);
            return listToBeChecked;
        }
        else
            return new List<Tile>();
    }
    private void RemoveTile(Tile tile)
    {
        ShrinkTileSizeToZero(new Vector2Int(tile.Index.x, tile.Index.y),
            () => SetTileInPoolingContainer(tile));
    }

    private void SetTileInPoolingContainer(Tile tile)
    {
        int poolingIndex = Random.Range(0, poolingContainer.transform.childCount - 1);
        Board.Instance.grid[tile.Index.x, tile.Index.y].TileID = -1;
        tile.transform.SetParent(poolingContainer.transform, false);
        tile.transform.localScale = Vector3.one;
        tile.transform.SetSiblingIndex(poolingIndex);
    }
}