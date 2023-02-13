using UnityEngine.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Iterium
{
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
        [Header("Faction UI Elements")]
        [SerializeField] private string chinaFaction = "chinaFaction";
        [SerializeField] private string usaFaction = "usaFaction";
        [SerializeField] private string ussrFaction = "ussrFaction";

        [Header("Player UI Elements")]
        [SerializeField] private string playerName = "playerStatsText";
        [SerializeField] private string playerXpTotal = "playerXPTotal";
        [SerializeField] private string playerLevel = "playerLevel";
        [SerializeField] private string playerIteriumTotal = "playerIteriumTotal";

        //Ships
        [Header("Ships")]
        [SerializeField] private SO_Ship shipChn;
        [SerializeField] private SO_Ship shipUs;
        [SerializeField] private SO_Ship shipUssr;

        //Progress bars (planes with a shader graph material)
        [Header("Progress Bar Chn")]
        [SerializeField] private GameObject progressChnFirepower;
        [SerializeField] private GameObject progressChnSpeed;
        [SerializeField] private GameObject progressChnShield;

        [Header("Progress Bar Us")]
        [SerializeField] private GameObject progressUsFirepower;
        [SerializeField] private GameObject progressUsSpeed;
        [SerializeField] private GameObject progressUsShield;

        [Header("Progress Bar Ussr")]
        [SerializeField] private GameObject progressUssrFirepower;
        [SerializeField] private GameObject progressUssrSpeed;
        [SerializeField] private GameObject progressUssrShield;

        //Player info labels
        private Label profileName;
        private Label xp;
        private Label level;
        private Label iterium;

        //Faction Buttons
        private Button buttonChina;
        private Button buttonUsa;
        private Button buttonUssr;

        private void OnEnable()
        {
            //Init UI elements
            VisualElement uiRoot = GetComponent<UIDocument>().rootVisualElement;
            buttonChina = uiRoot.Q<Button>(chinaFaction);
            buttonUsa = uiRoot.Q<Button>(usaFaction);
            buttonUssr = uiRoot.Q<Button>(ussrFaction);
            profileName = uiRoot.Q<Label>(playerName);
            xp = uiRoot.Q<Label>(playerXpTotal);
            level = uiRoot.Q<Label>(playerLevel);
            iterium = uiRoot.Q<Label>(playerIteriumTotal);

            //Events
            buttonChina.clicked += ButtonChina_clicked;
            buttonUsa.clicked += ButtonUsa_clicked;
            buttonUssr.clicked += ButtonUssr_clicked;
        }

        private void OnDisable()
        {
            //Clean-up events
            buttonChina.clicked -= ButtonChina_clicked;
            buttonUsa.clicked -= ButtonUsa_clicked;
            buttonUssr.clicked -= ButtonUssr_clicked;
        }

        private void Start()
        {
            //Init ship progress bar values, including any player upgrades

            //Init player elements
            profileName.text = GameManager.Instance.player.ProfileName;
            xp.text = GameManager.Instance.player.Xp.ToString() + " / " + GameManager.Instance.xpLevelSteps * GameManager.Instance.player.Level;
            level.text = GameManager.Instance.player.Level.ToString();
            iterium.text = GameManager.Instance.player.Iterium.ToString();

            //Init Chn firepower slider
            Renderer renderer = progressChnFirepower.GetComponent<Renderer>();
            Material matFirepower = renderer.material;
            matFirepower.SetFloat("_RemovedSeg", 100 - shipChn.Bullet.FirePower * GameManager.Instance.player.BulletLvlChn);

            //Init Chn speed slider
            renderer = progressChnSpeed.GetComponent<Renderer>();
            matFirepower = renderer.material;
            matFirepower.SetFloat("_RemovedSeg", 100 - shipChn.Thrust / 10 / 3 * GameManager.Instance.player.SpeedLvlChn);

            //Init Chn shield slider
            renderer = progressChnShield.GetComponent<Renderer>();
            matFirepower = renderer.material;
            matFirepower.SetFloat("_RemovedSeg", 100 - shipChn.ShieldPower * 15 * GameManager.Instance.player.ShieldLvlChn);

            //Init Us firepower slider
            renderer = progressUsFirepower.GetComponent<Renderer>();
            matFirepower = renderer.material;
            matFirepower.SetFloat("_RemovedSeg", 100 - shipUs.Bullet.FirePower * GameManager.Instance.player.BulletLvlUs);

            //Init Us speed slider
            renderer = progressUsSpeed.GetComponent<Renderer>();
            matFirepower = renderer.material;
            matFirepower.SetFloat("_RemovedSeg", 100 - shipUs.Thrust / 10 / 3 * GameManager.Instance.player.SpeedLvlUs);

            //Init Us shield slider
            renderer = progressUsShield.GetComponent<Renderer>();
            matFirepower = renderer.material;
            matFirepower.SetFloat("_RemovedSeg", 100 - shipUs.ShieldPower * 15 * GameManager.Instance.player.ShieldLvlUs);

            //Init Ussr firepower slider
            renderer = progressUssrFirepower.GetComponent<Renderer>();
            matFirepower = renderer.material;
            matFirepower.SetFloat("_RemovedSeg", 100 - shipUssr.Bullet.FirePower * GameManager.Instance.player.BulletLvlUssr);

            //Init Ussr speed slider
            renderer = progressUssrSpeed.GetComponent<Renderer>();
            matFirepower = renderer.material;
            matFirepower.SetFloat("_RemovedSeg", 100 - shipUssr.Thrust / 10 / 3 * GameManager.Instance.player.SpeedLvlUssr);

            //Init Ussr shield slider
            renderer = progressUssrShield.GetComponent<Renderer>();
            matFirepower = renderer.material;
            matFirepower.SetFloat("_RemovedSeg", 100 - shipUssr.ShieldPower * 15 * GameManager.Instance.player.ShieldLvlUssr);
        }

        //Select faction China
        private void ButtonChina_clicked()
        {
            GameManager.Instance.player.Faction = GameManager.Instance.factions.Factions[1];
            LoadScene();
        }

        //Select faction US
        private void ButtonUsa_clicked()
        {
            GameManager.Instance.player.Faction = GameManager.Instance.factions.Factions[2];
            LoadScene();
        }

        //Select faction USSR
        private void ButtonUssr_clicked()
        {
            GameManager.Instance.player.Faction = GameManager.Instance.factions.Factions[3];
            LoadScene();
        }

        //Load play scene
        private void LoadScene()
        {
            SoundManager.Instance.PlayEffect(2);
            GameManager.Instance.UpgradeLevelSync();
            GameManager.Instance.SaveGame();
            GameManager.Instance.ResetArena();
            SceneManager.LoadScene(sceneName);
        }
    }
}