using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemies", menuName = "Add SO/Common/Enemies")]

//Default properties and data for a NPC enemy
public class SO_Enemies : ScriptableObject
{
    [SerializeField] private List<SO_Character> enemies = new List<SO_Character>();

    public List<SO_Character> Enemies { get => enemies; }
}
