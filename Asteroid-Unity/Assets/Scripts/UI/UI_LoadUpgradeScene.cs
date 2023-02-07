using UnityEngine.UIElements;
using UnityEngine;

namespace Iterium
{
    public class UI_LoadUpgradeScene : MonoBehaviour
    {
        [Header("UI button")]
        [SerializeField] private string upgradeButton = "upgrades";
        Button button;

        private void OnEnable()
        {
            VisualElement uiRoot = GetComponent<UIDocument>().rootVisualElement;
            button = uiRoot.Q<Button>(upgradeButton);

            //Events
            button.clicked += Button_clicked;
        }

        private void OnDisable()
        {
            //Clean-up events
            button.clicked -= Button_clicked;
        }

        private void Button_clicked()
        {
            GameManager.Instance.SceneUpgrade();
        }
    }
}