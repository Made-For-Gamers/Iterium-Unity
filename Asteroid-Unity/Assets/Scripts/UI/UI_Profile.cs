using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Iterium
{
    //Player profile UI

    public class UI_Profile : MonoBehaviour
    {
        [Header("Scene to load on save")]
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

        [Header("UI Elements")]
        [SerializeField] private string playerName;
        [SerializeField] private string bio;
        [SerializeField] private string email;
        [SerializeField] private string save;

        private TextField nameTextfield;
        private TextField bioTextfield;
        private TextField emailTextfield;

        private Button saveButton;

        private void OnEnable()
        {
            VisualElement uiRoot = GetComponent<UIDocument>().rootVisualElement;
            nameTextfield = uiRoot.Q<TextField>(playerName);
            bioTextfield = uiRoot.Q<TextField>(bio);
            emailTextfield = uiRoot.Q<TextField>(email);
            saveButton = uiRoot.Q<Button>(save);

            //Update field data
            if (!string.IsNullOrEmpty(GameManager.Instance.player.ProfileName))
            {
                nameTextfield.value = GameManager.Instance.player.ProfileName;
            }
            else
            {
                nameTextfield.value = "Player 1";
            }
            if (!string.IsNullOrEmpty(GameManager.Instance.player.Bio))
            {
                bioTextfield.value = GameManager.Instance.player.Bio;
            }
            else
            {
                bioTextfield.value = "Enter a short bio about yourself.";
            }
            if (!string.IsNullOrEmpty(GameManager.Instance.player.Email))
            {
                emailTextfield.value = GameManager.Instance.player.Email;
            }
            else
            {
                emailTextfield.value = "Enter email address";
            }

            //UI Events
            saveButton.clicked += SaveProfile;
        }

        private void OnDisable()
        {
            //Clean-up events
            saveButton.clicked -= SaveProfile;
        }

        private void SaveProfile()
        {
            SoundManager.Instance.PlayEffect(2);
            GameManager.Instance.player.ProfileName = nameTextfield.value;
            GameManager.Instance.player.Bio = bioTextfield.value;
            GameManager.Instance.player.Email = emailTextfield.value;
            GameManager.Instance.SaveGame();
            SceneManager.LoadScene(sceneName);
        }
    }
}