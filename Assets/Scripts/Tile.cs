using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerDownHandler
{
    #region Variables

    public event Action<Tile> OnClick;
    
    private Vector2Int index;
    private int id;
    private Sprite sprite;
    private bool matched = false;
    private bool locked = false;


    #endregion

    #region Properties

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


    #endregion

    #region Input

    private void OnMouseDown()
    {
        OnClick?.Invoke(this);
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        OnClick?.Invoke(this);
    }
    #endregion
}
