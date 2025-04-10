using UnityEngine;

public class Idle : State
{
    public Idle(StateMachine machine)
    {
        this.machine = machine;
        logger = new Logger("Idle");
    }

    public override void Arrive()
    {
        base.Arrive();
        Transition<Wander>();
    }
    public override void Exit()
    {
        base.Exit();
    }
}
