using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameData
{
    public int fightCount;
    public Vector3 playerPosition;

    public SerializableDictionary<string, bool> enemiesFought;

    public GameData()
    {
        this.fightCount = 0;        // initialize it to be zero
        playerPosition = new Vector3(-332.33f, -157.89f, -4.2f);

        enemiesFought = new SerializableDictionary<string, bool>(); 
    }
}
