using UnityEngine;

[CreateAssetMenu(fileName = "Asteroid", menuName = "Add SO/Objects/Character")]

//Default properties and data for a faction character
public class SO_Character : ScriptableObject
{
    [SerializeField] private string charName;
    [TextArea(5, 5)]
    [SerializeField] private string description;
    [SerializeField] private Sprite image;
    [SerializeField] private string country;
    [SerializeField] private Sprite flag;
    [SerializeField] private SO_Player ship;
   
}
