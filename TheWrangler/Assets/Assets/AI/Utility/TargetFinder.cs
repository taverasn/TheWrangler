using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IInteractable {};

public static class TargetFinder
{
    public static List<Interactable> FindTarget(Vector3 center, float radius, LayerMask targetLayer)
    {
        Collider[] colliders = Physics.OverlapSphere(center, radius, targetLayer);

        List<Interactable> interactables = new List<Interactable>();

        foreach (Collider collider in colliders)
        {
            if(collider.TryGetComponent<Interactable>(out Interactable interactable))
            {
                interactables.Add(interactable);
            }
        }

        return interactables;
    }
    //public static List<IInteractable> FindTarget(Vector3 center, float radius, LayerMask targetLayer, List<AITargetType> ignore_targets)
    //{
    //    List<Character_Base> targets = new List<Character_Base>();
    //    Collider[] hitColliders = Physics.OverlapSphere(center, radius, targetLayer);

    //    foreach (Collider collider in hitColliders)
    //    {
    //        bool ownership_ignored = false;
    //        bool target_ignored = false;
    //        Character_Base target = collider.GetComponent<Character_Base>();
    //        if (target != null)
    //        {
    //            if (ignore_targets.Contains(target.targetType))
    //            {
    //                target_ignored = true;
    //            }

    //            if (target.TryGetComponent<StructurePhysical>(out StructurePhysical structure))
    //            {
    //                if (structure.ownership == ownership_ignore)
    //                {
    //                    ownership_ignored = true;
    //                }
    //            }

    //            if (!target_ignored && !ownership_ignored)
    //            {
    //                targets.Add(target);
    //            }
    //        }
    //    }

    //    return targets;
    //}
}
