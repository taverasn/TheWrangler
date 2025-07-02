using System;
using System.Collections.Generic;
using System.Linq;
using JUTPS.CharacterBrain;
using Unity.Entities.UniversalDelegates;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UIElements;

public class Inventory : MonoBehaviour, IDataPersistence
{
    [field:SerializeField] protected string uniqueId = Guid.NewGuid().ToString();
    private bool loadInventoryState;

    [field: SerializeField] public int size { get; protected set; } = 30;
    public Item[] items { get; protected set; }
    public List<Item> combinedItemsList => items.Where(item => item != null && item?.info != null).GroupBy(item => item.info).Select(group => new Item(group.Sum(i => i.amount), group.Key)).ToList();
    public event Action onInventoryUpdated;
    public void InventoryUpdated() => onInventoryUpdated?.Invoke();

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
        InventoryUpdated();
    }

    protected virtual int GetFirstOpenSlot(Item item = null, int startingPosition = 0)
    {
        int position = -1;
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

    public virtual bool CanAddItem(ItemSO itemSO)
    {
        bool canAdd = false;
        Item compareItem = items.FirstOrDefault(i => i?.info == itemSO);
        if (compareItem?.info != null && itemSO.stackable)
        {
            canAdd = true;
        }
        else
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i]?.info == null)
                {
                    canAdd = true;
                    break;
                }
            }
        }
        return canAdd;
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
        InventoryUpdated();
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

        InventoryUpdated();
        return item;
    }

    public (bool, List<Item>) CanCraft(RecipeSO recipe)
    {
        List<Item> ingredients = new List<Item>();
        bool canCraft = true;
        foreach (KeyValuePair<ItemSO, int> pair in recipe.ingredients)
        {
            Item foundItem = combinedItemsList.FirstOrDefault(i => i.info == pair.Key);

            if (foundItem == null)
            {
                canCraft = false;
                continue;
            }
            else
            {
                if (foundItem.amount < pair.Value)
                {
                    canCraft = false;
                }
                ingredients.Add(foundItem);
            }
        }

        return (canCraft, ingredients);
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
        InventoryUpdated();
    }

    public static int SafeParse(string input)
    {
        bool parsed = int.TryParse(input, out int result);

        return parsed ? result : -1;
    }

    public virtual void LoadData(GameData data)
    {
        // Create the quest map
        Dictionary<int, string> allItems = data.inventories.ContainsKey(uniqueId) ? data.inventories[uniqueId] : new SerializableDictionary<int, string>();
        foreach (KeyValuePair<int, string> itemPair in allItems)
        {
            items[itemPair.Key] = LoadItem(itemPair.Value);
        }
    }

    public virtual void SaveData(GameData data)
    {
        SerializableDictionary<int, string> serializedItemDictionary = new SerializableDictionary<int, string>();

        for (int i = 0; i < items.Length; i++)
        {
            serializedItemDictionary.Add(i, SerializedItemString(items[i]));
        }

        if (data.inventories.ContainsKey(uniqueId))
        {
            data.inventories.Remove(uniqueId);
        }
        data.inventories.Add(uniqueId, serializedItemDictionary);
    }

    protected string SerializedItemString(Item item)
    {
        string serializedData = "";
        try
        {
            if (item != null)
            {
                ItemData itemData = item.GetItemData();
                // serialize using JsonUtility
                serializedData = JsonUtility.ToJson(itemData);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save item with id " + item == null ? "DoesNotExist" : item.info.ID + ": " + e);
        }
        return serializedData;
    }

    protected Item LoadItem(string serializedData)
    {
        Item item = null;
        try
        {
            // load quest from saved data
            if (!loadInventoryState)
            {
                if (serializedData == "")
                {
                    item = null;
                }
                else
                {
                    ItemData itemData = JsonUtility.FromJson<ItemData>(serializedData);
                    ItemSO itemSO = GlobalInventoryManager.Instance.allItemSOs[itemData.ID];
                    item = new Item(itemData.amount, itemSO);
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to load " + serializedData + ": " + e);
        }
        return item;
    }
}