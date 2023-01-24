using UnityEngine;
using UnityEngine.UIElements;

public class UI_Settings : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private string toggle1Name;
   

    private void OnEnable()
    {
        VisualElement uiRoot = GetComponent<UIDocument>().rootVisualElement;
        Button toggle1 = uiRoot.Q<Button>(toggle1Name);

        //Click events
        toggle1.clicked += Toggle1_clicked;
    }    

    private void Toggle1_clicked()
    {
       
    }
}
