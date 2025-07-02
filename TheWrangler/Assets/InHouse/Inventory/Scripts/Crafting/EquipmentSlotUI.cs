using UnityEngine;

public class EquipmentSlotUI : InventorySlot
{
    [field:SerializeField] public EquipmentSlot slot { get; private set; }

    public override void SetItem(Item item)
    {
        base.SetItem(item);
    }
}
