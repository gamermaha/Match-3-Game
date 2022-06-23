using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;

public class TileManager : MonoBehaviour
{
    [SerializeField] private GameObject poolingContainer;
    [SerializeField] private GameObject powerUpContainer;
    [SerializeField] private Tile tileRef;
    [SerializeField] private List<Sprite> sprites = new List<Sprite>();
    [SerializeField] private List<Sprite> powerUps = new List<Sprite>();

    private bool _isSelected = false;
    private Tile _prevSelected = null;
    private Tile _currentSelected = null;
    private int _gridSize = 0;
    private GridCell[,] _grid;
    private int[] _countsOfTiles = new int[7];
    
    public Tile[] InstantiateTileArray(int arraySize)
    {
        Tile[] tileArray = new Tile[arraySize];
        for (int index = 0; index < arraySize; index++)
        {
            Tile newTile = Instantiate(tileRef);
            SetTileData(newTile);
            tileArray[index] = newTile;
        }
        return tileArray;
    }

    public void OnTileClickEnabled()
    {
        for (int i = 0; i < Board.Instance.GridSize; i++)
        {
            for (int j = 0; j < Board.Instance.GridSize; j++)
            {
                Board.Instance.grid[i,j].GetComponentInChildren<Tile>().OnClick += OnTileClicked;
            }
        }
    }

    public void OnTileClickDisabled()
    {
        for (int i = 0; i < Board.Instance.GridSize; i++)
        {
            for (int j = 0; j < Board.Instance.GridSize; j++)
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
            SetTileData(newTile);
        }
    }

