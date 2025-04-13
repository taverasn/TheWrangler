using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

public class Inventory
{
    public int size { get; private set; }
    private Item[] items;
    private Dictionary<EquipmentSlot, Item> equipment;

    public Inventory(int size)
    {
        this.size = size;
        items = new Item[size];
        equipment = new Dictionary<EquipmentSlot, Item>();
    }

    public void AddItem(Item item, int position = -1, bool equip = false)
    {
        if (position == -1)
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

        if (items[position] == null)
        {
            items[position] = item;
        }
        else
        {
            items[position].Add(item.amount);
        }

        if (equip)
        {
            Equip(position);
        }
    }

    public void Equip(int position)
    {
        Item equipItem = items[position];

        if (equipment.ContainsKey(equipItem.info.equipmentSlot))
        {
            Item unequipItem = equipment[equipItem.info.equipmentSlot];
            items[position] = unequipItem;
        }

        equipment[equipItem.info.equipmentSlot] = equipItem;
    }

    public void UnEquip(EquipmentSlot equipmentSlot)
    {
        if (equipment.ContainsKey(equipmentSlot))
        {
            Item unequipItem = equipment[equipmentSlot];
            equipment.Remove(equipmentSlot);

            AddItem(unequipItem);
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
}