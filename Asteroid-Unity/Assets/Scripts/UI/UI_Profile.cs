using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine;

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

    private void OnEnable()
    {
        VisualElement uiRoot = GetComponent<UIDocument>().rootVisualElement;
        nameTextfield = uiRoot.Q<TextField>(playerName);
        bioTextfield = uiRoot.Q<TextField>(bio);
        emailTextfield = uiRoot.Q<TextField>(email);
        Button saveButton = uiRoot.Q<Button>(save);

        //Update field data
        nameTextfield.value = GameManager.Instance.player.ProfileName;
        bioTextfield.value = GameManager.Instance.player.Bio;
        emailTextfield.value = GameManager.Instance.player.Email;

        //UI Events
        saveButton.clicked += SaveProfile;
    }

    private void SaveProfile()
    {
        GameManager.Instance.player.ProfileName = nameTextfield.value;
        GameManager.Instance.player.Bio = bioTextfield.value;
        GameManager.Instance.player.Email = emailTextfield.value;
        GameManager.Instance.SaveGame();
        SceneManager.LoadScene(sceneName);
    }
}