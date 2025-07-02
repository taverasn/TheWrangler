using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public InventoryUI inventoryUI;
    public ItemSO itemSO { get; private set; }
    public int index { get; protected set; } = -1;
    [field:SerializeField] protected Image image { get; private set; }
    [field:SerializeField] protected TextMeshProUGUI amountText { get; private set; }
    [field: SerializeField] protected Sprite defaultIcon;
    [SerializeField] private Color emptyColor = new Color(255, 255, 255, 100);

    public virtual void Initialize(InventoryUI inventoryUI, int index = -1, Item item = null)
    {
        this.inventoryUI = inventoryUI;
        this.index = index;
        SetItem(item);
    }

    public virtual void SetItem(Item item)
    {
        if (item == null)
        {
            if (amountText != null) amountText.text = "";
            itemSO = null;
            image.sprite = defaultIcon;
            image.color = emptyColor;
        }
        else
        {
            itemSO = item.info;
            if(amountText != null) amountText.text = item.amount <= 1 ? "" : item.amount.ToString();
            image.sprite = item.info.icon;
            image.color = Color.white;
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
