using System.Collections;
using UnityEngine;

namespace Iterium
{
    /// <summary>
    /// Zoom out the orthographic camera when starting an arena;
    /// </summary>

    public class CameraZoom : MonoBehaviour
    {
        [SerializeField] private float zoomLevel = 8f;
        [SerializeField] private float speed = 0.3f;

        private void Start()
        {
            StartCoroutine(ZoomCamera());
        }

        IEnumerator ZoomCamera()
        {
            for (float i = Camera.main.orthographicSize; i >= zoomLevel; i--)
            {
                Camera.main.orthographicSize = i;
                yield return new WaitForSeconds(speed);
            }
        }
    }
}