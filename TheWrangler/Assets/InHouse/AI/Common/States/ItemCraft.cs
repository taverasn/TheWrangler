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

        string msg = "";

        if (item.info.equipmentSlot == EquipmentSlot.NONE)
        {
            machine.inventory.Equip(item.info.ID);
            msg = $"Equipped {item.info.ID} to Companion";
        }
        else
        {
            machine.interactable.inventory.AddItem(item);
            msg = $"Added {item.info.ID} to Interactable Inventory";
        }

        logger.Debug(msg);

        Transition<Idle>();
    }

    public override void Exit()
    {

    }
}
