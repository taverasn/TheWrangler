using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    private InventoryUI inventoryUI;
    public int index { get; private set; }
    [field:SerializeField] protected Image image { get; private set; }
    [field:SerializeField] protected TextMeshProUGUI text { get; private set; }
    [field:SerializeField] protected TextMeshProUGUI amountText { get; private set; }

    public void Initialize(int index, Item item)
    {
        inventoryUI = transform.parent.parent.GetComponent<InventoryUI>();
        this.index = index;
        SetItem(item);
    }

    public void SetItem(Item item)
    {
        if (item == null)
        {
            text.text = "";
            amountText.text = "";
        }
        else
        {
            text.text = item.info.displayName;
            amountText.text = item.amount.ToString();
        }
    }

    public void OnSetFromSlot()
    {
        inventoryUI.SetFromSlot(this);
    }


    public void OnSetToSlot()
    {
        inventoryUI.SetToSlot(this);
    }
}
