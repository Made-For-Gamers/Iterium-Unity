using UnityEngine.UIElements;
using UnityEngine;

public class UI_Upgrade : MonoBehaviour
{
    [Header("UI Upgrade Sliders")]
    [SerializeField] private string speedSlider = "thrustSlider";
    [SerializeField] private string shieldSlider = "shieldSlider";
    [SerializeField] private string firepowerSlider = "firepowerSlider";

    [Header("UI Upgrade Buttons")]
    [SerializeField] private string speedButton = "thrustUpgrade";
    [SerializeField] private string shieldButton = "shieldUpgrade";
    [SerializeField] private string firepowerButton = "firepowerUpgrade";

    [Header("UI Iterium Label")]
    [SerializeField] private string iteriumLabel = "iteriumAmount";

    [Header("Thrust Labels")]
    [SerializeField] private string speedLevel1Label = "thrustLevel1";
    [SerializeField] private string speedLevel2Label = "thrustLevel2";

    [Header("Shield Labels")]
    [SerializeField] private string shieldLevel1Label = "shieldLevel1";
    [SerializeField] private string shieldLevel2Label = "shieldLevel2";

    [Header("Firepower Labels")]
    [SerializeField] private string firepowerLevel1Label = "firepowerLevel1";
    [SerializeField] private string firepowerLevel2Label = "firepowerLevel2";

    //UI thrust controls
    private Slider thrust;
    private Button thrustUpgrade;
    private Label thrustLabelLevel1;
    private Label thrustLabelLevel2;

    //UI shield controls
    private Slider shield;
    private Button shieldUpgrade;
    private Label shieldLabelLevel1;
    private Label shieldLabelLevel2;

    //UI firepower controls
    private Slider firepower;
    private Button firepowerUpgrade;
    private Label firepowerLabelLevel1;
    private Label firepowerLabelLevel2;

    //UI iterium label
    private Label iterium;

    private void OnEnable()
    {
        //UI Toolkit Document
        VisualElement uiRoot = GetComponent<UIDocument>().rootVisualElement;
        iterium = uiRoot.Q<Label>(iteriumLabel);


        //UI thrust elements
        thrust = uiRoot.Q<Slider>(speedSlider);
        thrustUpgrade = uiRoot.Q<Button>(speedButton);
        thrustLabelLevel1 = uiRoot.Q<Label>(speedLevel1Label);
        thrustLabelLevel2 = uiRoot.Q<Label>(speedLevel2Label);
        thrust.SetEnabled(false);

        //UI shield elements
        shield = uiRoot.Q<Slider>(shieldSlider);
        shieldUpgrade = uiRoot.Q<Button>(shieldButton);
        shieldLabelLevel1 = uiRoot.Q<Label>(shieldLevel1Label);
        shieldLabelLevel2 = uiRoot.Q<Label>(shieldLevel2Label);
        shield.SetEnabled(false);

        //UI firepower elements
        firepower = uiRoot.Q<Slider>(firepowerSlider);
        firepowerUpgrade = uiRoot.Q<Button>(firepowerButton);
        firepowerLabelLevel1 = uiRoot.Q<Label>(firepowerLevel1Label);
        firepowerLabelLevel2 = uiRoot.Q<Label>(firepowerLevel2Label);
        firepower.SetEnabled(false);

        //Events
        thrustUpgrade.clicked += UpgradeThrust;
        shieldUpgrade.clicked += UpgradeShield;
        firepowerUpgrade.clicked += UpgradeFirepower;
    }

    private void OnDisable()
    {
        //Clean-up events
        thrustUpgrade.clicked -= UpgradeThrust;
        shieldUpgrade.clicked -= UpgradeShield;
        firepowerUpgrade.clicked -= UpgradeFirepower;
    }

    private void Start()
    {
        ChangeUpgradeLabels();
        ChangeIterium();
        ChangeUpgradeSliders();

        //UI Events
        GameManager.Instance.player.onChange_Iterium.AddListener(ChangeIterium);
    }

    void UpgradeThrust()
    {
        if (GameManager.Instance.player.SpeedLvl == 1 && GameManager.Instance.player.Iterium >= GameManager.Instance.speedLevel1)
        {
            //Upgrade speed to lvl 2
            GameManager.Instance.player.Iterium -= GameManager.Instance.speedLevel1;
            GameManager.Instance.player.SpeedLvl = 2;
            thrust.value = 1;
        }
        else if (GameManager.Instance.player.SpeedLvl == 2 && GameManager.Instance.player.Iterium >= GameManager.Instance.speedLevel2)
        {
            //Upgrade speed to lvl 3
            GameManager.Instance.player.Iterium -= GameManager.Instance.speedLevel2;
            GameManager.Instance.player.SpeedLvl = 3;
            thrust.value = 2;
        }
    }

    void UpgradeShield()
    {
        if (GameManager.Instance.player.ShieldLvl == 1 && GameManager.Instance.player.Iterium >= GameManager.Instance.shieldLevel1)
        {
            //Upgrade shield to lvl 2
            GameManager.Instance.player.Iterium -= GameManager.Instance.shieldLevel1;
            GameManager.Instance.player.ShieldLvl = 2;
            shield.value = 1;
        }
        else if (GameManager.Instance.player.ShieldLvl == 2 && GameManager.Instance.player.Iterium >= GameManager.Instance.shieldLevel2)
        {
            //Upgrade shield to lvl 3
            GameManager.Instance.player.Iterium -= GameManager.Instance.shieldLevel2;
            GameManager.Instance.player.ShieldLvl = 3;
            shield.value = 2;
        }
    }

    void UpgradeFirepower()
    {
        if (GameManager.Instance.player.BulletLvl == 1 && GameManager.Instance.player.Iterium >= GameManager.Instance.firepowerLevel1)
        {
            //Upgrade bullet to lvl 2
            GameManager.Instance.player.Iterium -= GameManager.Instance.firepowerLevel1;
            GameManager.Instance.player.BulletLvl = 2;
            firepower.value = 1;
        }
        else if (GameManager.Instance.player.BulletLvl == 2 && GameManager.Instance.player.Iterium >= GameManager.Instance.firepowerLevel2)
        {
            //Upgrade bullet to lvl 3
            GameManager.Instance.player.Iterium -= GameManager.Instance.firepowerLevel2;
            GameManager.Instance.player.BulletLvl = 3;
            firepower.value = 2;
        }
    }

    void ChangeIterium()
    {
        iterium.text = GameManager.Instance.player.Iterium.ToString();
    }

    void ChangeUpgradeLabels()
    {
        thrustLabelLevel1.text = GameManager.Instance.speedLevel1.ToString();
        thrustLabelLevel2.text = GameManager.Instance.speedLevel2.ToString();
        shieldLabelLevel1.text = GameManager.Instance.shieldLevel1.ToString();
        shieldLabelLevel2.text = GameManager.Instance.shieldLevel2.ToString();
        firepowerLabelLevel1.text = GameManager.Instance.firepowerLevel1.ToString();
        firepowerLabelLevel2.text = GameManager.Instance.firepowerLevel2.ToString();
    }

    void ChangeUpgradeSliders()
    {
        shield.value = GameManager.Instance.player.ShieldLvl - 1;
        thrust.value = GameManager.Instance.player.SpeedLvl - 1;
        firepower.value = GameManager.Instance.player.BulletLvl - 1;
    }
}
