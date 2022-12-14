using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Asteroid", menuName = "Add SO/Common/Players")]

public class SO_Players : ScriptableObject
{
    [SerializeField] private List<SO_Player> players = new List<SO_Player>();

    public List<SO_Player> Players { get => players; }
}
