using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Items/ItemSO")]
[Serializable]
public class ItemSO : ScriptableObject
{
    [field: SerializeField] public string ID { get; private set; }
    public string displayName;
    public EquipmentSlot equipmentSlot;
    public Sprite icon;
    public GameObject prefab;
    public bool stackable;
    public int maxAmount = 300;

    private void OnValidate()
    {
#if UNITY_EDITOR
        ID = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
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
