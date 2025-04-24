using NUnit.Framework;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Interactable inventoryHolder;
    private Inventory inventory;
    [SerializeField] private GameObject inventorySlotsParent;
    [SerializeField] private GameObject equipmentSlotsParent;
    [SerializeField] private InventorySlot inventorySlotPrefab;

    private InventorySlot fromSlot;
    private InventorySlot toSlot;

    private void Start()
    {
        inventory = inventoryHolder.inventory;
        if (inventory != null)
        {
            for (int i = 0; i < inventory.size; i++)
            {
                InventorySlot slot = Instantiate(inventorySlotPrefab);
                slot.transform.SetParent(inventorySlotsParent.transform, false);
                slot.Initialize(i, inventory.items[i]);
            }
        }
    }

    public void SetFromSlot(InventorySlot slot)
    {
        this.fromSlot = slot;
    }


    public void SetToSlot(InventorySlot slot)
    {
        this.toSlot = slot;
        BeginItemSwap();
    }

    public void BeginItemSwap()
    {
        if (fromSlot == null || toSlot == null)
        {
            fromSlot = null;
            toSlot = null;
            return;
        }

        (Item fromItem, Item toItem) = inventory.MoveItem(fromSlot.index, toSlot.index);

        toSlot.SetItem(toItem);
        fromSlot.SetItem(fromItem);

        fromSlot = null;
        toSlot = null;
    }
}
