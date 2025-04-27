using System;
using System.Collections.Generic;
using System.Linq;
using JUTPS.CharacterBrain;
using Unity.Entities.UniversalDelegates;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UIElements;

public class Inventory : MonoBehaviour
{
    [field: SerializeField] public int size { get; protected set; } = 30;
    public Item[] items { get; protected set; }

    public virtual void Awake()
    {
        items = new Item[size];
    }

    public virtual void Start()
    {
    }

    public virtual void OnEnable()
    {
    }

    public virtual void OnDisable()
    {
    }

    public void AddItem(Item item, int position = -1)
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
    }

    protected virtual int GetFirstOpenSlot(Item item = null, int startingPosition = 0)
    {
        int position = startingPosition;
        Item compareItem = items.FirstOrDefault(i => i?.info == item?.info);
        if (compareItem?.info != null && item.info.stackable)
        {
            position = Array.IndexOf(items, compareItem);
        }
        else
        {
            for (int i = startingPosition; i < items.Length; i++)
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
    
    public Item RemoveItem(int position, int amount, bool all = false)
    {
        Item item = null;
        if (items[position] != null && items[position].info != null)
        {
            item = new Item(items[position].amount, items[position].info);
            if (all)
            {
                items[position].Remove(items[position].amount);
            }
            else
            {
                items[position].Remove(amount);
                item.amount = amount;
            }

            if (items[position].amount <= 0)
            {
                items[position] = null;
            }
        }

        return item;
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

    public virtual void MoveItem(int oldPosition, int newPosition)
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
    }

    public static int SafeParse(string input)
    {
        bool parsed = int.TryParse(input, out int result);

        return parsed ? result : -1;
    }
}