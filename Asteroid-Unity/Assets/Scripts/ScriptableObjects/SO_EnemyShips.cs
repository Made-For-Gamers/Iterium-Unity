using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Asteroid", menuName = "Add SO/Common/Enemies")]

public class SO_EnemyShips : ScriptableObject
{
    [SerializeField] private List<SO_Character> enemyShips = new List<SO_Character>();

    public List<SO_Character> EnemyShips { get => enemyShips; }
}
