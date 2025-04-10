using NUnit.Framework;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    public Inventory inventory;
    [SerializeField] private RecipeSO recipe;

    private void Awake()
    {
        inventory = new Inventory(30);
        
        int position = 0;

        foreach (KeyValuePair<ItemSO, int> itemSO in recipe.ingredients)
        {
            Item item = new Item(itemSO.Value, itemSO.Key);

            inventory.AddItem(item, position);
            position++;
        }
    }
}
