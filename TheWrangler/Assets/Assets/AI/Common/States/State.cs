using UnityEngine;
public class State
{
    public StateMachine machine;
    protected Logger logger;

    public State()
    {

    }

    public virtual void Arrive()
    {
        logger.Info("Arrived");
    }

    public virtual void Update(float deltaTime)
    {

    }

    public virtual void Exit()
    {

    }

    protected void Transition<T>()
    {
        Transition(typeof(T));
    }

    protected void Transition(System.Type state)
    {
        machine.TransitionTo(state);
    }
}
