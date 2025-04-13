using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeSO", menuName = "Crafting/RecipeSO")]
public class RecipeSO : ScriptableObject
{
    public ItemSO item;

    [ShowInInspector] public ItemAmountDictionary ingredients;
}

[Serializable] public class ItemAmountDictionary : SerializableDictionary<ItemSO, int> { };
