using System.Collections.Generic;
using System.Linq;
using JUTPS.InventorySystem;
using JUTPS.JUInputSystem;
using NUnit.Framework;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private JUTPS.CharacterBrain.JUCharacterBrain JUCharacter;
    [SerializeField] private GameObject toggleUI;
    [SerializeField] private GameObject inventorySlotsParent;
    [SerializeField] private GameObject equipmentSlotsParent;
    [SerializeField] private GameObject hotbarSlotsParent;
    [SerializeField] private InventorySlot inventorySlotPrefab;
    private List<InventorySlot> inventorySlots = new List<InventorySlot>();
    private Dictionary<EquipmentSlot, EquipmentSlotUI> equipmentSlots = new Dictionary<EquipmentSlot, EquipmentSlotUI>();
    private Dictionary<HotBarSlot, HotBarSlotUI> hotbarSlots = new Dictionary<HotBarSlot, HotBarSlotUI>();

    private InventorySlot fromSlot;
    private InventorySlot toSlot;

    private JUTPSInputControlls _inputs;

    private void Start()
    {
        if (JUCharacter.Inventory != null)
        {
            for (int i = Inventory.HOT_BAR_SIZE; i < JUCharacter.Inventory.items.Length; i++)
            {
                InventorySlot slot = Instantiate(inventorySlotPrefab);
                slot.transform.SetParent(inventorySlotsParent.transform, false);
                slot.Initialize(i, JUCharacter.Inventory.items[i]);
                inventorySlots.Add(slot);
            }
        }

        foreach (EquipmentSlotUI slot in equipmentSlotsParent.GetComponentsInChildren<EquipmentSlotUI>().ToList())
        {
            equipmentSlots[slot.slot] = slot;
        }
        foreach (HotBarSlotUI slot in hotbarSlotsParent.GetComponentsInChildren<HotBarSlotUI>().ToList())
        {
            hotbarSlots[slot.slot] = slot;
        }
    }

    private void OnEnable()
    {
        _inputs = JUInput.Instance().InputActions;
        _inputs.Player.OpenInventory.started += OnOpenInventory;
    }

    private void OnDisable()
    {
        _inputs.Player.OpenInventory.started -= OnOpenInventory;
    }

    public void OnOpenInventory(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        OnInventoryUpdated();
        toggleUI.SetActive(!toggleUI.activeSelf);
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

        EquipmentSlotUI equipmentSlot;
        EquipmentSlotUI equipmentSlot1;

        if (fromSlot.TryGetComponent(out equipmentSlot) && toSlot.TryGetComponent(out equipmentSlot1))
        {
            JUCharacter.Inventory.MoveEquipment(equipmentSlot.slot, equipmentSlot1.slot);
        }
        // If the toSlot is the equipmentSlot we call equip because we're moving something
        // to an equipment slot and vice versa for the fromSlot.
        else if (toSlot.TryGetComponent(out equipmentSlot))
        {
            JUCharacter.Inventory.Equip(equipmentSlot.slot, fromSlot.index);
        } 
        else if (fromSlot.TryGetComponent(out equipmentSlot))
        {
            JUCharacter.Inventory.Equip(equipmentSlot.slot, toSlot.index, false);
        }
        else
        {
            JUCharacter.Inventory.MoveItem(fromSlot.index, toSlot.index);
        }

        fromSlot = null;
        toSlot = null;

        OnInventoryUpdated();
    }

    private void OnInventoryUpdated()
    {
        for (int i = 0; i < JUCharacter.Inventory.items.Length; i++)
        {
            if (i < 10)
            {
                hotbarSlots[(HotBarSlot)i].SetItem(JUCharacter.Inventory.items[i]);
            }
            else
            {
                inventorySlots[i - Inventory.HOT_BAR_SIZE].SetItem(JUCharacter.Inventory.items[i]);
            }
        }        
        
        foreach (KeyValuePair<EquipmentSlot, Item> slotPair in JUCharacter.Inventory.equipment)
        {
            equipmentSlots[slotPair.Key].SetItem(slotPair.Value);
        }
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
