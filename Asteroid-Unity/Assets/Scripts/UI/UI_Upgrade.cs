using UnityEngine.UIElements;
using UnityEngine;


public class UI_Upgrade : MonoBehaviour
{
    [Header("UI Slider names")]
    [SerializeField] private string thrustSlider;
    [SerializeField] private string shieldSlider;
    [SerializeField] private string firepowerSlider;

    [Header("Thrust Iterium Requirement")]
    [SerializeField] private int level1 = 10;
    [SerializeField] private int level2 = 20;
    [SerializeField] private int level3 = 30;

    private Slider thrust;

    private void OnEnable()
    {
        VisualElement uiRoot = GetComponent<UIDocument>().rootVisualElement;
        thrust = uiRoot.Q<Slider>(thrustSlider);
        thrust.RegisterCallback<ChangeEvent<float>>(ThrustChanged);
    }

    private void ThrustChanged(ChangeEvent<float> value)
    {
        if (GameManager.Instance.player.SpeedLvl <= 2 && value.newValue >= 3 && GameManager.Instance.player.Iterium >= level3)
        {
            thrust.value = 3;
            GameManager.Instance.player.Iterium -= level3;
            GameManager.Instance.player.SpeedLvl = 3;
        }
        else if (GameManager.Instance.player.SpeedLvl == 1 && value.newValue >= 2 && GameManager.Instance.player.Iterium >= level2)
        {
            thrust.value = 2;
            GameManager.Instance.player.Iterium -= level2;
            GameManager.Instance.player.SpeedLvl = 2;
        }
        else
        {
            thrust.value = value.previousValue;
        }
      
    }
}
