using UnityEngine;

/// <summary>
/// ScriptableObject of the NPC properties
/// </summary>

[CreateAssetMenu(fileName = "NPC", menuName = "Add SO/Objects/NPC")]

public class SO_NPC : ScriptableObject
{
    [SerializeField] private string charName;
    [TextArea(5, 5)]
    [SerializeField] private string description;
    [SerializeField] private Sprite image;
    [SerializeField] private SO_Player ship;
}
