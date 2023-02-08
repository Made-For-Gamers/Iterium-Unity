using UnityEngine.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Iterium
{
    public class UI_QuitGame : MonoBehaviour
    {
        [Header("UI button")]
        [SerializeField] private string buttonName = "quit";
        Button button;

        private void OnEnable()
        {
            VisualElement uiRoot = GetComponent<UIDocument>().rootVisualElement;
            button = uiRoot.Q<Button>(buttonName);
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
            GameManager.Instance.OnApplicationQuit();
        }
    }
}