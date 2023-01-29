using UnityEngine.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;


//Faction selection using UI Toolkit
public class UI_FactionSelection : MonoBehaviour
{
    [Header("Target scene")]
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

    //UI Toolkit button names
    [Header("Faction UI buttons")]
    [SerializeField] private string chinaFaction;
    [SerializeField] private string usaFaction;
    [SerializeField] private string ussrFaction;

    [HideInInspector] public string sceneName;

    private void OnEnable()
    {
        VisualElement uiRoot = GetComponent<UIDocument>().rootVisualElement;
        Button buttonChina = uiRoot.Q<Button>(chinaFaction);
        Button buttonUsa = uiRoot.Q<Button>(usaFaction);
        Button buttonUssr = uiRoot.Q<Button>(ussrFaction);

        buttonChina.clicked += ButtonChina_clicked;
        buttonUsa.clicked += ButtonUsa_clicked;
        buttonUssr.clicked += ButtonUssr_clicked;
    }

    //Select faction China and set ship upgrades
    private void ButtonChina_clicked()
    {
        GameManager.Instance.player.Character = GameManager.Instance.factions.Factions[1];
        GameManager.Instance.bulletLvl = GameManager.Instance.player.BulletLvlChn;
        GameManager.Instance.speedLvl = GameManager.Instance.player.SpeedLvlChn;
        GameManager.Instance.shieldLvl = GameManager.Instance.player.ShieldLvlChn;
        LoadScene();
    }

    //Select faction US and set ship upgrades
    private void ButtonUsa_clicked()
    {
        GameManager.Instance.player.Character = GameManager.Instance.factions.Factions[2];
        GameManager.Instance.bulletLvl = GameManager.Instance.player.BulletLvlUs;
        GameManager.Instance.speedLvl = GameManager.Instance.player.SpeedLvlUs;
        GameManager.Instance.shieldLvl = GameManager.Instance.player.ShieldLvlUs;
        LoadScene();
    }

    //Select faction USSR and set ship upgrades
    private void ButtonUssr_clicked()
    {
        GameManager.Instance.player.Character = GameManager.Instance.factions.Factions[3];
        GameManager.Instance.bulletLvl = GameManager.Instance.player.BulletLvlUssr;
        GameManager.Instance.speedLvl = GameManager.Instance.player.SpeedLvlUssr;
        GameManager.Instance.shieldLvl = GameManager.Instance.player.ShieldLvlUssr;
        LoadScene();
    }

    private void LoadScene()
    {
        GameManager.Instance.SaveGame();
        SceneManager.LoadScene(sceneName);
    }
}
