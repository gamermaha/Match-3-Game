using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField] private GridCell gridCellRef;

    public GridCell[,] GridMaker(int gridSize, int tileLength, int tileWidth)
    {
        GridCell[,] grid = new GridCell[gridSize, gridSize];
        
        for (int i = 0; i< gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                GridCell gridCell = Instantiate(gridCellRef, transform);
                gridCell.transform.localScale = new Vector3(tileLength, tileWidth);
                gridCell.transform.position = new Vector3((transform.position.x + (2.5f * tileLength * i)),
                    (transform.position.y + (2.5f * tileWidth * j)), 0);
                gridCell.Index = new Vector2Int(i,j);
                grid[i, j] = gridCell;
            }
        }
        return grid;
    }
}
