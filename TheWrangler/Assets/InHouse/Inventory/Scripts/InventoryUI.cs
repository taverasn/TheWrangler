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
        if (!CanSwap(fromSlot, toSlot))
        {
            fromSlot = null;
            toSlot = null;
            return;
        }

        Item fromItem, toItem;
        EquipmentSlotUI equipmentSlot;
        EquipmentSlotUI equipmentSlot1;

        if (fromSlot.TryGetComponent(out equipmentSlot) && toSlot.TryGetComponent(out equipmentSlot1))
        {
            (fromItem, toItem) = inventory.MoveEquipment(equipmentSlot.slot, equipmentSlot1.slot);
        }
        // If the toSlot is the equipmentSlot we call equip because we're moving something
        // to an equipment slot and vice versa for the fromSlot.
        else if (toSlot.TryGetComponent(out equipmentSlot))
        {
            (fromItem, toItem) = inventory.Equip(equipmentSlot.slot, fromSlot.index);

        } else if (fromSlot.TryGetComponent(out equipmentSlot))
        {
            (fromItem, toItem) = inventory.Equip(equipmentSlot.slot, toSlot.index, false);
        } else
        {
            (fromItem, toItem) = inventory.MoveItem(fromSlot.index, toSlot.index);
        }


        toSlot.SetItem(toItem);
        fromSlot.SetItem(fromItem);

        fromSlot = null;
        toSlot = null;
    }

    private bool CanSwap(InventorySlot fromSlot, InventorySlot toSlot)
    {
        if (fromSlot == null || toSlot == null || fromSlot.itemSO == null)
        {
            return false;
        }

        EquipmentSlotUI equipmentSlot = null;
        EquipmentSlotUI equipmentSlot1 = null;

        // If both Slots are Equipment Slots then check if they're the same type of slot and if they are
        // return true
        if (toSlot.TryGetComponent(out equipmentSlot) && fromSlot.TryGetComponent(out equipmentSlot1))
        {
            if (equipmentSlot.equipmentType == equipmentSlot1.equipmentType)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Check if either slot is an equipment slot.
        // If the fromSlot is an equipment slot that means we are unequiping an item and
        // we need to see if the toSlot has an item because if it does we can only unequip there
        // if its the same type of equipment
        else if (toSlot.TryGetComponent(out equipmentSlot) || (fromSlot.TryGetComponent(out equipmentSlot1) && toSlot.itemSO != null))
        {
            // In addition to checking if the slot and the ones its going to are the same type we also need to make sure that
            // that they are not equal because they are both null. Because if they're equal because they're null we're gonna
            // end up with non equipment items in equipment slots
            bool fromSlotCanSwap = equipmentSlot == null ? false : fromSlot.itemSO == null ? false : fromSlot.itemSO.equipmentSlots.Contains(equipmentSlot.slot);
            bool toSlotCanSwap = equipmentSlot1 == null ? false : toSlot.itemSO == null ? false : toSlot.itemSO.equipmentSlots.Contains(equipmentSlot1.slot);

            if (fromSlotCanSwap || toSlotCanSwap)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // If both slots aren't EquipmentSlots than they should always be able to swap
        return true;
    }
}
