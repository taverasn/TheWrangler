using UnityEngine;

public class HotBarSlotUI : InventorySlot
{
    [field: SerializeField] public HotBarSlot slot { get; private set; }
    
    public override void Start()
    {
        index = (int)slot;
        inventoryUI = transform.parent.parent.GetComponent<InventoryUI>();
    }
    public override void SetItem(Item item)
    {
        base.SetItem(item);
    }
}
