using UnityEngine;

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
    [SerializeField] private GameObject bulletLvl1;
    [SerializeField] private GameObject bulletLvl2;
    [SerializeField] private GameObject bulletLvl3;

    public string BulletName { get => bulletName;}
    public string Description { get => description;}
    public Sprite Image { get => image;}
    public float Speed { get => speed;}
    public float FirePower { get => firePower;}
    public GameObject BulletLvl1 { get => bulletLvl1;}
    public GameObject BulletLvl2 { get => bulletLvl2;}
    public GameObject BulletLvl3 { get => bulletLvl3;}
}
