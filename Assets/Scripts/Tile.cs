using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{
    #region Variables

    public event Action<Tile, TileMovementDirection> OnClick;
    
    private Vector2Int index;
    private int id;
    private Sprite sprite;
    private bool matched = false;
    private bool locked = false;
    private bool isBeingHeld;

    private float startPosX;
    private float startPosY;
    private float startPosZ;
    

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

    private void Update()
    {
        if (isBeingHeld)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            transform.localPosition = new Vector3(mousePos.x-startPosX, mousePos.y-startPosY, mousePos.z-startPosZ);
            if (transform.localPosition.x >= 1f)
            {
                OnClick?.Invoke(this, TileMovementDirection.right);
                isBeingHeld = false;
            }
            else if (transform.localPosition.x <= -1f)
            {
                OnClick?.Invoke(this, TileMovementDirection.left);
                isBeingHeld = false;
            }
            else if (transform.localPosition.y >= 1f)
            {
                OnClick?.Invoke(this, TileMovementDirection.up);
                isBeingHeld = false;
            }
            else if (transform.localPosition.y <= -1f)
            {
                OnClick?.Invoke(this, TileMovementDirection.down);
                isBeingHeld = false;
            }
        }
    }

    #region Input

    private void OnMouseDown()
    {
        if (!GameManager.Instance.timeUp &&  !locked)
    
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
    
            startPosX = mousePos.x - transform.localPosition.x;
            startPosY = mousePos.y - transform.localPosition.y;
            startPosZ = mousePos.z - transform.localPosition.z;
            isBeingHeld = true;
        }
    }

    private void OnMouseUp()
    {
        isBeingHeld = false;
    }
    
    #endregion
}
