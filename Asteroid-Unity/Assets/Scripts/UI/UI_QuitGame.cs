using UnityEngine.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_QuitGame : MonoBehaviour
{
    [Header("UI button")]
    [SerializeField] private string buttonName = "quit";
   
    private void OnEnable()
    {
        VisualElement uiRoot = GetComponent<UIDocument>().rootVisualElement;
        Button button = uiRoot.Q<Button>(buttonName);
        button.clicked += Button_clicked;
    }

    private void Button_clicked()
    {
        GameManager.Instance.OnApplicationQuit();
    }
}
