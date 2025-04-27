using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    protected InventoryUI inventoryUI;
    public ItemSO itemSO { get; private set; }
    public int index { get; protected set; }
    [field:SerializeField] protected Image image { get; private set; }
    [field:SerializeField] protected TextMeshProUGUI text { get; private set; }
    [field:SerializeField] protected TextMeshProUGUI amountText { get; private set; }

    public virtual void Start()
    {
        inventoryUI = transform.parent.parent.parent.GetComponent<InventoryUI>();
    }

    public void Initialize(int index, Item item)
    {
        this.index = index;
        SetItem(item);
    }

    public virtual void SetItem(Item item)
    {
        if (item == null)
        {
            text.text = "";
            amountText.text = "";
            itemSO = null;
        }
        else
        {
            itemSO = item.info;
            text.text = item.info.displayName;
            amountText.text = item.amount == 1 ? "" : item.amount.ToString();
        }
    }

    public void OnSetFromSlot()
    {
        inventoryUI.SetFromSlot(this as InventorySlot);
    }


    public void OnSetToSlot()
    {
        inventoryUI.SetToSlot(this as InventorySlot);
    }
}
