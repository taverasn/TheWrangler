using UnityEngine;

public class Idle : State
{
    public Idle(StateMachine machine)
    {
        this.machine = machine;
        logger = LogManager.Instance.AddLogger("Idle", LogLevel.INFO);
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
