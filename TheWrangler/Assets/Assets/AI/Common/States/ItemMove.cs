using UnityEngine;

public class ItemMove : ItemInteract
{
    public ItemMove(StateMachine machine)
    {
        this.machine = machine;
        logger = LogManager.Instance.AddLogger("ItemMove", LogLevel.INFO);
    }

    public override void Arrive()
    {
        base.Arrive();
    }
}
