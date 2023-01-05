using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface that scripts inherit from that save/load data to/from the save system./// 
/// </summary>
public interface ISave 
{
    void LoadData(SaveData saveData);

    void SaveData(ref SaveData saveData);
}
