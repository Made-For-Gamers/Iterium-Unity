using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameObjects", menuName = "Add SO/Common/GameObject List")]

public class SO_GameObjects : ScriptableObject
{
    [SerializeField] private List<GameObject> obj = new List<GameObject>();

    public List<GameObject> Obj { get => obj; }

    public GameObject GetRandomGameObject()
    {
        GameObject randomObj = obj[Random.Range(0, Obj.Count)];
        return randomObj;
    }
}
