using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public event Action<Tile> OnClick;
    
    private Vector2Int index;
    private int id;
    private Sprite sprite;
    private bool matched = false;
    private bool locked = false;
    
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
    
    public bool Locked
    {
        get { return locked; }
        set { locked = value; }
    }
    
    private void OnMouseDown()
    {
        OnClick?.Invoke(this);
    }
}
