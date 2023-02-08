using UnityEngine.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Iterium
{
    public class UI_Button : MonoBehaviour
    {
        [Header("Drag scene to load")]
#if UNITY_EDITOR
        public UnityEditor.SceneAsset destinationScene;
        private void OnValidate()
        {
            if (destinationScene != null)
            {
                sceneName = destinationScene.name;
            }
        }
#endif
        [HideInInspector] public string sceneName;

        [Header("UI button")]
        [SerializeField] private string buttonName;

        private Button button;

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
            SceneManager.LoadScene(sceneName);
        }
    }
}