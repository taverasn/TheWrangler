using CrashKonijn.Goap.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int version;

    public long lastUpdated;
    public int lastHotBarSlot;

    public SerializableDictionary<NeedsOwner, SerializableDictionary<NeedsType, string>> needs;

    public SerializableDictionary<string, SerializableDictionary<int, string>> inventories;
    public SerializableDictionary<string, SerializableDictionary<int, string>> equipments;

    public GameData()
    {
        version = 1;

        needs = new SerializableDictionary<NeedsOwner, SerializableDictionary<NeedsType, string>>();
        inventories = new SerializableDictionary<string, SerializableDictionary<int, string>>();
        equipments = new SerializableDictionary<string, SerializableDictionary<int, string>>();

        lastHotBarSlot = 0;
    }
}
