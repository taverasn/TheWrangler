using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int version;

    public long lastUpdated;

    public SerializableDictionary<string, string> territories;

    public GameData()
    {
        version = 1;

        territories = new SerializableDictionary<string, string>();
    }
}
