using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Wander : State
{
    private float walkRadius = 10f;

    public Wander(float walkRadius, StateMachine machine)
    {
        this.walkRadius = walkRadius;
        this.machine = machine;
        logger = new Logger("Wander");
    }

    public override void Arrive()
    {
        machine.onDestinationReached += HandleDestinationReached;
    }

    private void HandleDestinationReached()
    {
        if (FoundAndSetInteractableTarget())
        {
            Transition<Deciding>();
        }
        else
        {
            ChooseRandomTargetLocation();
        }
    }

    private void ChooseRandomTargetLocation()
    {
        float randX = UnityEngine.Random.Range(-walkRadius, walkRadius);
        float randZ = UnityEngine.Random.Range(-walkRadius, walkRadius);
        Vector3 position = machine.target.position;
        SetDestination(new Vector3(position.x + randX, position.y, position.z + randZ));
    }

    private bool FoundAndSetInteractableTarget()
    {
        if (!machine.canInteract)
        {
            return false;
        }

        List<Interactable> interactables = TargetFinder.FindTarget(machine.transform.position, walkRadius, LayerMask.GetMask("Default"));

        if (interactables.Count > 0)
        {
            Transform target = interactables.First().transform;
            SetDestination(target.position);

            machine.interactable = interactables.First();
            
            machine.StartCoroutine(machine.StartCanInteractCoolDown());

            return true;
        }
        else
        {
            return false;
        }
    }

    private void SetDestination(Vector3 position)
    {
        machine.target.position = position;
    }

    public override void Exit()
    {
        machine.onDestinationReached -= HandleDestinationReached;
    }
}
