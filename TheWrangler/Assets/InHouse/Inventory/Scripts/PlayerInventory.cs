using System;
using System.Collections.Generic;
using System.Linq;
using JUTPS.CharacterBrain;
using Unity.Entities.UniversalDelegates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public enum HotBarSlot { 
    first = 0, 
    second = 1, 
    third = 2, 
    fourth = 3, 
    fifth = 4, 
    sixth = 5, 
    seventh = 6, 
    eighth = 7, 
    ninth = 8, 
    tenth = 9 }

public class PlayerInventory : Inventory
{
    public const int HOT_BAR_SIZE = 10;
    [SerializeField] private JUTPS.CharacterBrain.JUCharacterBrain JUCharacter;
    public Dictionary<EquipmentSlot, Item> equipment { get; private set; }
    public Item[] mainHandItems => items.Where(i => i?.info != null && i.info.equipmentSlot == EquipmentSlot.MAIN_HAND).ToArray();
    public Item[] offHandItems => items.Where(i => i?.info != null && i.info.equipmentSlot == EquipmentSlot.OFF_HAND).ToArray();
    public Item[] bothHandItems => items.Where(i => i?.info != null && i.info.equipmentSlot == EquipmentSlot.BOTH_HAND).ToArray();

    public PhysicalItem ItemInRightHand { get; private set; }
    public PhysicalItem ItemInLeftHand { get; private set; }

    public bool IsItemEquipped => ItemInLeftHand || ItemInRightHand;
    public bool IsDualWielding => ItemInLeftHand && ItemInRightHand;
    
    public string CurrentRightHandItemID => ItemInRightHand == null ? "" : ItemInRightHand.itemSO.ID;
    public string CurrentLeftHandItemID => ItemInLeftHand == null ? "" : ItemInLeftHand.itemSO.ID;

    public event Action<Item, bool> onItemEquipped;
    public void ItemEquipped(Item item, bool equipped) => onItemEquipped?.Invoke(item, equipped);
    
    public event Action<PhysicalItem, EquipmentSlot> onPhysicalItemEquipped;
    public void PhysicalItemEquipped(PhysicalItem item, EquipmentSlot slot) => onPhysicalItemEquipped?.Invoke(item, slot);

    private int lastHotBarSlot = 0;

    public override void Awake()
    {
        items = new Item[size + HOT_BAR_SIZE];
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = null;
        }

        equipment = new Dictionary<EquipmentSlot, Item>();
        foreach (EquipmentSlot slot in Enum.GetValues(typeof(EquipmentSlot)))
        {
            if (slot == EquipmentSlot.NONE) continue;
            equipment[slot] = null;
            if (slot == EquipmentSlot.FEET) break;
        }
    }

    public override void Start()
    {
        JUCharacter = GetComponent<JUCharacterBrain>();
    }

    public override void OnEnable()
    {
        onPhysicalItemEquipped += OnPhysicalItemEquipped;
    }

    public override void OnDisable()
    {
        onPhysicalItemEquipped -= OnPhysicalItemEquipped;
    }

    public string GetSequentialSlotItemID(HotBarSlot sequentialSlot)
    {
        return items[(int)sequentialSlot]?.info == null ? ((int)sequentialSlot).ToString() : items[(int)sequentialSlot].info.ID;
    }

    private void OnPhysicalItemEquipped(PhysicalItem item, EquipmentSlot slot)
    {
        if (slot == EquipmentSlot.MAIN_HAND)
        {
            ItemInRightHand = item;
        }
        else if (slot == EquipmentSlot.OFF_HAND)
        {
            ItemInLeftHand = item;
        }
    }

    public void AddItem(Item item, int position = -1, EquipmentSlot slot = EquipmentSlot.NONE)
    {
        if (position == -1)
        {
            position = GetFirstOpenSlot(item);
        }

        if (items[position]?.info == null)
        {
            items[position] = item;
        }
        else
        {
            items[position].Add(item.amount);
        }

        if (slot != EquipmentSlot.NONE)
        {
            Equip(slot, position);
        }
    }

    public void Equip(string ID, bool equip = true)
    {
        int slot = SafeParse(ID);

        if (ID == "" || slot != -1)
        {
            ItemInRightHand = null;
            ItemInLeftHand = null;

            if (slot != -1)
            {
                HotBarEquip((HotBarSlot)slot);
                return;
            }
        }

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i]?.info.ID == ID)
            {
                if (i < HOT_BAR_SIZE)
                {
                    HotBarEquip((HotBarSlot)i);
                    break;
                }
                else
                {
                    Equip(items[i].info.equipmentSlot, i, equip);
                    break;
                }
            }
        }
    }

    public void HotBarEquip(HotBarSlot slot, bool equip = true)
    {
        Item equipItem = items[(int)slot];
        Item unequipItem = lastHotBarSlot == -1 ? null : items[lastHotBarSlot];

        lastHotBarSlot = (int)slot;
        ItemEquipped(unequipItem, false);
        ItemEquipped(equipItem, true);
    }

    public Item Equip(EquipmentSlot slot, int position = -1, bool equip = true)
    {
        if (position == -1)
        {
            position = GetFirstOpenSlot();
        }
        
        Item equipItem = items[position];
        Item unequipItem = null;

        unequipItem = equipment[slot];

        items[position] = unequipItem;
        equipment[slot] = equipItem;

        ItemEquipped(unequipItem, false);
        ItemEquipped(equipItem, true);

        return equip ? equipItem : unequipItem;
    }

    protected override int GetFirstOpenSlot(Item item = null, int startingPosition = 0)
    {
        return base.GetFirstOpenSlot(item, HOT_BAR_SIZE);
    }

    public string GetNextEquippableWeapon(string ID)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i]?.info.ID == ID)
            {
                if ((i + 1) < (items.Length - 1))
                {
                    return items[i+1].info.ID;
                }
            }
        }

        return "";
    }
    public string GetPreviousEquippableWeapon(string ID)
    {
        for (int i = items.Length-1; i >= 0; i--)
        {
            if (items[i]?.info.ID == ID)
            {
                if ((i - 1) > 0)
                {
                    return items[i-1].info.ID;
                }
            }
        }

        return "";
    }

    public override void MoveItem(int oldPosition, int newPosition)
    {
        base.MoveItem(oldPosition, newPosition);

        if (oldPosition == lastHotBarSlot)
        {
            JUCharacter.SwitchToItem("");
            ItemEquipped(items[newPosition], false);
        }
        
        if (newPosition == lastHotBarSlot)
        {
            JUCharacter.SwitchToItem(newPosition.ToString());
        }
    }
    
    public void MoveEquipment(EquipmentSlot oldSlot, EquipmentSlot newSlot)
    {
        Item itemExistingInNewSpot = equipment[newSlot];
        Item itemToMove = equipment[oldSlot];

        equipment[oldSlot] = itemExistingInNewSpot;
        equipment[newSlot] = itemToMove;
    }
}