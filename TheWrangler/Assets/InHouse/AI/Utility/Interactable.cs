using NUnit.Framework;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    [SerializeField] private List<ItemSO> items;
    [SerializeField] private List<ItemSO> ingredients;
    public PlayerInventory inventory { get; private set; }
    private void Start()
    {
        inventory = GetComponent<PlayerInventory>();

        int position = 0;

        foreach (ItemSO itemSO in items)
        {
            Item item = new Item(1, itemSO);
            inventory.AddItem(item, position);
            position++;
        }


        foreach (ItemSO itemSO in ingredients)
        {
            Item item = new Item(5, itemSO);
            inventory.AddItem(item, position);
            position++;
        }
    }
}
