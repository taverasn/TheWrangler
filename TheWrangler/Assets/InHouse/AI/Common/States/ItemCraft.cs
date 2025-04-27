using System.Linq;
using UnityEngine;

public class ItemCraft : State
{
    public ItemCraft(StateMachine machine)
    {
        logger = LogManager.Instance.AddLogger("ItemCraft", LogLevel.INFO);
        this.machine = machine;
    }

    public override void Arrive()
    {
        base.Arrive();
        CraftItem();
    }

    private void CraftItem()
    {
        Item item = machine.craftingTable.CraftItem(machine.interactable.inventory, machine.craftableRecipes[UnityEngine.Random.Range(0, machine.craftableRecipes.Count)]);
        logger.Debug($"Crafted {item.info.ID}");

        string msg = item.info.equipmentSlot == EquipmentSlot.NONE ? $"Added {item.info.ID} to Interactable Inventory" : $"Equipped {item.info.ID} to Companion";

        logger.Debug(msg);

        machine.interactable.inventory.AddItem(item, -1, item.info.equipmentSlot);
        
        Transition<Idle>();
    }

    public override void Exit()
    {

    }
}
