using UnityEngine;

// used as an interface: used to describe the methods that implementing scripts needs to have
public interface IDataPersistence
{
    void LoadData(GameData data);               // no reference bc we only need to read the data

    void SaveData(ref GameData data);           // reference bc we want to allow the implementing script to modify the data

}
