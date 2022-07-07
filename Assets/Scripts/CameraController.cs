using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Sprite tile;

    public void SetCamera(int gridSize)
    {
        float orthoSize = tile.bounds.size.x * gridSize + Screen.height / Screen.width;
        Camera.main.orthographicSize = orthoSize;

        transform.position = new Vector3( (2.5f * (gridSize-1))/ 2, (2.5f * gridSize-1), -10);
    }
}
