using UnityEngine;

public class GridCell : MonoBehaviour
{
    private Vector2Int index;
    private int tileID;

    public Vector2Int Index
    {
        get { return index; }
        set { index = value; }
    }
    
    public int TileID
    {
        get { return tileID; }
        set { tileID = value; }
    }
    
    
    
}
