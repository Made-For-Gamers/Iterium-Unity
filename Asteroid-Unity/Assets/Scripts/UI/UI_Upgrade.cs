using UnityEngine.UIElements;
using UnityEngine;
using UnityEditor.UIElements;

public class UI_Upgrade : MonoBehaviour
{
    [Header("UI Upgrade Sliders")]
    [SerializeField] private string thrustSlider = "thrustSlider";
    [SerializeField] private string shieldSlider = "shieldSlider";
    [SerializeField] private string firepowerSlider = "firepowerSlider";

    [Header("UI Upgrade Buttons")]
    [SerializeField] private string thrustButton = "thrustUpgrade";
    [SerializeField] private string shieldButton = "shieldUpgrade";
    [SerializeField] private string firepowerButton = "firepowerUpgrade";

    [Header("UI Iterium Label")]
    [SerializeField] private string iteriumLabel = "iteriumAmount";

    [Header("Thrust Iterium Requirement")]   
    [SerializeField] private int thrustLevel1 = 10;
    [SerializeField] private int thrustLevel2 = 30;

    [Header("Shield Iterium Requirement")]  
    [SerializeField] private int shieldLevel1 = 10;
    [SerializeField] private int shieldLevel2 = 30;

    [Header("Firepower Iterium Requirement")]   
    [SerializeField] private int firepowerLevel1 = 10;
    [SerializeField] private int firepowerLevel2 = 30;

    private Slider thrust;
    private Button thrustUpgrade;
    private Slider shield;
    private Button shieldUpgrade;
    private Slider firepower;
    private Button firepowreUpgrade;
    private Label iterium;

    private void OnEnable()
    {
        //UI Toolkit Document
        VisualElement uiRoot = GetComponent<UIDocument>().rootVisualElement;
        iterium = uiRoot.Q<Label>(iteriumLabel);


        //UI thrust elements
        thrust = uiRoot.Q<Slider>(thrustSlider);
        thrustUpgrade = uiRoot.Q<Button>(thrustButton);
        thrust.SetEnabled(false);
        thrustUpgrade.clicked += UpgradeThrust;

        //UI shield elements
        shield = uiRoot.Q<Slider>(shieldSlider);
        shieldUpgrade = uiRoot.Q<Button>(shieldButton);
        shield.SetEnabled(false);
        shieldUpgrade.clicked += UpgradeShield;

        //UI firepower elements
        firepower = uiRoot.Q<Slider>(firepowerSlider);
        firepowreUpgrade = uiRoot.Q<Button>(firepowerButton);
        firepower.SetEnabled(false);
        firepowreUpgrade.clicked += UpgradeFirepower;
    }

    private void Start()
    {
        ChangeIterium();
        GameManager.Instance.player.onChange_Iterium.AddListener(ChangeIterium);
    }

    void UpgradeThrust()
    {
        if (GameManager.Instance.player.SpeedLvl == 0 && GameManager.Instance.player.Iterium >= thrustLevel1)
        {
            //Upgrade speed to lvl 1
            GameManager.Instance.player.Iterium -= thrustLevel1;
            GameManager.Instance.player.SpeedLvl = 1;
            thrust.value = 1;
        }
        else if (GameManager.Instance.player.SpeedLvl == 1 && GameManager.Instance.player.Iterium >= thrustLevel2)
        {
            //Upgrade speed to lvl 2
            GameManager.Instance.player.Iterium -= thrustLevel2;
            GameManager.Instance.player.SpeedLvl = 2;
            thrust.value = 2;
        }
    }

    void UpgradeShield()
    {
        if (GameManager.Instance.player.ShieldLvl == 0 && GameManager.Instance.player.Iterium >= shieldLevel1)
        {
            //Upgrade shield to lvl 1
            GameManager.Instance.player.Iterium -= shieldLevel1;
            GameManager.Instance.player.ShieldLvl = 1;
            shield.value = 1;
        }
        else if (GameManager.Instance.player.ShieldLvl == 1 && GameManager.Instance.player.Iterium >= shieldLevel2)
        {
            //Upgrade shield to lvl 1
            GameManager.Instance.player.Iterium -= shieldLevel2;
            GameManager.Instance.player.ShieldLvl = 2;
            shield.value = 2;
        }
    }

    void UpgradeFirepower()
    {
        if (GameManager.Instance.player.BulletLvl == 0 && GameManager.Instance.player.Iterium >= firepowerLevel1)
        {
            //Upgrade bullet to lvl 1
            GameManager.Instance.player.Iterium -= firepowerLevel1;
            GameManager.Instance.player.BulletLvl = 1;
            firepower.value = 1;
        }
        else if (GameManager.Instance.player.BulletLvl == 1 && GameManager.Instance.player.Iterium >= firepowerLevel2)
        {
            //Upgrade bullet to lvl 2
            GameManager.Instance.player.Iterium -= firepowerLevel2;
            GameManager.Instance.player.BulletLvl = 2;
            firepower.value = 2;
        }
    }

    void ChangeIterium()
    {
        iterium.text = GameManager.Instance.player.Iterium.ToString();
    }


}
