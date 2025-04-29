using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int version;

    public long lastUpdated;
    public int lastHotBarSlot;

    public SerializableDictionary<string, string> territories;

    public SerializableDictionary<string, SerializableDictionary<int, string>> inventories;
    public SerializableDictionary<string, SerializableDictionary<int, string>> equipments;

    public GameData()
    {
        version = 1;

        territories = new SerializableDictionary<string, string>();
        inventories = new SerializableDictionary<string, SerializableDictionary<int, string>>();
        equipments = new SerializableDictionary<string, SerializableDictionary<int, string>>();

        lastHotBarSlot = 0;
    }
}
