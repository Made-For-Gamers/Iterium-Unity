using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Asteroid", menuName = "Add SO/Common/Factions")]

//List of faction characters
//Do NOT change the index of items, used by UI_FactionSelection
public class SO_FactionList : ScriptableObject
{
    [SerializeField] private List<SO_Faction> factions = new List<SO_Faction>();

    public List<SO_Faction> Factions { get => factions;}
}
