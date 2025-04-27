using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Items/ItemSO")]
[Serializable]
public class ItemSO : ScriptableObject
{
    public string ID;
    public string displayName;
    public List<EquipmentSlot> equipmentSlots;
    public EquipmentType equipmentType;
    public Sprite icon;
    public GameObject prefab;
    public bool stackable;
    public int maxAmount = 300;
}

public enum EquipmentSlot
{
    NONE,
    HEAD,
    CHEST,
    LEGS,
    FEET,
    MAIN_HAND,
    OFF_HAND,
    BOTH_HAND
}

public enum EquipmentType
{
    NONE,
    HEAD,
    CHEST,
    LEGS,
    FEET,
    MAIN_HAND,
    OFF_HAND
}
