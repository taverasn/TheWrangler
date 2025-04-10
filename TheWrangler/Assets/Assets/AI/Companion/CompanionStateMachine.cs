using NUnit.Framework;
using UnityEngine;

public class CompanionStateMachine : StateMachine
{
    protected override void Awake()
    {
        
        states.Add(new Deciding(this));
        states.Add(new ItemCraft(this));
        states.Add(new ItemMove(this));
    }

    protected override void Update()
    {
        
    }
}
