using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Items/ItemSO")]
[Serializable]
public class ItemSO : ScriptableObject
{
    public string ID;
    public string displayName;
    public EquipmentSlot equipmentSlot;
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
