using NUnit.Framework;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    [SerializeField] private List<ItemSO> items;
    [SerializeField] private List<ItemSO> ingredients;
    public Inventory inventory { get; set; }
    Transform IInteractable.transform { get => transform; set => throw new System.NotImplementedException(); }
    InteractType IInteractable.type { get => InteractType.Inventory; set => throw new System.NotImplementedException(); }

    private void Start()
    {
        inventory = GetComponent<Inventory>();

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
