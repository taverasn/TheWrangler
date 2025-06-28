using UnityEngine;

public class HotBarSlotUI : InventorySlot
{
    [field: SerializeField] public HotBarSlot slot { get; private set; }
    
    public void Start()
    {
        index = (int)slot;
    }

    public override void Initialize(InventoryUI inventoryUI, int index = -1, Item item = null)
    {
        this.inventoryUI = inventoryUI;
    }

    public override void SetItem(Item item)
    {
        base.SetItem(item);
    }
}
