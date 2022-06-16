using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField] private GridCell gridCellRef;

    public GridCell[,] GridMaker(int gridSize, int tileLength, int tileWidth)
    {
        GridCell[,] grid = new GridCell[gridSize, gridSize];
        
        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                GridCell gridCell = Instantiate(gridCellRef, transform);
                gridCell.transform.localScale = new Vector3(tileLength, tileWidth);
                gridCell.transform.position = new Vector3((transform.position.x + (2.5f * tileLength * row)),
                    (transform.position.y + (2.5f * tileWidth * col)), 0);
                gridCell.Index = new Vector2Int(row, col);
                grid[row, col] = gridCell;
            }
        }
        return grid;
    }
}
