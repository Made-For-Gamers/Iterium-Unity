using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISave 
{
    void LoadData(SaveData saveData);

    void SaveData(ref SaveData saveData);
}
