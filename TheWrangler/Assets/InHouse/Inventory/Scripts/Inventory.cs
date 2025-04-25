using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

public class Inventory
{
    public int size { get; private set; }
    public Item[] items { get; private set; }
    public Dictionary<EquipmentSlot, Item> equipment { get; private set; }

    public Inventory(int size)
    {
        this.size = size;
        items = new Item[size];
        equipment = new Dictionary<EquipmentSlot, Item>();
        foreach (EquipmentSlot slot in Enum.GetValues(typeof(EquipmentSlot)))
        {
            equipment[slot] = null;
        }
    }

    public void AddItem(Item item, int position = -1, EquipmentSlot slot = EquipmentSlot.NONE)
    {
        if (position == -1)
        {
            if (items.Contains(item) && item.info.stackable)
            {
                position = Array.IndexOf(items, item);
            }
            else
            {
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i] == null)
                    {
                        position = i;
                        break;
                    }
                }
            }
        }

        if (items[position] == null)
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

    public (Item, Item) Equip(EquipmentSlot slot, int position, bool equip = true)
    {
        Item equipItem = items[position];
        Item unequipItem = null;

        unequipItem = equipment[slot];

        items[position] = unequipItem;
        equipment[slot] = equipItem;

        if (equip)
        {
            return (unequipItem, equipItem);
        }
        else
        {
            return (equipItem, unequipItem);
        }
    }

    public void RemoveItem(ItemSO itemSO, int amount)
    {
        Item item = items.Where(i => i.info == itemSO && i.amount >= amount).FirstOrDefault();
        if (item == null)
        {
            return;
        }

        item.Remove(amount);

        if(item.amount <= 0)
        {
            item = null;
        }
    }

    public bool CanCraft(RecipeSO recipe)
    {
        foreach (KeyValuePair<ItemSO, int> pair in recipe.ingredients)
        {
            if (!items.Any(i => i.info == pair.Key && i.amount >= pair.Value))
            {
                return false;
            }
        }

        return true;
    }

    public (Item, Item) MoveItem(int oldPosition, int newPosition)
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

        return (toItem, fromItem);
    }
    
    public (Item, Item) MoveEquipment(EquipmentSlot oldSlot, EquipmentSlot newSlot)
    {
        Item itemExistingInNewSpot = equipment[newSlot];
        Item itemToMove = equipment[oldSlot];

        equipment[oldSlot] = itemExistingInNewSpot;
        equipment[newSlot] = itemToMove;

        return (itemExistingInNewSpot, itemToMove);
    }
}