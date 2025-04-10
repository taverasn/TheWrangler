using UnityEngine;

public class ItemMove : ItemInteract
{
    public ItemMove(StateMachine machine)
    {
        this.machine = machine;
        logger = new Logger("ItemMove");
    }

    public override void Arrive()
    {
        base.Arrive();
    }
}
