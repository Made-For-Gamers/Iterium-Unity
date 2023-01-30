using UnityEngine.UIElements;
using UnityEngine;

public class UI_LoadUpgradeScene : MonoBehaviour
{
    [Header("UI button")]
    [SerializeField] private string upgradeButton;

    private void OnEnable()
    {
        VisualElement uiRoot = GetComponent<UIDocument>().rootVisualElement;
        Button button = uiRoot.Q<Button>(upgradeButton);
        button.clicked += Button_clicked;
    }

    private void Button_clicked()
    {
        GameManager.Instance.SceneUpgrade();
    }
}
