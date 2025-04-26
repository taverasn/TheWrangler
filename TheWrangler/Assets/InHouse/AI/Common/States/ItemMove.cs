using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

public class ItemMove : ItemInteract
{
    private Item itemToMove;
    bool itemMoved = false;
    public ItemMove(StateMachine machine)
    {
        this.machine = machine;
        logger = LogManager.Instance.AddLogger("ItemMove", LogLevel.INFO);
    }

    public override void Arrive()
    {
        base.Arrive();
        itemMoved = false;
        MoveItem();
        machine.onDestinationReached += HandleDestinationReached;
    }

    public override void Exit()
    {
        machine.onDestinationReached -= HandleDestinationReached;
    }

    private void HandleDestinationReached()
    {
        if (itemMoved)
        {
            Transition<Idle>();
            return;
        }

        // add that item to that inventory
        itemMoved = true;
        machine.interactable.inventory.AddItem(itemToMove);
        logger.Debug("Item Moved");
        Transition<Idle>();
    }

    private void MoveItem()
    {
        // Grab Item
        List<Item> existingItems = machine.interactable.inventory.items.Where(item => item?.info != null).ToList();

        // If there arent any items in the inventory move on
        if (existingItems.IsNullOrEmpty())
        {
            Transition<Idle>();
            return;
        }

        // Select Random Item
        Item tempItem = existingItems[Random.Range(0, existingItems.Count)];
        itemToMove = new Item(tempItem.amount, tempItem.info);

        // Remove Item
        machine.interactable.inventory.RemoveItem(itemToMove.info, itemToMove.amount);

        // Move Item
        // Search for next nearest inventory thats not the current interactable
        List<Interactable> interactables = TargetFinder.FindTarget(machine.transform.position, 50, LayerMask.GetMask("Default"));

        if (interactables.Count > 0)
        {
            machine.interactable = interactables.FirstOrDefault(i => i != machine.interactable);
            // Set that inventory as the target
            SetDestination(machine.interactable.transform.position);

            logger.Debug($"Destination Set and Ready to Move Item {itemToMove.info.ID}");
        }
    }
}
