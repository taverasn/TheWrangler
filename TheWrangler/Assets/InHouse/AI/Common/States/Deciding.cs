using Sirenix.Utilities;
using UnityEngine;

public class Deciding : State
{
    float interactChance = 0.5f;
    float craftChance = 0.5f;

    public Deciding(StateMachine machine)
    {
        this.machine = machine;
        logger = LogManager.Instance.AddLogger("Deciding", LogLevel.INFO);
    }

    public override void Arrive()
    {
        base.Arrive();
        DecideWhatToDo();
    }

    private void DecideWhatToDo()
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


    public override void Exit()
    {
        base.Exit();
    }
}
