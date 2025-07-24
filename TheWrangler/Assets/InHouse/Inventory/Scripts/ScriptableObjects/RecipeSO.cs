using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeSO", menuName = "Crafting/RecipeSO")]
public class RecipeSO : ScriptableObject
{
    [field: SerializeField] public string ID { get; private set; }
    public ItemSO item;
    public BuildSection buildSection;

    [ShowInInspector] public ItemAmountDictionary ingredients;

    // ensure the id is always the name of the Scriptable Object asset
    private void OnValidate()
    {
#if UNITY_EDITOR
        ID = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}


[Serializable] public class ItemAmountDictionary : SerializableDictionary<ItemSO, int> { };

public enum CraftingType
{
    WEAPON,
    ARMOR,
    BUILDING,
    RESOURCES,
}

public enum BuildSection
{
    NONE,
    STRUCTURE, 
    CRAFTING, 
    DECORATION, 
    LIGHTING
}