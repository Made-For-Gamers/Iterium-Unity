using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Display the FPS to a Text field
/// </summary>

public class FPSCounter : MonoBehaviour
{
    const float fpsMeasurePeriod = 1f;
    private int fpsCalc = 0;
    private float period = 0;
    private int currentFps;  
    Text fpsText;

    private void Start()
    {
        fpsText = GetComponent<Text>();
        period = Time.realtimeSinceStartup + fpsMeasurePeriod;
    }

    private void Update()
    {       
        fpsCalc++;
        if (Time.realtimeSinceStartup > period)
        {
            currentFps = (int)(fpsCalc / fpsMeasurePeriod);
            fpsText.text = currentFps.ToString();
            fpsCalc = 0;
            period += fpsMeasurePeriod;
        }
    }
}

