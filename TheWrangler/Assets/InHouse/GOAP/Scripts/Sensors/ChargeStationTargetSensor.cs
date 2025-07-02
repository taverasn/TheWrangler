using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Runtime;
using System.Linq;
using TheWrangler.GOAP.Interfaces;
using UnityEngine;

namespace TheWrangler.GOAP
{
    public class ChargeStationTargetSensor : LocalTargetSensorBase, IInjectable
    {
        private NeedsConfigSO needsConfig;
        private Collider[] colliders = new Collider[1];
        public override void Created()
        {
        }

        public void Inject(DependencyInjector dependencyInjector)
        {
            needsConfig = dependencyInjector.needsConfig;
        }

        public override ITarget Sense(IActionReceiver agent, IComponentReference references, ITarget existingTarget)
        {
            Vector3 agentPosition = agent.Transform.position;
            int hits = Physics.OverlapSphereNonAlloc(agentPosition, needsConfig.NeedsSearchRadius, colliders, needsConfig.NeedsLayer);

            if (hits == 0)
                return null;


            for (int i = colliders.Length - 1; i > hits; i--)
            {
                colliders[i] = null;
            }

            colliders = colliders.OrderBy(collider => collider == null ? float.MaxValue : (collider.transform.position - agent.Transform.position).sqrMagnitude).ToArray();

            return new PositionTarget(colliders[0].transform.position);
        }

        public override void Update()
        {
        }
    }
}