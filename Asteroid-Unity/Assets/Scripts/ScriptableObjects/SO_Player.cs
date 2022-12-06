using UnityEngine;

[CreateAssetMenu(fileName = "Asteroid", menuName = "Add SO/Objects/Player")]

public class SO_Player : ScriptableObject
{
    [SerializeField] private string charName;
    [TextArea(5, 5)]
    [SerializeField] private string description;
    [SerializeField] private int xp;

    //Editor change/update
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(charName))
        {
            charName = name;
        }
    }
}
