using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Zoom out the orthographic camera when starting an arena;
/// </summary>
public class CameraZoom : MonoBehaviour
{
    [SerializeField] private float zoomLevel = 8f;
    [Header("Zoom Speed in milliseconds")]
    [SerializeField] private int speed = 300;

    private void Start()
    {
        ZoomCamera();
    }

    async void ZoomCamera()
    {
        for (float i = Camera.main.orthographicSize; i >= zoomLevel; i--)
        {
            Camera.main.orthographicSize = i;
            await Task.Delay(speed);
        }
    }
}
