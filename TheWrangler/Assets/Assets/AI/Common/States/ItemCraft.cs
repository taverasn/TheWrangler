using UnityEngine;

public class ItemCraft : State
{
    public ItemCraft(StateMachine machine)
    {
        logger = new Logger("ItemCraft");
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


        if (item.info.equipmentSlot != EquipmentSlot.NONE)
        {
            logger.Debug($"Equipped {item.info.ID} to Companion");

            machine.inventory.AddItem(item, -1, true);
        }
        else
        {
            logger.Debug($"Added {item.info.ID} to Interactable Inventory");

            machine.interactable.inventory.AddItem(item);
        }
        Transition<Idle>();
    }

    public override void Exit()
    {

    }
}
