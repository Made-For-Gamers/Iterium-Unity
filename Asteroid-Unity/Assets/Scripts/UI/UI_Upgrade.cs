using UnityEngine.UIElements;
using UnityEngine;
using UnityEditor.UIElements;

public class UI_Upgrade : MonoBehaviour
{
    [Header("UI Slider names")]
    [SerializeField] private string thrustSlider = "thrustSlider";
    [SerializeField] private string shieldSlider = "shieldSlider";
    [SerializeField] private string firepowerSlider = "firepowerSlider";

    [Header("Thrust Iterium Requirement")]
    [SerializeField] private int level1 = 10;
    [SerializeField] private int level2 = 20;
    [SerializeField] private int level3 = 30;

    private Slider thrust;
    private Button upgrade;

    private void OnEnable()
    {
        VisualElement uiRoot = GetComponent<UIDocument>().rootVisualElement;
        thrust = uiRoot.Q<Slider>(thrustSlider);
        upgrade = uiRoot.Q<Button>("thrustUpgrade");
        thrust.SetEnabled(false);
        upgrade.clicked += UpgradeThrust;
    }

    void UpgradeThrust()
    {
        thrust.value = 2;
    }
}
