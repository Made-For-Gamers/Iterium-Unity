using UnityEngine;

[CreateAssetMenu(fileName = "Asteroid", menuName = "Add SO/Objects/Character")]

public class SO_Character : ScriptableObject
{
    [SerializeField] private string charName;
    [TextArea(5, 5)]
    [SerializeField] private string description;
    [SerializeField] private Sprite image;
    [SerializeField] private string country;
    [SerializeField] private Sprite flag;
    [SerializeField] private SO_Ship ship;

    //Editor change/update
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(charName))
        {
            charName = name;
        }
    }
}
