using Sirenix.Utilities;
using UnityEngine;

public class Deciding : State
{
    float interactChance = .5f;
    float craftChance = .5f;

    public Deciding(StateMachine machine)
    {
        this.machine = machine;
        logger = LogManager.Instance.AddLogger("Deciding", LogLevel.INFO);
    }

    public override void Arrive()
    {
        base.Arrive();
        machine.onDestinationReached += HandleDestinationReached;

    }

    private void DecideWhatToDo()
    {
        switch (machine.interactable.type)
        {
            case InteractType.ItemPickUp:
                HandleItemPickUpInteractable();
                break;
            case InteractType.Inventory:
                HandleInventoryInteractable();
                break;
            case InteractType.None:
                logger.Error("There Shouldnt be any Interactables with an Unassigned type. How did you get here???");
                Transition<Wander>();
                break;
        }
    }

    private void HandleItemPickUpInteractable()
    {
        Transition<ItemPickUp>();
    }

    private void HandleInventoryInteractable()
    {
        if (UnityEngine.Random.Range(0f, 1f) < interactChance)
        {
            if (UnityEngine.Random.Range(0f, 1f) < craftChance && !machine.craftableRecipes.IsNullOrEmpty())
            {
                Transition<ItemCraft>();
            }
            else
            {
                Transition<ItemMove>();
            }
        }
        else
        {
            Transition<Wander>();
        }
    }

    private void HandleDestinationReached()
    {
        DecideWhatToDo();
    }

    public override void Exit()
    {
        base.Exit();
        machine.onDestinationReached -= HandleDestinationReached;
    }
}
