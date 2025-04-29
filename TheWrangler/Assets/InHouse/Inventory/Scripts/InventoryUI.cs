using System;
using System.Collections.Generic;
using System.Linq;
using JUTPS;
using JUTPS.CameraSystems;
using JUTPS.InventorySystem;
using JUTPS.JUInputSystem;
using NUnit.Framework;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [field:SerializeField] protected Inventory inventory;
    [field:SerializeField] protected GameObject inventorySlotsParent;
    [field:SerializeField] protected GameObject toggleUI;
    [field:SerializeField] protected InventorySlot inventorySlotPrefab;
    protected List<InventorySlot> inventorySlots = new List<InventorySlot>();

    protected InventorySlot fromSlot;
    protected InventorySlot toSlot;

    protected virtual void Start()
    {
        SetUp();
    }

    public virtual void OnEnable()
    {
        GameEventsManager.Instance.UIEvents.onMoveItemToSeparateInventory += MoveItemToSeparateInventory;
        GameEventsManager.Instance.UIEvents.onRequestItemFromSeparateInventory += OnRequestItemFromSeparateInventory;
        GameEventsManager.Instance.UIEvents.onCancelRequestItemFromSeparateInventory += OnCancelRequestItemFromSeparateInventory;
    }

    public virtual void OnDisable()
    {
        GameEventsManager.Instance.UIEvents.onMoveItemToSeparateInventory -= MoveItemToSeparateInventory;
        GameEventsManager.Instance.UIEvents.onRequestItemFromSeparateInventory -= OnRequestItemFromSeparateInventory;
        GameEventsManager.Instance.UIEvents.onCancelRequestItemFromSeparateInventory -= OnCancelRequestItemFromSeparateInventory;
    }

    public virtual void MoveItemToSeparateInventory(Item item, bool toPlayer, bool swapItemsBothWays)
    {
        if (toPlayer && this is PlayerInventoryUI || !toPlayer && this is not PlayerInventoryUI)
        {
            if (toggleUI.activeSelf)
            {
                if (swapItemsBothWays)
                {
                    Item swapItem = null;
                    if (toSlot is EquipmentSlotUI)
                    {
                        PlayerInventory playerInventory = inventory as PlayerInventory;
                        swapItem = playerInventory.Equip(((EquipmentSlotUI)toSlot).slot, toSlot.index, false);
                        swapItem = inventory.RemoveItem(Array.IndexOf(inventory.items, swapItem), 0, true);
                    }
                    else
                    {
                        swapItem = inventory.RemoveItem(toSlot.index, 0, true);
                    }
                    GameEventsManager.Instance.UIEvents.MoveItemToSeparateInventory(swapItem, this is PlayerInventoryUI ? false : true, false);
                }

                inventory.AddItem(item, toSlot.index);

                if (toSlot is EquipmentSlotUI && inventory is PlayerInventory)
                {
                    PlayerInventory playerInventory = inventory as PlayerInventory;
                    playerInventory.Equip(item.info.ID);
                }


                toSlot.SetItem(item);
                toSlot = null;
            }
        }
    }

    public virtual void OnRequestItemFromSeparateInventory(bool swapItemsBothWays, InventorySlot swapCheckSlot)
    {
        if(toggleUI.activeSelf && fromSlot != null)
        {
            if (!PlayerInventoryUI.CanSwap(fromSlot, swapCheckSlot))
            {
                fromSlot = null;
                GameEventsManager.Instance.UIEvents.CancelRequestItemFromSeparateInventory();
                return;
            }

            Item swapItem = null;
            if (fromSlot is EquipmentSlotUI)
            {
                PlayerInventory playerInventory = inventory as PlayerInventory;
                swapItem = playerInventory.Equip(((EquipmentSlotUI)fromSlot).slot, fromSlot.index, false);
                swapItem = inventory.RemoveItem(Array.IndexOf(inventory.items, swapItem), 0, true);
            }
            else
            {
                swapItem = inventory.RemoveItem(fromSlot.index, 0, true);
            }

            if (swapItemsBothWays)
            {
                toSlot = fromSlot;
            }
            else
            {
                fromSlot.SetItem(null);
            }
            fromSlot = null;

            GameEventsManager.Instance.UIEvents.MoveItemToSeparateInventory(swapItem, this is PlayerInventoryUI ? false : true, swapItemsBothWays);
        }
    }

    public virtual void OnCancelRequestItemFromSeparateInventory()
    {
        fromSlot = null;
        toSlot = null;
    }


    protected virtual void SetUp()
    {
        for (int i = 0; i < inventory.items.Length; i++)
        {
            InventorySlot slot = Instantiate(inventorySlotPrefab);
            slot.transform.SetParent(inventorySlotsParent.transform, false);
            slot.Initialize(i, inventory.items[i]);
            inventorySlots.Add(slot);
        }

        OnInventoryUpdated();
    }

    public void EnableUI()
    {
        JUCameraController.LockMouse(toggleUI.activeSelf, toggleUI.activeSelf);
        toggleUI.SetActive(!toggleUI.activeSelf);
        OnInventoryUpdated();
    }

    public void SetFromSlot(InventorySlot slot)
    {
        this.fromSlot = slot;
    }


    public void SetToSlot(InventorySlot slot)
    {
        this.toSlot = slot;
        if (fromSlot == null)
        {
            GameEventsManager.Instance.UIEvents.RequestItemFromSeparateInventory(slot.itemSO == null ? false : true, toSlot);
        }
        else
        {
            BeginItemSwap();
        }
    }

    public virtual void BeginItemSwap()
    {
        if (!CanSwap(fromSlot, toSlot))
        {
            fromSlot = null;
            toSlot = null;
            return;
        }

        inventory.MoveItem(fromSlot.index, toSlot.index);

        fromSlot = null;
        toSlot = null;

        OnInventoryUpdated();
    }

    private void OnInventoryUpdated()
    {
        for (int i = 0; i < inventory.items.Length; i++)
        {
            inventorySlots[i]?.SetItem(inventory.items[i]);
        }
    }

    public static bool CanSwap(InventorySlot fromSlot, InventorySlot toSlot)
    {
        if (fromSlot == null || toSlot == null || fromSlot.itemSO == null)
        {
            return false;
        }
        return true;
    }
}