    public void InstantiatePowerUpTiles()
    {
        for (int index = 0; index < 20; index++)
        {
            Tile newTile = Instantiate(tileRef);
            newTile.transform.SetParent(powerUpContainer.transform, false);
            SetPowerUpTileData(newTile);
            newTile.name = "" + newTile.Id;
            newTile.transform.localScale = Vector3.one * 0.5f;
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
        tile.Sprite = tile.GetComponent<SpriteRenderer>().sprite = sprites[index];
        tile.Id = index + 1;
    }

    private void SetPowerUpTileData(Tile tile)
    {
        int index = Random.Range(0, powerUps.Count);
        tile.GetComponent<SpriteRenderer>().sprite = powerUps[index];
        tile.Id = index + 8;
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

    private void CheckForPowerUps(Tile tile1, Tile tile2)
    {
        Tile powerUpTile4 = null;
        Tile powerUpTile5 = null;
        Tile tileSwappedWith4 = null;
        Tile tileSwappedWith5 = null;

        if (tile1.Id == 8)
        {
            powerUpTile4 = tile1;
            tileSwappedWith4 = tile2;
        }
        else if (tile2.Id == 8)
        {
            powerUpTile4 = tile2;
            tileSwappedWith4 = tile1;
        }
        else if (tile2.Id == 9)
        {
            powerUpTile5 = tile2;
            tileSwappedWith5 = tile1;
        }
        else if (tile1.Id == 9)
        {
            powerUpTile5 = tile1;
            tileSwappedWith5 = tile2;

        }
        if (powerUpTile4 != null) 
            PowerUpFor4Consecutive(powerUpTile4, tileSwappedWith4);
        if (powerUpTile5 != null) 
            PowerUpFor5Consecutive(powerUpTile5, tileSwappedWith5);
    }

    private void PowerUpFor4Consecutive(Tile powerUpTile, Tile tileToDestroy)
    {
        if (powerUpTile.Index.x == tileToDestroy.Index.x)
        {
            SetTilePositionAndIndex(powerUpTile, Board.Instance.grid[powerUpTile.Index.x, powerUpTile.Index.y]);
            powerUpTile.transform.SetParent(powerUpContainer.transform, false);
            DestroyRow(tileToDestroy.Index.x);
        }
        else if (powerUpTile.Index.y == tileToDestroy.Index.y)
        {
            SetTilePositionAndIndex(powerUpTile, Board.Instance.grid[powerUpTile.Index.x, powerUpTile.Index.y]);
            powerUpTile.transform.SetParent(powerUpContainer.transform, false);
            DestroyCol(tileToDestroy.Index.y);
        }
    }

    private void PowerUpFor5Consecutive(Tile powerUpTile, Tile tileToDestroy)
    {
        powerUpTile.transform.SetParent(powerUpContainer.transform, false);
        SetTilePositionAndIndex(TakeFromPool(), Board.Instance.grid[powerUpTile.Index.x, powerUpTile.Index.y]);
        DestroySimilarTile(tileToDestroy.Id);
    }
    
    private void CheckForMatches(Tile tile1, Tile tile2)
    { 
        Board.Instance.StopTakingInputs();
        
        List<Tile> tileList1 = FindMatches(tile1);
        List<Tile> tileList2 = FindMatches(tile2);
       
        CheckForPowerUps(tile1, tile2);
       
        tileList1.AddRange(tileList2);
        StartCoroutine(FindAllMatches(tileList1));
    }

    IEnumerator FindAllMatches(List<Tile> tileListFromSwapping)
    {
        List<Tile> matches = tileListFromSwapping;
        while (true)
        { 
            for (int i = 0; i < Board.Instance.GridSize; i++)
            {
                for (int j = 0; j < Board.Instance.GridSize; j++)
                {
                    if (Board.Instance.grid[i, j].GetComponentInChildren<Tile>() != null)
                        matches.AddRange(FindMatches(Board.Instance.grid[i, j].GetComponentInChildren<Tile>()));
                }
            }
            DestroyTiles();
            yield return new WaitForSeconds(1f);
            Board.Instance.MoveTilesDown();
            Board.Instance.AskFromPool();
            yield return new WaitForSeconds(1f);
            
            if (matches.Count == 0)
            {
                PrintTileCount();
                Board.Instance.activeState = BoardState.Ready;
                Board.Instance.StartTakingInputs();
                yield break;
            }
            matches.Clear();
            yield return new WaitForSeconds(0.01f);
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
        for (int i = tile.Index.y + 1; i < Board.Instance.GridSize; i++)
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
        for (int i = tile.Index.x + 1; i < Board.Instance.GridSize; i++)
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
        List<Tile> matchFoundInRow = CheckTileListForMatches(matchesInRow);
        List<Tile> matchFoundInCol = CheckTileListForMatches(matchesInCol);

        if (matchFoundInRow.Count != 0 )
            return AddSwappedTileToMatches(matchFoundInRow, tile);
        if (matchFoundInCol.Count != 0)
            return AddSwappedTileToMatches(matchFoundInCol, tile);
        
        return new List<Tile>();
    }

    private List<Tile> AddSwappedTileToMatches(List<Tile> matchFoundList, Tile tile)
    {
        Board.Instance.grid[tile.Index.x, tile.Index.y].GetComponentInChildren<Tile>().Matched = true;
        tile.Matched = true;
        matchFoundList.Add(tile);
        return matchFoundList;
    }
    
    private List<Tile> CheckTileListForMatches(List<Tile> listToBeChecked)
    {
        if (listToBeChecked.Count >= 2)
        {
            for (int l = 0; l < listToBeChecked.Count; l++)
            {
                Board.Instance.grid[listToBeChecked[l].Index.x, listToBeChecked[l].Index.y].GetComponentInChildren<Tile>().Matched = true;
                listToBeChecked[l].Matched = true;
            }
            if (listToBeChecked.Count >= 4)
                ConvertToPowerUpTile(9, listToBeChecked[0]);
            else if (listToBeChecked.Count == 3)
                ConvertToPowerUpTile(8, listToBeChecked[0]);
            
            return listToBeChecked;
        }
        return new List<Tile>();
    }

    private void ConvertToPowerUpTile(int powerUpId, Tile tile)
    {
        UpdateTileCount(tile.Id);
        Board.Instance.grid[tile.Index.x, tile.Index.y]
            .GetComponentInChildren<Tile>().transform.SetParent(poolingContainer.transform, false);
        SetTilePositionAndIndex(powerUpContainer.transform.Find("" + powerUpId).GetComponent<Tile>(), Board.Instance.grid[tile.Index.x,tile.Index.y]);
    }

    private void DestroyTiles()
    {
        for (int i = 0; i < Board.Instance.GridSize; i++)
        {
            for (int j = 0; j < Board.Instance.GridSize; j++)
            {
                Tile tile = Board.Instance.grid[i, j].GetComponentInChildren<Tile>();
                if (tile != null && tile.Matched)
                {
                    UpdateTileCount(tile.Id);
                    RemoveTile(tile);
                }
            }
        }
    }

    private void DestroyRow(int i)
    {
        for (int j = 0; j < Board.Instance.GridSize; j++)
        {
            Tile tile = Board.Instance.grid[i, j].GetComponentInChildren<Tile>();
            if (tile != null)
            {
                // UpdateTileCount(tile.Id);
                // RemoveTile(tile);
                Board.Instance.grid[tile.Index.x, tile.Index.y].GetComponentInChildren<Tile>().Matched = true;
            }
        }
    }
    private void DestroyCol(int j)
    {
        for (int i= 0; i < Board.Instance.GridSize; i++)
        {
            Tile tile = Board.Instance.grid[i, j].GetComponentInChildren<Tile>();
            if (tile != null)
            {
                // UpdateTileCount(tile.Id);
                // RemoveTile(tile);
                Board.Instance.grid[tile.Index.x, tile.Index.y].GetComponentInChildren<Tile>().Matched = true;
            }
        }
    }

    private void DestroySimilarTile(int tileID)
    {
        for (int i = 0; i < Board.Instance.GridSize; i++)
        {
            for (int j= 0; j < Board.Instance.GridSize; j++)
            {
                Tile tile = Board.Instance.grid[i, j].GetComponentInChildren<Tile>();
                if (tile != null && tile.Id == tileID)
                {
                    // UpdateTileCount(tile.Id);
                    // RemoveTile(tile);
                    Board.Instance.grid[tile.Index.x, tile.Index.y].GetComponentInChildren<Tile>().Matched = true;
                }
            }
        }
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
        tile.Matched = false;
        tile.transform.SetSiblingIndex(poolingIndex);
    }

    private void UpdateTileCount(int tileID)
    {
        _countsOfTiles[tileID - 1] += 1;
    }
    
    private void PrintTileCount()
    { 
        string msg = "";
        
            for (int j = 0; j < _countsOfTiles.Length ; j++)
            {
                msg += j+1 + ":" + _countsOfTiles[j] + "\t";
            }
            msg += "\n";
        
        Debug.Log(msg);
    }
}