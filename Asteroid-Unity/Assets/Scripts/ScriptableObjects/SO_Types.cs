using UnityEngine;

[CreateAssetMenu(fileName = "Asteroid", menuName = "Add SO/Common/Type")]

public class SO_Types : ScriptableObject
{
    [SerializeField] private string typeName;
    [TextArea(5, 5)]
    [SerializeField] private string description;

    public string TypeName { get => typeName; set => typeName = value; }
    public string Description { get => description; set => description = value; } 

    //Editor change/update
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(TypeName))
        {
            TypeName = name;
        }
    }
}
