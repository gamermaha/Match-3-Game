using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Vector2Int index;
    private int id;
    private Sprite sprite;
    private bool matched = false;
    private int powerUp = 0;
    

    public event Action<Tile> OnClick;

    public Vector2Int Index
    {
        get { return index; }
        set { index = value; }
    }

    public int Id
    {
        get { return id; }
        set { id = value; }
    }

    public Sprite Sprite
    {
        get { return sprite; }
        set { sprite = value; }
    }
    
    public bool Matched
    {
        get { return matched; }
        set { matched = value; }
    }

    public int PowerUp
    {
        get { return powerUp; }
        set { powerUp = value; }
    }
    
    private void OnMouseDown()
    {
        OnClick?.Invoke(this);
    }
}
