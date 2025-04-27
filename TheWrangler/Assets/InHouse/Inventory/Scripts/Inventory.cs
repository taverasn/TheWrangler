using System;
using System.Collections.Generic;
using System.Linq;
using JUTPS.CharacterBrain;
using Unity.Entities.UniversalDelegates;
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

public class Inventory : MonoBehaviour
{
    public const int HOT_BAR_SIZE = 10;
    [SerializeField] private JUTPS.CharacterBrain.JUCharacterBrain JUCharacter;
    [field: SerializeField] public int size { get; private set; } = 30;
    public Item[] items { get; private set; }
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

    public void Awake()
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

    private void Start()
    {
        JUCharacter = GetComponent<JUCharacterBrain>();
    }

    private void OnEnable()
    {
        onPhysicalItemEquipped += OnPhysicalItemEquipped;
    }

    private void OnDisable()
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

    public void Equip(EquipmentSlot slot, int position = -1, bool equip = true)
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
    }

    private int GetFirstOpenSlot(Item item = null)
    {
        int position = HOT_BAR_SIZE;
        Item compareItem = items.FirstOrDefault(i => i?.info == item.info);
        if (compareItem?.info != null && item.info.stackable)
        {
            position = Array.IndexOf(items, compareItem);
        }
        else
        {
            for (int i = HOT_BAR_SIZE; i < items.Length; i++)
            {
                if (items[i]?.info == null)
                {
                    position = i;
                    break;
                }
            }
        }
        return position;
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

    public void RemoveItem(ItemSO itemSO, int amount)
    {
        Item item = items.Where(i => i?.info == itemSO && i.amount >= amount).FirstOrDefault();
        if (item == null)
        {
            return;
        }

        item.Remove(amount);

        if(item.amount <= 0)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (item == items[i])
                {
                    items[i] = null;
                }
            }
        }
    }

    public bool CanCraft(RecipeSO recipe)
    {
        foreach (KeyValuePair<ItemSO, int> pair in recipe.ingredients)
        {
            if (!items.Any(i => i?.info == pair.Key && i.amount >= pair.Value))
            {
                return false;
            }
        }

        return true;
    }

    public void MoveItem(int oldPosition, int newPosition)
    {
        Item toItem = items[newPosition];
        Item fromItem = items[oldPosition];

        // If the items are the same and can stack add them and delete
        // the item in the old position
        if (toItem?.info == fromItem?.info && fromItem != null && fromItem.info.stackable)
        {
            fromItem.amount += toItem.amount;
            toItem = null;
            items[oldPosition] = null;
            items[newPosition] = fromItem;
        }
        else
        {
            items[oldPosition] = toItem;
            items[newPosition] = fromItem;
        }

        if (oldPosition == lastHotBarSlot)
        {
            JUCharacter.SwitchToItem("");
            ItemEquipped(fromItem, false);
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

    public static int SafeParse(string input)
    {
        bool parsed = int.TryParse(input, out int result);

        return parsed ? result : -1;
    }
}