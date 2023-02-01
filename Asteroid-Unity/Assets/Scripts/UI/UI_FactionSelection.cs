using UnityEngine.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;

//Faction selection using UI Toolkit
//Cameras projecting ships on 3 RenderTextures in the UI
//Planes in front of each camera displaying 3 ShaderGraph circlular sliders

public class UI_FactionSelection : MonoBehaviour
{
    [Header("Play Scene")]
#if UNITY_EDITOR
    public UnityEditor.SceneAsset targetScene;
    private void OnValidate()
    {
        if (targetScene != null)
        {
            sceneName = targetScene.name;
        }
    }
#endif 
    [HideInInspector] public string sceneName;

    //UI element names
    [Header("Faction UI buttons")]
    [SerializeField] private string chinaFaction;
    [SerializeField] private string usaFaction;
    [SerializeField] private string ussrFaction;

    //Ships
    [Header("Ships")]
    [SerializeField] private SO_Ship shipChn;
    [SerializeField] private SO_Ship shipUs;
    [SerializeField] private SO_Ship shipUssr;

    //Slider planes (shader graph materials)
    [Header("Sliders Chn")]
    [SerializeField] private GameObject sliderChnFirepower;
    [SerializeField] private GameObject sliderChnSpeed;
    [SerializeField] private GameObject sliderChnShield;

    [Header("Sliders Us")]
    [SerializeField] private GameObject sliderUsFirepower;
    [SerializeField] private GameObject sliderUsSpeed;
    [SerializeField] private GameObject sliderUsShield;

    [Header("Sliders Ussr")]
    [SerializeField] private GameObject sliderUssrFirepower;
    [SerializeField] private GameObject sliderUssrSpeed;
    [SerializeField] private GameObject sliderUssrShield;


    private void OnEnable()
    {
        //Init UI elements
        VisualElement uiRoot = GetComponent<UIDocument>().rootVisualElement;
        Button buttonChina = uiRoot.Q<Button>(chinaFaction);
        Button buttonUsa = uiRoot.Q<Button>(usaFaction);
        Button buttonUssr = uiRoot.Q<Button>(ussrFaction);

        //Events
        buttonChina.clicked += ButtonChina_clicked;
        buttonUsa.clicked += ButtonUsa_clicked;
        buttonUssr.clicked += ButtonUssr_clicked;
    }

    private void Start()
    {
        //Init Chn firepower slider
        Renderer renderer = sliderChnFirepower.GetComponent<Renderer>();
        Material matFirepower = renderer.material;
        matFirepower.SetFloat("_RemovedSeg", 100 - (shipChn.Bullet.FirePower * GameManager.Instance.player.BulletLvlChn));

        //Init Chn speed slider
        renderer = sliderChnSpeed.GetComponent<Renderer>();
        matFirepower = renderer.material;
        matFirepower.SetFloat("_RemovedSeg", 100 - (((shipChn.Thrust / 10) / 3) * GameManager.Instance.player.SpeedLvlChn));

        //Init Chn shield slider
        renderer = sliderChnShield.GetComponent<Renderer>();
        matFirepower = renderer.material;
        matFirepower.SetFloat("_RemovedSeg", 100 - ((shipChn.ShieldPower * 15) * GameManager.Instance.player.ShieldLvlChn));

        //Init Us firepower slider
        renderer = sliderUsFirepower.GetComponent<Renderer>();
        matFirepower = renderer.material;
        matFirepower.SetFloat("_RemovedSeg", 100 - (shipUs.Bullet.FirePower * GameManager.Instance.player.BulletLvlUs));

        //Init Us speed slider
        renderer = sliderUsSpeed.GetComponent<Renderer>();
        matFirepower = renderer.material;
        matFirepower.SetFloat("_RemovedSeg", 100 - (((shipUs.Thrust / 10) / 3) * GameManager.Instance.player.SpeedLvlUs));

        //Init Us shield slider
        renderer = sliderUsShield.GetComponent<Renderer>();
        matFirepower = renderer.material;
        matFirepower.SetFloat("_RemovedSeg", 100 - ((shipUs.ShieldPower * 15) * GameManager.Instance.player.ShieldLvlUs));

        //Init Ussr firepower slider
        renderer = sliderUssrFirepower.GetComponent<Renderer>();
        matFirepower = renderer.material;
        matFirepower.SetFloat("_RemovedSeg", 100 - (shipUssr.Bullet.FirePower * GameManager.Instance.player.BulletLvlUssr));

        //Init Ussr speed slider
        renderer = sliderUssrSpeed.GetComponent<Renderer>();
        matFirepower = renderer.material;
        matFirepower.SetFloat("_RemovedSeg", 100 - (((shipUssr.Thrust / 10) / 3) * GameManager.Instance.player.SpeedLvlUssr));

        //Init Ussr shield slider
        renderer = sliderUssrShield.GetComponent<Renderer>();
        matFirepower = renderer.material;
        matFirepower.SetFloat("_RemovedSeg", 100 - ((shipUssr.ShieldPower * 15) * GameManager.Instance.player.ShieldLvlUssr));
    }

    //Select faction China and set ship upgrades
    private void ButtonChina_clicked()
    {
        GameManager.Instance.player.Character = GameManager.Instance.factions.Factions[1];
        LoadScene();
    }

    //Select faction US and set ship upgrades
    private void ButtonUsa_clicked()
    {
        GameManager.Instance.player.Character = GameManager.Instance.factions.Factions[2];
        LoadScene();
    }

    //Select faction USSR and set ship upgrades
    private void ButtonUssr_clicked()
    {
        GameManager.Instance.player.Character = GameManager.Instance.factions.Factions[3];
        LoadScene();
    }

    //Load play scene
    private void LoadScene()
    {
        GameManager.Instance.UpgradeLevelSync();
        GameManager.Instance.SaveGame();
        GameManager.Instance.NewArena();
        SceneManager.LoadScene(sceneName);
    }
}
