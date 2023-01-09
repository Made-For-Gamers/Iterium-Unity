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
    [SerializeField] private SO_Ship ship;

    public string CharName { get => charName; set => charName = value; }
    public string Description { get => description; set => description = value; }
    public Sprite Image { get => image; set => image = value; }
    public string Country { get => country; set => country = value; }
    public Sprite Flag { get => flag; set => flag = value; }
    public SO_Ship Ship { get => ship; set => ship = value; }
}
