using UnityEngine.UIElements;
using UnityEngine;

namespace Iterium
{
    //Loads the selected faction ship upgrade scene

    public class UI_LoadUpgradeScene : MonoBehaviour
    {
        [Header("UI button")]
        [SerializeField] private string upgradeButton = "upgrades";
        private Button button;

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
            SoundManager.Instance.PlayEffect(2);
            GameManager.Instance.SceneUpgrade();
        }
    }
}