using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Asteroid", menuName = "Add SO/Objects/Bullet")]

//Default properties and data for a bullet
public class SO_Bullet : ScriptableObject
{
    [SerializeField] private string bulletName;
    [TextArea(5, 5)]
    [SerializeField] private string description;
    [SerializeField] private Sprite image;
    [SerializeField] private float speed;
    [SerializeField] private float firePower;
    [SerializeField] private List<GameObject> bullet = new List<GameObject>();

    public string BulletName { get => bulletName;}
    public string Description { get => description;}
    public Sprite Image { get => image;}
    public float Speed { get => speed;}
    public float FirePower { get => firePower;}
    public List<GameObject> Bullet { get => bullet;}

}
