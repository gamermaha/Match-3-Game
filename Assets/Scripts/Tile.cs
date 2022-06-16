using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Vector2Int index;
    private int id;
    private Sprite sprite;

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
    
    private void OnMouseDown()
    { 
        Debug.Log("I was clicked");
        OnClick?.Invoke(this);
    }
}
