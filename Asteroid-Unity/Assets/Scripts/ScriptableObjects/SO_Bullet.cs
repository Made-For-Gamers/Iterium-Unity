using UnityEngine;

[CreateAssetMenu(fileName = "Asteroid", menuName = "Add SO/Objects/Bullet")]

public class SO_Bullet : ScriptableObject
{
    [SerializeField] private string bulletName;
    [TextArea(5, 5)]
    [SerializeField] private string description;
    [SerializeField] private Sprite image;
    [SerializeField] private float speed;
    [SerializeField] private float firePower;

    public string BulletName { get => bulletName; set => bulletName = value; }
    public string Description { get => description; set => description = value; }
    public Sprite Image { get => image; set => image = value; }
    public float Speed { get => speed; set => speed = value; }
    public float FirePower { get => firePower; set => firePower = value; }


    //Editor change/update
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(BulletName))
        {
            BulletName = name;
        }
    }
}
