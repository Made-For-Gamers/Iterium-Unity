using UnityEngine;

[CreateAssetMenu(fileName = "Asteroid", menuName = "Add SO/Objects/Ship")]

public class SO_Ship : ScriptableObject
{
    [SerializeField] private string shipName;
    [TextArea(5, 5)]
    [SerializeField] private string description;
    [SerializeField] private Sprite image;
    [SerializeField] private SO_Types faction;
    [SerializeField] private int health;
    [SerializeField] private float shieldPower;
    [SerializeField] private float shieldCooldown;
    [SerializeField] private float shieldTime;
    [SerializeField] private float thrust;  
    [SerializeField] private float turnSpeed;
    [SerializeField] private SO_Bullet bullet;
    [SerializeField] private GameObject shipPrefab;

    public string ShipName { get => shipName; set => shipName = value; }
    public string Description { get => description; set => description = value; }
    public Sprite Image { get => image; set => image = value; }
    public SO_Types Faction { get => faction; set => faction = value; }
    public int Health { get => health; set => health = value; }
    public float ShieldPower { get => shieldPower; set => shieldPower = value; }
    public float ShieldCooldown { get => shieldCooldown; set => shieldCooldown = value; }
    public float ShieldTime { get => shieldTime; set => shieldTime = value; }
    public float Thrust { get => thrust; set => thrust = value; }
    public float TurnSpeed { get => turnSpeed; set => turnSpeed = value; }
    public SO_Bullet Bullet { get => bullet; set => bullet = value; }
    public GameObject ShipPrefab { get => shipPrefab; set => shipPrefab = value; }


    //Editor change/update
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(ShipName))
        {
            ShipName = name;
        }
    }
}
