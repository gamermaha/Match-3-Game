using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Sprite tile;

    public void SetCamera(int gridSize)
    {
        float orthoSize = tile.bounds.size.x * gridSize + Screen.height / Screen.width + 2f;
        Camera.main.orthographicSize = orthoSize;
        
        Camera.main.transform.position = new Vector3(gridSize + 2, gridSize + 13, -10);
    }
}
