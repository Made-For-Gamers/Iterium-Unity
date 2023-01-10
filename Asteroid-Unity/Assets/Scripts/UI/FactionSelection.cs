using UnityEngine.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;


//Faction selection using UI Toolkit
public class FactionSelection : MonoBehaviour
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
    [Header("UI button")]
    [SerializeField] private string chinaFaction;
    [SerializeField] private string usaFaction;
    [SerializeField] private string ussrFaction;

    private string sceneName;

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

    private void ButtonChina_clicked()
    {
        GameManager.Instance.player.Character = GameManager.Instance.factions.Factions[1];
        LoadScene();
    }

    private void ButtonUsa_clicked()
    {
        GameManager.Instance.player.Character = GameManager.Instance.factions.Factions[2];
        LoadScene();
    }

    private void ButtonUssr_clicked()
    {
        GameManager.Instance.player.Character = GameManager.Instance.factions.Factions[3];
        LoadScene();
    }

    private void LoadScene()
    {
        GameManager.Instance.SaveGame();
        SceneManager.LoadScene(sceneName);
    }
}
