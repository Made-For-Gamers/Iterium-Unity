using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Singleton manager to manage static data
/// </summary>
public class Singleton : MonoBehaviour
{
    public static Singleton Instance; 

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {

    }


}
