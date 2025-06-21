using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Items/ItemSO")]
[Serializable]
public class ItemSO : ScriptableObject
{
    [field: SerializeField] public string ID { get; private set; }
    public string displayName;
    public GameObject prefab;
    public Sprite icon;
    public int maxAmount = 300;
    public bool stackable;
    public EquipmentSlot equipmentSlot;

    private void OnValidate()
    {
#if UNITY_EDITOR
        ID = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}

public enum Tier
{
    COMMON,
    RARE,
    LEGENDARY
}

public enum ToolType
{
    ANY,
    AXE,
    PICKAXE,
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
