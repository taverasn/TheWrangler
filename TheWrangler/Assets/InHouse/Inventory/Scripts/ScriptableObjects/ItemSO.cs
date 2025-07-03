using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Items/ItemSO")]
[Serializable]
public class ItemSO : ScriptableObject
{
    [field: SerializeField] public string ID { get; private set; }
    [Header("UI Settings")] 
    public string displayName;
    public GameObject prefab;
    public HoverBehavior hoverBehavior; 
    [field: SerializeField] public GameObject pickUpPrefab;
    public Sprite icon;
    [Header("Amount Settings")]
    public int maxAmount = 300;
    public bool stackable;
    [Header("Holdable Settings")]
    public EquipmentSlot equipmentSlot;
    [Header("Tool Settings")]
    public ToolType toolType;
    public Tier tier;

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
    BOTH_HAND,
    BACK_PACK
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

public enum HoverBehavior { 
    FallToGround, 
    Hover, 
    ManualPlacement 
} 
