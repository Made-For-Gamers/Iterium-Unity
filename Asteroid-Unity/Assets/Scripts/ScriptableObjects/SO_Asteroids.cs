using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Asteroid", menuName = "Add SO/Common/Asteroids")]

public class SO_Asteroids : ScriptableObject
{
    [SerializeField] private List<GameObject> asteroids = new List<GameObject>();

    public List<GameObject> Asteroids { get => asteroids; }
}
