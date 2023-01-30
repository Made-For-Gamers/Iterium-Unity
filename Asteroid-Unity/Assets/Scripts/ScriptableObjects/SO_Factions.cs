using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Asteroid", menuName = "Add SO/Common/Factions")]

//List of faction characters
//Do NOT change the index of items, used by UI_FactionSelection
public class SO_Factions : ScriptableObject
{
    [SerializeField] private List<SO_Character> factions = new List<SO_Character>();

    public List<SO_Character> Factions { get => factions;}
}
