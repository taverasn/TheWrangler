using UnityEngine;

public class ItemPickUp : State
{
    public ItemPickUp(StateMachine machine)
    {
        this.machine = machine;
        logger = LogManager.Instance.AddLogger("ItemPickUp", LogLevel.INFO);
    }

    public override void Arrive()
    {
        base.Arrive();
        PickUpItem();
        Transition<Wander>();
    }

    public void PickUpItem()
    {
        machine.interactable.transform.GetComponent<PickUp>().PickUpItem();
        logger.Info("Picked Up Item");
    }
}
